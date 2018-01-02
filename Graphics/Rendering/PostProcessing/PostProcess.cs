using Graphics.Outputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.PostProcessing
{
    public abstract class PostProcess
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public Resolution Resolution { get; set; }
        
        public PostProcess(string name)
        {
            Name = name;
        }

        public abstract void Load();
        public abstract void Unload();
        public abstract void Reload();
    }
}
