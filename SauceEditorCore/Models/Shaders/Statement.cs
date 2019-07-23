using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Shaders
{
    public class Statement
    {
        private string _contents;

        public Statement(string contents) => _contents = contents;

        public string GetText() => _contents;
    }
}
