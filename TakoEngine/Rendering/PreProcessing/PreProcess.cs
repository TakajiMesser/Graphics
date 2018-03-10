using TakoEngine.Outputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Rendering.PreProcessing
{
    public abstract class PreProcess
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public Resolution Resolution { get; set; }

        public PreProcess(string name, Resolution resolution)
        {
            Name = name;
            Resolution = resolution;
        }

        public void Load()
        {
            LoadPrograms();
            LoadBuffers();
        }

        protected abstract void LoadPrograms();
        protected abstract void LoadBuffers();
        //public abstract void Render();
    }
}
