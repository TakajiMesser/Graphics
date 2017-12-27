using Graphics.Inputs;
using Graphics.Matrices;
using Graphics.Meshes;
using Graphics.Physics.Collision;
using Graphics.Rendering.Shaders;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.GameObjects
{
    public class GameProperty
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }

        public GameProperty(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}
