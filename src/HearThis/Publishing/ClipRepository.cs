// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2015, SIL International. All Rights Reserved.
// <copyright from='2011' to='2015' company='SIL International'>
//		Copyright (c) 2015, SIL International. All Rights Reserved.
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
using L10NSharp;
using SIL.CommandLineProcessing;
using SIL.IO;
using SIL.Extensions;
using SIL.Progress;
using SIL.Reporting;
using HearThis.Script;

namespace HearThis.Publishing
{
	/// <summary>
	/// Each script block is recorded and each clip stored as its own file.  This class manages that collection of files.
	/// </summary>
	public static class ClipRepository
	{
		private const string kSkipFileExtension = "skip";

		#region Retrieval and Deletion methods

		/// <summary>
		/// Gets the path to the indicated line. If a script provider is provided that implements IActorCharacterProvider
		/// and there is a current character, lineNumber is relative to the lines for that character.
		/// </summary>
		/// <param name="projectName"></param>
		/// <param name="bookName"></param>
		/// <param name="chapterNumber"></param>
		/// <param name="lineNumber"></param>
		/// <param name="scriptProvider"></param>
		/// <returns></returns>
		public static string GetPathToLineRecording(string projectName, string bookName, int chapterNumber, int lineNumber, IScriptProvider scriptProvider = null)
		{
			var chapter = GetChapterFolder(projectName, bookName, chapterNumber);
			var adjustedLineNumber = GetRealLineNumber(bookName, chapterNumber, lineNumber, scriptProvider);
			return Path.Combine(chapter, adjustedLineNumber + ".wav");
		}

		public static string GetPathToLineRecordingUnfiltered(string projectName, string bookName, int chapterNumber, int lineNumber)
		{
			// Not passing a script provider means that line number won't get adjusted.
			return GetPathToLineRecording(projectName, bookName, chapterNumber, lineNumber);
		}

		// When HearThis is filtering by character, generally it pretends the only blocks a chapter has are the ones that
		// character is supposed to record. However, the recording file has to use the real block number (actually one less
		// than the number recorded in the block) so that recordings from different files don't overwrite each other.
		// This routine converts from a possibly-filtered block number address to a real one.
		private static int GetRealLineNumber(string bookName, int chapterNumber, int lineNumber, IScriptProvider scriptProvider)
		{
			var adjustedLineNumber = lineNumber;
			if (scriptProvider != null)
			{
				var bookNumber = scriptProvider.VersificationInfo.GetBookNumber(bookName);
				// We do sometimes find ourselves in an unrecorded chapter asking for the path to block 0.
				// That will crash GetBlock, so check.
				if (scriptProvider.GetScriptBlockCount(bookNumber, chapterNumber) > lineNumber)
				{
					var block = scriptProvider.GetBlock(bookNumber, chapterNumber, lineNumber);
					adjustedLineNumber = block.Number - 1;
				}
			}
			return adjustedLineNumber;
		}

		/// <summary>
		/// See whether we have the specified clip. If a scriptProvider is passed which implements IActorCharacterProvider
		/// and it has a current character, lineNumber is relative to the lines for that character.
		/// </summary>
		public static bool GetHaveClip(string projectName, string bookName, int chapterNumber, int lineNumber, IScriptProvider scriptProvider = null)
		{
			var path = GetPathToLineRecording(projectName, bookName, chapterNumber, lineNumber, scriptProvider);
			return File.Exists(path);
		}

		public static bool GetHaveClipUnfiltered(string projectName, string bookName, int chapterNumber, int lineNumber)
		{
			// Not passing a script provider ensures that the line number won't get adjusted.
			return GetHaveClip(projectName, bookName, chapterNumber, lineNumber);
		}

		public static string GetChapterFolder(string projectName, string bookName, int chapterNumber)
		{
			var book = GetBookFolder(projectName, bookName);
			var chapter = Utils.CreateDirectory(book, chapterNumber.ToString());
			return chapter;
		}

		private static string GetBookFolder(string projectName, string bookName)
		{
			var project = GetProjectFolder(projectName);
			var book = Utils.CreateDirectory(project, bookName.Trim());
			return book;
		}

		public static string GetProjectFolder(string projectName)
		{
			return Program.GetApplicationDataFolder(projectName);
		}

