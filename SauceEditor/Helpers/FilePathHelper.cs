using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceEditor.Helpers
{
    public static class FilePathHelper
    {
        private static string SOLUTION_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..";//@"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\TakoEngine";

        public static string SETTINGS_PATH = SOLUTION_DIRECTORY + @"\SauceEditor\EditorSettings" + SauceEditor.Structure.EditorSettings.FILE_EXTENSION;
    }
}
