﻿using SpiceEngine.Maps;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Maps;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Vertices;
using System.Linq;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace TowerWarfare.Entities.Actors.Troops
{
    public abstract class Enemy : MapActor
    {
        public Enemy(Vector3 position)
        {
            Position = position;
            Offset = new Vector3();
            Scale = 0.04f * Vector3.One;
            Rotation = Vector3.Zero;
            Orientation = new Vector3(0.0f, 0.0f, 90.0f);
            ModelFilePath = Helpers.FilePathHelper.BOB_LAMP_MESH_PATH;

            IsPhysical = true;
            
            Color = GetColor();

            Behavior = MapBehavior.Load(Helpers.FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);
            //BuildShape();
        }

        protected abstract ModelMesh GetShape();// => ModelMesh.Box(2.0f, 2.0f, 2.0f);
        protected abstract Color4 GetColor();

        private void BuildShape()
        {
            var modelMesh = GetShape();
            if (modelMesh != null)
            {
                var modelBuilder = new ModelBuilder(modelMesh);

                Vertices.AddRange(modelBuilder.GetVertices().Select(v => new Vertex3D(v.Position, v.Normal, v.Tangent, v.UV, Color)));
                TriangleIndices.AddRange(modelBuilder.TriangleIndices);
                Material = SpiceEngineCore.Rendering.Materials.Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2;
            }
        }
    }
}
 