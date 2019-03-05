using SauceEditor.Models;
using System;

namespace SauceEditor.Helpers
{
    public static class FilePathHelper
    {
        private static string SOLUTION_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..";//@"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\SpiceEngine";

        public static string SETTINGS_PATH = SOLUTION_DIRECTORY + @"\SauceEditor\EditorSettings" + EditorSettings.FILE_EXTENSION;
    }
}
