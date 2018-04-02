using OpenTK;
using System.Collections.Generic;
using TakoEngine.Entities;
using TakoEngine.Entities.Models;
using TakoEngine.Game;
using TakoEngine.Physics.Collision;
using TakoEngine.Rendering.Textures;
using TakoEngine.Scripting.Behaviors;

namespace TakoEngine.Maps
{
    public class MapActor
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Orientation { get; set; }
        public string ModelFilePath { get; set; }
        public List<TexturePaths> TexturesPaths { get; set; } = new List<TexturePaths>();
        public string BehaviorFilePath { get; set; }
        public List<GameProperty> Properties { get; set; }
        public bool HasCollision { get; set; }
        //public ICollider Collider { get; set; }

        public Actor ToActor(TextureManager textureManager)
        {
            var actor = new Actor(Name);

            if (!string.IsNullOrEmpty(ModelFilePath))
            {
                actor.Model = Model.LoadFromFile(ModelFilePath, textureManager);
            }

            if (!string.IsNullOrEmpty(BehaviorFilePath))
            {
                actor.Behaviors = Behavior.Load(BehaviorFilePath);
            }

            if (Properties != null)
            {
                foreach (var property in Properties)
                {
                    actor.Properties.Add(property.Name, property);
                }
            }

            if (Position != null)
            {
                actor.Model.Position = Position;
            }

            if (Rotation != null)
            {
                actor.OriginalRotation = Rotation;
            }

            if (Scale != null)
            {
                actor.Model.Scale = Scale;
            }

            if (Orientation != null)
            {
                actor.Model.Orientation = Quaternion.FromEulerAngles(Orientation);
            }

            actor.Model.AddTestColors();

            actor.HasCollision = HasCollision;
            actor.Bounds = actor.Name == "Player"
                ? (Bounds)new BoundingCircle(actor)
                : new BoundingBox(actor);

            return actor;
        }
    }
}
