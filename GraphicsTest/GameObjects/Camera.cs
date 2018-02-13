using Graphics.Maps;
using Graphics.Rendering.Matrices;
using Graphics.Utilities;
using GraphicsTest.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest.GameObjects
{
    public class Camera : MapCamera
    {
        public const string NAME = "MainCamera";

        public Camera()
        {
            Name = NAME;
            AttachedGameObjectName = "Player";
            Position = new Vector3(0.0f, 0.0f, 20.0f);
            Type = ProjectionTypes.Perspective;
            FieldOfViewY = UnitConversions.ToRadians(45.0f);
        }
    }
}
