using OpenTK;
using OpenTK.Graphics;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Maps
{
    public class MapLight : MapEntity3D<ILight>
    {
        public enum LightTypes
        {
            Point,
            Spot,
            Directional
        }

        public LightTypes LightType { get; set; }

        public Color4 Color { get; set; }

        public float Intensity { get; set; }
        public float Radius { get; set; }
        public float Height { get; set; }

        /*public Mesh3D<Vertex3D> ToMesh()
        {
            var vertices = Vertices.Select(v => new Vertex3D(v, v, v, Vector2.Zero, Color4.Blue)).ToList();
            return new Mesh3D<Vertex3D>(vertices, TriangleIndices);
        }*/

        public override ILight ToEntity()
        {
            switch (LightType)
            {
                case LightTypes.Point:
                    return new PointLight()
                    {
                        Position = Position,
                        Color = Color.ToVector4(),
                        Intensity = Intensity,
                        Radius = Radius
                    };
                case LightTypes.Spot:
                    return new SpotLight()//Vertices, TriangleIndices, Color)
                    {
                        Position = Position,
                        Rotation = Quaternion.FromEulerAngles(Rotation),
                        Color = Color.ToVector4(),
                        Intensity = Intensity,
                        Radius = Radius,
                        Height = Height
                    };
                case LightTypes.Directional:
                    return new DirectionalLight()//Vertices, TriangleIndices, Color)
                    {
                        Position = Position,
                        Rotation = Quaternion.FromEulerAngles(Rotation),
                        Color = Color.ToVector4(),
                        Intensity = Intensity,
                    };
            }

            throw new NotImplementedException();
        }
    }
}
