using TakoEngine.Inputs;
using TakoEngine.Meshes;
using TakoEngine.Physics.Collision;
using TakoEngine.Rendering.Matrices;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Scripting.BehaviorTrees;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TakoEngine.Lighting;
using TakoEngine.Rendering.Textures;
using OpenTK.Graphics.OpenGL;
using TakoEngine.Rendering.Vertices;

namespace TakoEngine.GameObjects
{
    public abstract class GameEntity
    {
        public int ID { get; internal set; }
    }
}
