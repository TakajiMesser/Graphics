using TakoEngine.GameObjects;
using TakoEngine.Meshes;
using TakoEngine.Physics.Collision;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;
using TakoEngine.Rendering.Vertices;
using TakoEngine.Scripting.BehaviorTrees;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TakoEngine.Maps
{
    public class MapGameObject
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public string ModelFilePath { get; set; }
        public List<TexturePaths> TexturesPaths { get; set; } = new List<TexturePaths>();
        public string BehaviorFilePath { get; set; }
        public List<GameProperty> Properties { get; set; }
        public bool HasCollision { get; set; }
        //public ICollider Collider { get; set; }

        public GameObject ToGameObject(TextureManager textureManager)
        {
            var gameObject = new GameObject(Name);

            if (!string.IsNullOrEmpty(ModelFilePath))
            {
                gameObject.Model = Model.LoadFromFile(ModelFilePath, textureManager);
            }

            if (!string.IsNullOrEmpty(BehaviorFilePath))
            {
                gameObject.Behaviors = new BehaviorTree()
                {
                    RootNode = Node.Load(BehaviorFilePath)
                };
            }

            if (Properties != null)
            {
                foreach (var property in Properties)
                {
                    gameObject.Properties.Add(property.Name, property);
                }
            }

            if (Position != null)
            {
                gameObject.Model.Position = Position;
            }

            if (Rotation != null)
            {
                gameObject.Model.OriginalRotation = Rotation;
            }

            if (Scale != null)
            {
                gameObject.Model.Scale = Scale;
            }

            gameObject.Model.AddTestColors();

            gameObject.HasCollision = HasCollision;
            gameObject.Bounds = gameObject.Name == "Player"
                ? (Bounds)new BoundingCircle(gameObject)
                : new BoundingBox(gameObject);

            return gameObject;
        }
    }
}