		public static int GetCountOfRecordingsInFolder(string path, IScriptProvider scriptProvider)
		{
			if (!Directory.Exists(path))
				return 0;
			var provider = scriptProvider as IActorCharacterProvider;
			var soundFilesInFolder = GetSoundFilesInFolder(path);
			if (provider == null)
				return soundFilesInFolder.Length;
			int chapter = int.Parse(Path.GetFileName(path));
			var bookName = Path.GetFileName(Path.GetDirectoryName(path));
			int book = scriptProvider.VersificationInfo.GetBookNumber(bookName);
			return soundFilesInFolder.Count(f =>
			{
				int lineNo0Based;
				if (!int.TryParse(Path.GetFileNameWithoutExtension(f), out lineNo0Based))
					return false; // don't count files whose names don't parse as numbers
				return provider.IsBlockInCharacter(book, chapter, lineNo0Based);
			});
		}

		public static int GetCountOfRecordingsForBook(string projectName, string name, IScriptProvider scriptProvider)
		{
			var path = GetBookFolder(projectName, name);
			if (!Directory.Exists(path))
				return 0;
			return Directory.GetDirectories(path).Sum(directory => GetCountOfRecordingsInFolder(directory, scriptProvider));
		}

		public static bool HasRecordingsForProject(string projectName)
		{
			return Directory.GetDirectories(Program.GetApplicationDataFolder(projectName))
				.Any(bookDirectory => Directory.GetDirectories(bookDirectory).Any(chDirectory => GetSoundFilesInFolder(chDirectory).Length > 0));
		}

