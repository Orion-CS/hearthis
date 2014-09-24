// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2014, SIL International. All Rights Reserved.
// <copyright from='2011' to='2014' company='SIL International'>
//		Copyright (c) 2014, SIL International. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (http://sil.mit-license.org/)
// </copyright>
#endregion
// --------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using L10NSharp;
using Palaso.CommandLineProcessing;
using Palaso.IO;
using Palaso.Extensions;
using Palaso.Progress;
using Palaso.Reporting;
using HearThis.Script;

namespace HearThis.Publishing
{
	/// <summary>
	/// Each script block is recorded and each clip stored as its own file.  This class manages that collection of files.
	/// </summary>
	public static class ClipRepository
	{
		private static string _sHearThisFolder;

		#region Retrieval and Deletion methods

		public static string GetPathToLineRecording(string projectName, string bookName, int chapterNumber, int lineNumber)
		{
			var chapter = GetChapterFolder(projectName, bookName, chapterNumber);
			return Path.Combine(chapter, lineNumber + ".wav");
		}

		public static bool GetHaveClip(string projectName, string bookName, int chapterNumber, int lineNumber)
		{
			var path = GetPathToLineRecording(projectName, bookName, chapterNumber, lineNumber);
			return File.Exists(path);
		}

		public static string GetChapterFolder(string projectName, string bookName, int chapterNumber)
		{
			var book = GetBookFolder(projectName, bookName);
			var chapter = CreateDirectoryIfNeeded(book, chapterNumber.ToString());
			return chapter;
		}

		private static string GetBookFolder(string projectName, string bookName)
		{
			var project = GetApplicationDataFolder(projectName);
			var book = CreateDirectoryIfNeeded(project, bookName.Trim());
			return book;
		}

		public static int GetCountOfRecordingsInFolder(string path)
		{
			if (!Directory.Exists(path))
				return 0;
			return GetSoundFilesInFolder(path).Length;
		}

		public static int GetCountOfRecordingsForBook(string projectName, string name)
		{
			var path = GetBookFolder(projectName, name);
			if (!Directory.Exists(path))
				return 0;
			return Directory.GetDirectories(path).Sum(directory => GetSoundFilesInFolder(directory).Length);
		}

		public static bool DeleteLineRecording(string projectName, string bookName, int chapterNumber,
			int lineNumber)
		{
			// just being careful...
			if (GetHaveClip(projectName, bookName, chapterNumber, lineNumber))
			{
				var path = GetPathToLineRecording(projectName, bookName, chapterNumber, lineNumber);
				try
				{
					File.Delete(path);
					return true;
				}
				catch (IOException err)
				{
					ErrorReport.NotifyUserOfProblem(err,
						String.Format(LocalizationManager.GetString("ClipRepository.DeleteClipProblem",
							"HearThis was unable to delete this clip. File may be locked. Restarting HearThis might solve this problem. File: {0}"), path));
				}
			}
			return false;
		}

		#endregion

		#region AppData folder structure
		/// <summary>
		/// Get the folder %AppData%/SIL/HearThis where we store recordings and localization stuff.
		/// </summary>
		public static string ApplicationDataBaseFolder
		{
			get
			{
				if (_sHearThisFolder == null)
				{
					_sHearThisFolder = CreateDirectoryIfNeeded(
						Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
						Program.kCompany, Program.kProduct);
				}
				return _sHearThisFolder;
			}
		}

		/// <summary>
		/// Create (if necessary) and return the requested subfolder of the HearThis base AppData folder.
		/// </summary>
		/// <param name="projectName"></param>
		public static string GetApplicationDataFolder(string projectName)
		{
			return CreateDirectoryIfNeeded(ApplicationDataBaseFolder, projectName);
		}

		private static string CreateDirectoryIfNeeded(params string[] pathparts)
		{
			var path = Path.Combine(pathparts);
			Directory.CreateDirectory(path);
			return path;
		}

		#endregion

		#region Publishing methods

