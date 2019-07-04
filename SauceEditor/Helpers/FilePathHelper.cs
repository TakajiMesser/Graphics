﻿using SauceEditor.Models;
using System;

namespace SauceEditor.Helpers
{
    public static class FilePathHelper
    {
        internal static readonly string SOLUTION_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..";

        public static string SETTINGS_PATH = SOLUTION_DIRECTORY + @"\SauceEditor\EditorSettings" + EditorSettings.FILE_EXTENSION;
        public static string INITIAL_FILE_DIRECTORY = SOLUTION_DIRECTORY + @"\SampleGameProject\Maps";
    }
}
