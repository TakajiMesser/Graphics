using TakoEngine.Maps;
using TakoEngine.Rendering.Matrices;
using TakoEngine.Utilities;
using Jidai.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jidai.GameObjects
{
    public class Camera : MapCamera
    {
        public const string NAME = "MainCamera";

        public Camera()
        {
            Name = NAME;
            AttachedActorName = "Player";
            Position = new Vector3(0.0f, 0.0f, 20.0f);
            Type = ProjectionTypes.Perspective;
            ZNear = 0.1f;
            ZFar = 1000.0f;
            FieldOfViewY = UnitConversions.ToRadians(45.0f);
        }
    }
}