		public static void PublishAllBooks(PublishingModel publishingModel, string projectName,
			string publishRoot, IProgress progress)
		{
			if (!DirectoryUtilities.DeleteDirectoryRobust(publishRoot))
			{
				progress.WriteError(string.Format(LocalizationManager.GetString("ClipRepository.DeleteFolder",
					"Existing folder could not be deleted: {0}"), publishRoot));
				return;
			}

			foreach (string dir in Directory.GetDirectories(GetApplicationDataFolder(projectName)))
			{
				if (progress.CancelRequested)
					return;
				string bookName = Path.GetFileName(dir);
				//var filePath = Path.Combine(publishPath, bookName);
				PublishAllChapters(publishingModel, projectName, bookName, publishRoot, progress);
				if (progress.ErrorEncountered)
					return;
			}
		}

		public static void PublishAllChapters(PublishingModel publishingModel, string projectName,
			string bookName, string publishRoot, IProgress progress)
		{
			if (!publishingModel.IncludeBook(bookName)) // Maybe book has been deleted in Paratext.
				return;

			var bookFolder = GetBookFolder(projectName, bookName);
			foreach (var dirPath in Directory.GetDirectories(bookFolder))
			{
				if (progress.CancelRequested)
					return;
				var chapterNumber = int.Parse(Path.GetFileName(dirPath));
				PublishSingleChapter(publishingModel, projectName, bookName, chapterNumber, publishRoot, progress);
				if (progress.ErrorEncountered)
					return;
			}
		}

		private static string[] GetSoundFilesInFolder(string path)
		{
			return Directory.GetFiles(path, "*.wav");
		}

		public static bool GetDoAnyClipsExistForProject(string projectName)
		{
			return Directory.GetFiles(GetApplicationDataFolder(projectName), "*.wav", SearchOption.AllDirectories).Any();
		}

		private static void PublishSingleChapter(PublishingModel publishingModel, string projectName,
			string bookName, int chapterNumber, string rootPath, IProgress progress)
		{
			try
			{
				var verseFiles = GetSoundFilesInFolder(GetChapterFolder(projectName, bookName, chapterNumber));
				if (verseFiles.Length == 0)
					return;

				verseFiles = verseFiles.OrderBy(name =>
				{
					int result;
					if (Int32.TryParse(Path.GetFileNameWithoutExtension(name), out result))
						return result;
					throw new Exception(String.Format(LocalizationManager.GetString("ClipRepository.UnexpectedWavFile", "Unexpected WAV file: {0}"), name));
				}).ToArray();

				publishingModel.FilesInput += verseFiles.Length;
				publishingModel.FilesOutput++;

				progress.WriteMessage("{0} {1}", bookName, chapterNumber.ToString());

				string pathToJoinedWavFile = Path.GetTempPath().CombineForPath("joined.wav");
				using (TempFile.TrackExisting(pathToJoinedWavFile))
				{
					MergeAudioFiles(verseFiles, pathToJoinedWavFile, progress);

					PublishVerseIndexFiles(rootPath, bookName, chapterNumber, verseFiles, publishingModel, progress);

					publishingModel.PublishingMethod.PublishChapter(rootPath, bookName, chapterNumber, pathToJoinedWavFile,
						progress);
				}
			}
			catch (Exception error)
			{
				progress.WriteError(error.Message);
			}
		}

		internal static void MergeAudioFiles(IEnumerable<string> files, string pathToJoinedWavFile, IProgress progress)
		{
			if (files.Count() == 1)
			{
				File.Delete(pathToJoinedWavFile);
				File.Copy(files.First(), pathToJoinedWavFile);
			}
			else
			{
				var fileList = Path.GetTempFileName();
				File.WriteAllLines(fileList, files.ToArray());
				progress.WriteMessage(LocalizationManager.GetString("ClipRepository.MergeAudioProgress", "   Joining recorded clips",
					"Should have three leading spaces"));
				string arguments = string.Format("join -d \"{0}\" -F \"{1}\" -O always", Path.GetDirectoryName(pathToJoinedWavFile),
					fileList);
				RunCommandLine(progress, FileLocator.GetFileDistributedWithApplication(false, "shntool.exe"), arguments);
			}
			if (!File.Exists(pathToJoinedWavFile))
			{
				throw new ApplicationException(
					"Um... shntool.exe failed to produce the file of the joined clips. Reroute the power to the secondary transfer conduit.");
			}
		}

