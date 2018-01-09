using Graphics.GameObjects;
using Graphics.Meshes;
using Graphics.Physics.Collision;
using Graphics.Rendering.Shaders;
using Graphics.Rendering.Textures;
using Graphics.Scripting.BehaviorTrees;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Graphics.Maps
{
    public class MapGameObject
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public string MeshFilePath { get; set; }
        public string TextureFilePath { get; set; }
        public string NormalMapFilePath { get; set; }
        public string BehaviorFilePath { get; set; }
        public List<GameProperty> Properties { get; set; }
        //public ICollider Collider { get; set; }

        public GameObject ToGameObject(ShaderProgram program)
        {
            var gameObject = new GameObject(Name)
            {
                Mesh = Mesh.LoadFromFile(MeshFilePath, program)
            };

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
                gameObject.Position = Position;
            }

            if (Rotation != null)
            {
                gameObject.Rotation = Rotation;
            }

            if (Scale != null)
            {
                gameObject.Scale = Scale;
            }

            gameObject.Mesh.AddTestColors();
            gameObject.Bounds = gameObject.Name == "Player"
                ? (Bounds)new BoundingCircle(gameObject)
                : new BoundingBox(gameObject);

            return gameObject;
        }
    }
}
