using SauceEditor.Models;
using System;

namespace SauceEditor.Helpers
{
    public static class FilePathHelper
    {
        private static readonly string SOLUTION_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..";

        public static string SETTINGS_PATH = SOLUTION_DIRECTORY + @"\SauceEditor\EditorSettings" + EditorSettings.FILE_EXTENSION;
    }
}