		public static void RunCommandLine(IProgress progress, string exePath, string arguments)
		{
			progress.WriteVerbose(exePath + " " + arguments);
			ExecutionResult result = CommandLineRunner.Run(exePath, arguments, null, 60, progress);
			result.RaiseExceptionIfFailed("");
		}

		/// <summary>
		/// Publish Audacity Label Files or cue sheet to text files
		/// </summary>
		public static void PublishVerseIndexFiles(string rootPath, string bookName, int chapterNumber, string[] verseFiles,
			PublishingModel publishingModel, IProgress progress)
		{
			// get the output path
			var outputPath = Path.ChangeExtension(
				publishingModel.PublishingMethod.GetFilePathWithoutExtension(rootPath, bookName, chapterNumber), "txt");

			try
			{
				// clear the text file if it already exists
				File.Delete(outputPath);
			}
			catch (Exception error)
			{
				progress.WriteError(error.Message);
			}

			if (publishingModel.VerseIndexFormat != PublishingModel.VerseIndexFormatType.None)
			{
				string contents = GetVerseIndexFileContents(bookName, chapterNumber, verseFiles,
					publishingModel.VerseIndexFormat, publishingModel.PublishingInfoProvider, outputPath);

				if (contents == null)
					return;

				try
				{
					using (StreamWriter writer = new StreamWriter(outputPath, false))
						writer.Write(contents);
				}
				catch (Exception error)
				{
					progress.WriteError(error.Message);
				}
			}
		}

		internal static string GetVerseIndexFileContents(string bookName, int chapterNumber, string[] verseFiles,
			PublishingModel.VerseIndexFormatType verseIndexFormat, IPublishingInfoProvider publishingInfoProvider,
			string outputPath)
		{
			switch (verseIndexFormat)
			{
				case PublishingModel.VerseIndexFormatType.AudacityLabelFileVerseLevel:
					return chapterNumber == 0 ? null :
						GetAudacityLabelFileContents(verseFiles, publishingInfoProvider, bookName, chapterNumber, false);
				case PublishingModel.VerseIndexFormatType.AudacityLabelFilePhraseLevel:
					return GetAudacityLabelFileContents(verseFiles, publishingInfoProvider, bookName, chapterNumber, true);
				case PublishingModel.VerseIndexFormatType.CueSheet:
					return GetCueSheetContents(verseFiles, publishingInfoProvider, bookName, chapterNumber, outputPath);
				default:
					throw new InvalidEnumArgumentException("verseIndexFormat", (int)verseIndexFormat, typeof(PublishingModel.VerseIndexFormatType));
			}
		}

		internal static string GetCueSheetContents(string[] verseFiles, IPublishingInfoProvider infoProvider, string bookName,
			int chapterNumber, string outputPath)
		{
			var bldr = new StringBuilder();
			bldr.AppendFormat("FILE \"{0}\"", outputPath);
			bldr.AppendLine();

			TimeSpan indextime = new TimeSpan(0, 0, 0, 0);

			for (int i = 0; i < verseFiles.Length; i++)
			{
				bldr.AppendLine(String.Format("  TRACK {0:000} AUDIO", (i + 1)));
				//    "  TRACK 0" + (i + 1) + " AUDIO");
				//else
				//    "  TRACK " + (i + 1) + " AUDIO";
				bldr.AppendLine("	TITLE 00000-" + bookName + chapterNumber + "-tnnC001 ");
				bldr.AppendLine("	INDEX 01 " + indextime);

				// get the length of the block
				using (var b = new NAudio.Wave.WaveFileReader(verseFiles[i]))
				{
					TimeSpan wavlength = b.TotalTime;

					//update the indextime for the verse
					indextime = indextime.Add(wavlength);
				}
			}
			return bldr.ToString();
		}

