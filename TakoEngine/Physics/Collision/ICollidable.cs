using OpenTK;
using System;
using TakoEngine.Entities;
using TakoEngine.Entities.Lights;

namespace TakoEngine.Physics.Collision
{
    public interface ICollidable
    {
        Bounds Bounds { get; set; }
        bool HasCollision { get; set; }
    }
}