		// line number is not character-filtered.
		public static bool DeleteLineRecording(string projectName, string bookName, int chapterNumber, int lineNumber, IScriptProvider scriptProvider = null)
		{
			// just being careful...
			if (GetHaveClipUnfiltered(projectName, bookName, chapterNumber, lineNumber))
			{
				var path = GetPathToLineRecordingUnfiltered(projectName, bookName, chapterNumber, lineNumber);
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

		/// <summary>
		/// lineNumber is unfiltered
		/// </summary>
		public static void DeleteAllClipsAfterLine(string projectName, string bookName, int chapterNumber, int lineNumber)
		{
			var chapterFolder = GetChapterFolder(projectName, bookName, chapterNumber);
			var allFiles = Directory.GetFiles(chapterFolder);
			foreach (var file in allFiles)
			{
				var extension = Path.GetExtension(file);
				var lineNumberForFileStr = Path.GetFileNameWithoutExtension(file);
				int lineNumberForFile;
				if (new HashSet<string> {".wav", ".skip"}.Contains(extension) && int.TryParse(lineNumberForFileStr, out lineNumberForFile))
				{
					if (lineNumberForFile > lineNumber)
						File.Delete(file);
				}
			}
		}

		public static void BackUpRecordingForSkippedLine(string projectName, string bookName, int chapterNumber1Based, int block, IScriptProvider scriptProvider = null)
		{
			var recordingPath = GetPathToLineRecording(projectName, bookName, chapterNumber1Based, block, scriptProvider);
			if (File.Exists(recordingPath))
				File.Move(recordingPath, Path.ChangeExtension(recordingPath, kSkipFileExtension));
		}

		public static bool RestoreBackedUpClip(string projectName, string bookName, int chapterNumber1Based, int block, IScriptProvider scriptProvider = null)
		{
			var recordingPath = GetPathToLineRecording(projectName, bookName, chapterNumber1Based, block, scriptProvider);
			var skipPath = Path.ChangeExtension(recordingPath, kSkipFileExtension);
			if (File.Exists(skipPath))
			{
				File.Move(skipPath, recordingPath);
				return true;
			}
			return false;
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

			var bookNames = new List<string>(Directory.GetDirectories(Program.GetApplicationDataFolder(projectName)).Select(dir => Path.GetFileName(dir)));
			bookNames.Sort(publishingModel.PublishingInfoProvider.BookNameComparer);

			foreach (string bookName in bookNames)
			{
				if (progress.CancelRequested)
					return;
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
			var chapters = new List<int>(Directory.GetDirectories(bookFolder).Select(dir => int.Parse(Path.GetFileName(dir))));
			chapters.Sort();
			foreach (var chapterNumber in chapters)
			{
				if (progress.CancelRequested)
					return;
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
			return Directory.GetFiles(Program.GetApplicationDataFolder(projectName), "*.wav", SearchOption.AllDirectories).Any();
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

					var lastClipFile = verseFiles.LastOrDefault();
					if (lastClipFile != null)
					{
						int lineNumber = Int32.Parse(Path.GetFileNameWithoutExtension(lastClipFile));
						try
						{
							publishingModel.PublishingInfoProvider.GetUnfilteredBlock(bookName, chapterNumber, lineNumber);
						}
						catch (ArgumentOutOfRangeException)
						{
							progress.WriteWarning(string.Format(LocalizationManager.GetString("ClipRepository.ExtraneousClips",
								"Unexpected recordings (i.e., clips) were encountered in the folder for {0} {1}."), bookName, chapterNumber));
						}
					}
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
			var outputDirectoryName = Path.GetDirectoryName(pathToJoinedWavFile);
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
				string arguments = string.Format("join -d \"{0}\" -F \"{1}\" -O always -r none", outputDirectoryName,
					fileList);
				RunCommandLine(progress, FileLocator.GetFileDistributedWithApplication(false, "shntool.exe"), arguments);

				// Passing just the directory name for output file means the output file is ALWAYS joined.wav.
				// It's possible to pass more of a file name, but that just makes things more complex, because
				// shntool will always prepend 'joined' to the name we really want.
				// Some callers actually want the name to be 'joined.wav'. If not, we just rename it afterwards.
				var outputFilePath = pathToJoinedWavFile;
				if (Path.GetFileName(pathToJoinedWavFile) != "joined.wav")
				{
					outputFilePath = Path.Combine(outputDirectoryName, "joined.wav");
				}
				if (!File.Exists(outputFilePath))
				{
					throw new ApplicationException(
						"Um... shntool.exe failed to produce the file of the joined clips. Reroute the power to the secondary transfer conduit.");
				}
				if (Path.GetFileName(pathToJoinedWavFile) != "joined.wav")
				{
					File.Delete(pathToJoinedWavFile);
					File.Move(outputFilePath, pathToJoinedWavFile);
				}
			}
		}

		public static void RunCommandLine(IProgress progress, string exePath, string arguments)
		{
			progress.WriteVerbose(exePath + " " + arguments);
			ExecutionResult result = CommandLineRunner.Run(exePath, arguments, null, 60*10, progress);
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
			var audacityLabelFileBuilder = new AudacityLabelFileBuilder(verseFiles, infoProvider, bookName, chapterNumber,
				phraseLevel);
			return audacityLabelFileBuilder.ToString();
		}

		#region AudacityLabelFileBuilder class
		private class AudacityLabelFileBuilder
		{
			private readonly string[] verseFiles;
			private readonly IPublishingInfoProvider infoProvider;
			private readonly string bookName;
			private readonly int chapterNumber;
			private readonly bool phraseLevel;
			private readonly StringBuilder bldr = new StringBuilder();
			private readonly Dictionary<string, int> headingCounters = new Dictionary<string, int>();

			private ScriptLine block;
			private double startTime, endTime;
			private string prevVerse = null;
			private double accumClipTimeFromPrevBlocks = 0.0;
			private string currentVerse = null;
			private string nextVerse;
			private int subPhrase = -1;

			public AudacityLabelFileBuilder(string[] verseFiles, IPublishingInfoProvider infoProvider,
				string bookName, int chapterNumber, bool phraseLevel)
			{
				this.verseFiles = verseFiles;
				this.infoProvider = infoProvider;
				this.bookName = bookName;
				this.chapterNumber = chapterNumber;
				this.phraseLevel = phraseLevel;
			}

			public override string ToString()
			{
				for (int i = 0; i < verseFiles.Length; i++)
				{
					// get the length of the block
					double clipLength;
					using (var b = new NAudio.Wave.WaveFileReader(verseFiles[i]))
					{
						clipLength = b.TotalTime.TotalSeconds;
						//update the endTime for the verse
						endTime = endTime + clipLength;
					}

					// REVIEW: Use TryParse to avoid failure for extraneous filename?
					int lineNumber = Int32.Parse(Path.GetFileNameWithoutExtension(verseFiles[i]));
					block = GetUnfilteredBlock(lineNumber);
					if (block == null)
						break;

					nextVerse = null;

					string label;
					if (block.Heading)
					{
						subPhrase = -1;
						label = GetHeadingBlockLabel();
					}
					else
					{
						if (chapterNumber == 0)
						{
							// Intro material
							subPhrase++;
							label = string.Empty;
						}
						else
						{
							ScriptLine nextBlock = null;
							if (i < verseFiles.Length - 1)
							{
								// Check next block
								int nextLineNumber = Int32.Parse(Path.GetFileNameWithoutExtension(verseFiles[i + 1]));
								nextBlock = GetUnfilteredBlock(nextLineNumber);
								if (nextBlock != null)
								{
									nextVerse = nextBlock.CrossesVerseBreak
										? nextBlock.Verse.Substring(0, nextBlock.Verse.IndexOf('~'))
										: nextBlock.Verse;
								}
							}

							if (block.CrossesVerseBreak)
							{
								MakeLabelsForApproximateVerseLocationsInBlock(clipLength);
								continue;
							}

							// Current block is a normal verse or explicit verse bridge
							currentVerse = block.Verse;

							if (nextBlock != null)
							{
								Debug.Assert(currentVerse != null);

								if (phraseLevel)
								{
									// If this is the same as the next verse but different from the previous one, start
									// a new sub-verse sequence.
									if (!nextBlock.Heading && prevVerse != currentVerse &&
										(currentVerse == nextBlock.Verse ||
										(nextBlock.CrossesVerseBreak &&
										currentVerse == nextBlock.Verse.Substring(0, nextBlock.Verse.IndexOf('~')))))
									{
										subPhrase = 0;
									}
								}
								else if (!nextBlock.Heading && currentVerse == nextVerse)
								{
									// Same verse number.
									// For verse-level highlighting, postpone appending until we have the whole verse.
									prevVerse = currentVerse;
									accumClipTimeFromPrevBlocks += endTime - startTime;
									continue;
								}
							}

							label = currentVerse;
							UpdateSubPhrase();
						}
					}

					AppendLabel(startTime, endTime, label);

					// update start time for the next verse
					startTime = endTime;
					prevVerse = currentVerse;
				}

				return bldr.ToString();
			}

			private ScriptLine GetUnfilteredBlock(int lineNumber)
			{
				try
				{
					return infoProvider.GetUnfilteredBlock(bookName, chapterNumber, lineNumber);
				}
				catch (Exception)
				{
					return null;
				}
			}

			private void MakeLabelsForApproximateVerseLocationsInBlock(double clipLength)
			{
// Unless/until SAB can handle implicit verse bridges, we want to create a label
				// at approximately the right place (based on verse number offsets in text) for
				// each verse in the block.
				int ichVerse = 0;
				var verseOffsets = block.VerseOffsets.ToList();
				var textLen = block.Text.Length;
				verseOffsets.Add(textLen);
				int prevOffset = 0;
				double start = 0.0;
				foreach (var verseOffset in verseOffsets)
				{
					int ichVerseLim = block.Verse.IndexOf('~', ichVerse);
					if (ichVerseLim == -1)
					{
						currentVerse = block.Verse.Substring(ichVerse);
					}
					else
					{
						Debug.Assert(ichVerseLim <= block.Verse.Length - 2);
						currentVerse = block.Verse.Substring(ichVerse, ichVerseLim - ichVerse);
						ichVerse = ichVerseLim + 1;
					}
					double end = FindEndOfVerse(clipLength, start, prevOffset, verseOffset, block.Text);
					if (phraseLevel || currentVerse != prevVerse || currentVerse != nextVerse)
					{
						if (!phraseLevel && currentVerse == nextVerse)
						{
							accumClipTimeFromPrevBlocks += end - start;
							prevVerse = currentVerse;
							continue;
						}
						UpdateSubPhrase();
						end += accumClipTimeFromPrevBlocks;
						accumClipTimeFromPrevBlocks = 0.0;
						AppendLabel(startTime + start, startTime + end, currentVerse);
					}
					prevVerse = currentVerse;
					start = end;
					prevOffset = verseOffset;
				}
				startTime = endTime - accumClipTimeFromPrevBlocks;
			}

			private string GetHeadingBlockLabel()
			{
				var headingType = block.HeadingType.TrimEnd('1', '2', '3', '4');

				if (headingType == "c" || headingType == "mt")
					return headingType;

				int headingCounter;
				if (!headingCounters.TryGetValue(headingType, out headingCounter))
					headingCounter = 1;
				else
					headingCounter++;

				headingCounters[headingType] = headingCounter;
				return headingType + headingCounter;
			}

			private double FindEndOfVerse(double clipLength, double start, int prevOffset, int verseOffset, string text)
			{
				double percentage = (verseOffset - prevOffset) / (double) text.Length;
				return start + clipLength * percentage;
			}

			private void UpdateSubPhrase()
			{
				if (subPhrase >= 0 && prevVerse == currentVerse)
					subPhrase++;
				// if (!block.Heading && currentVerse == prevVerseEnd)
				//    return 1;
				else if (subPhrase > 0 && prevVerse != currentVerse)
					subPhrase = -1;
				if (subPhrase == -1 && currentVerse == nextVerse)
					subPhrase = 0;
			}

			private void AppendLabel(double start, double end, string label)
			{
				string timeRange = String.Format("{0:0.######}\t{1:0.######}\t", start, end);
				bldr.AppendLine(timeRange + label + (subPhrase >= 0 ? ((char)('a' + subPhrase)).ToString() : string.Empty));
			}
		}
		#endregion //AudacityLabelFileBuilder class

		#endregion
	}
}
