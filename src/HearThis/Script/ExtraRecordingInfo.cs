﻿// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2022, SIL International. All Rights Reserved.
// <copyright from='2021' to='2022' company='SIL International'>
//		Copyright (c) 2022, SIL International. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (https://sil.mit-license.org/)
// </copyright>
#endregion
// --------------------------------------------------------------------------------------------

using System;

namespace HearThis.Script
{
	/// <summary>
	/// Little class to hold the information about a clip that is for a position that is beyond the
	/// last block in the current script. Extra recordings can result from changes to settings, but
	/// usually they are the result of a change to the script (often earlier in the chapter).
	/// </summary>
	public class ExtraRecordingInfo
	{
		public ScriptLine RecordingInfo { get; }
		public string ClipFile { get; }

		public ExtraRecordingInfo(string file, ScriptLine recordingInfo)
		{
			ClipFile = file ?? throw new ArgumentNullException(nameof(file));
			RecordingInfo = recordingInfo;
		}
	}
}