		internal static string GetAudacityLabelFileContents(string[] verseFiles, IPublishingInfoProvider infoProvider,
			string bookName, int chapterNumber, bool phraseLevel)
		{
			var bldr = new StringBuilder();
			var headingCounters = new Dictionary<string, int>();
			TimeSpan startTime = new TimeSpan(0, 0, 0, 0);
			TimeSpan endTime = new TimeSpan(0, 0, 0, 0);
			string prevVerse = null;
			string prevVerseEnd = null;
			string currentVerse = null;
			int subPhrase = -1;

			for (int i = 0; i < verseFiles.Length; i++)
			{
				// get the length of the block
				using (var b = new NAudio.Wave.WaveFileReader(verseFiles[i]))
				{
					TimeSpan wavlength = b.TotalTime;

					//update the endTime for the verse
					endTime = endTime.Add(wavlength);
				}

				string timeRange = String.Format("{0:0.######}\t{1:0.######}\t", startTime.TotalSeconds, endTime.TotalSeconds);

				// REVIEW: Use TryParse to avoid failure for extraneous filename?
				int lineNumber = Int32.Parse(Path.GetFileNameWithoutExtension(verseFiles[i]));
				ScriptLine block = infoProvider.GetBlock(bookName, chapterNumber, lineNumber);

				string label;
				if (block.Heading)
				{
					subPhrase = -1;

					var headingType = block.HeadingType.TrimEnd('1', '2', '3', '4');

					switch (headingType)
					{
						case "c":
						case "mt":
							label = headingType; break;
						default:
							int headingCounter;
							if (!headingCounters.TryGetValue(headingType, out headingCounter))
								headingCounter = 1;
							else
								headingCounter++;

							label = headingType + headingCounter;
							headingCounters[headingType] = headingCounter;
							break;
					}
				}
				else if (chapterNumber == 0)
				{
					// Intro material
					subPhrase++;
					label = string.Empty;
				}
				else
				{
					currentVerse = block.Verse;

					// Current block is a normal verse
					if (i < verseFiles.Length - 1)
					{
						// Check next block
						int nextLineNumber = Int32.Parse(Path.GetFileNameWithoutExtension(verseFiles[i + 1]));
						ScriptLine nextBlock = infoProvider.GetBlock(bookName, chapterNumber, nextLineNumber);
						if (phraseLevel)
						{
							Debug.Assert(currentVerse != null);
							// If this is the same as the next verse but different from the previous one, start
							// a new sub-verse sequence.
							if (!nextBlock.Heading && currentVerse == nextBlock.Verse && prevVerse != currentVerse)
								subPhrase = 0;
							else
							{
								if (block.CrossesVerseBreak)
								{
									// Unless/until SAB can handle implicit verse bridges, all we care about is the first "verse"
									// (which could be an explicit verse bridge)
									currentVerse = currentVerse.Substring(0, currentVerse.IndexOf('~'));
								}
								if (nextBlock.CrossesVerseBreak &&
									currentVerse == nextBlock.Verse.Substring(0, nextBlock.Verse.IndexOf('~')) &&
									prevVerse != currentVerse)
								{
									subPhrase = 0;
								}
							}
						}
						else if (!nextBlock.Heading && currentVerse == nextBlock.Verse)
						{
							// Same verse number
							// for verse-level highlighting, postpone appending until we have the whole verse.
							continue;
						}
					}
					else if (block.CrossesVerseBreak)
					{
						// Unless/until SAB can handle implicit verse bridges, all we care about is the first "verse"
						// (which could be an explicit verse bridge)
						currentVerse = currentVerse.Substring(0, currentVerse.IndexOf('~'));
					}

					label = currentVerse;
				}

				if (chapterNumber > 0)
				{
					if (subPhrase >= 0 && prevVerse == currentVerse)
						subPhrase++;
					else if (!block.Heading && currentVerse == prevVerseEnd)
						subPhrase = 1;
					else if (subPhrase > 0 && prevVerse != currentVerse)
						subPhrase = -1;
				}
				bldr.AppendLine(timeRange + label + (subPhrase >= 0 ? ((char)('a' + subPhrase)).ToString() : string.Empty));
				// update start time for the next verse
				startTime = endTime;
				prevVerse = currentVerse;
				prevVerseEnd = null;
				if (block.CrossesVerseBreak && phraseLevel)
					prevVerseEnd = block.Verse.Substring(block.Verse.IndexOf('~') + 1);
			}

			return bldr.ToString();
		}

		#endregion
	}
}
