using Newtonsoft.Json;
using SavoryPhysicsCore.Partitioning;
using System.Linq;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngine.Maps
{
    /// <summary>
    /// A map should consist of a collection of static brushes, actors, a camera, and/or a player (cutscenes and menu's won't have a player)
    /// 
    /// </summary>
    public class Map2D : Map
    {
        [JsonIgnore]
        public Quad Boundaries { get; private set; }

        public Map2D() { }

        protected override void CalculateBounds()
        {
            var min = new Vector2(
                new float[]
                    {
                        Actors.Min(a => a.Position.X),
                        Brushes.Min(b => b.Position.X),
                        //Volumes.Min(v => v.Position.X),
                        Lights.Min(l => l.Position.X)
                    }.Min(),
                new float[]
                    {
                        Actors.Min(a => a.Position.Y),
                        Brushes.Min(b => b.Position.Y),
                        //Volumes.Min(v => v.Position.Y),
                        Lights.Min(l => l.Position.Y)
                    }.Min()
            );

            var max = new Vector2(
                new float[]
                    {
                        Actors.Max(a => a.Position.X),
                        Brushes.Max(b => b.Position.X),
                        //Volumes.Max(v => v.Position.X),
                        Lights.Max(l => l.Position.X)
                    }.Max(),
                new float[]
                    {
                        Actors.Max(a => a.Position.Y),
                        Brushes.Max(b => b.Position.Y),
                        //Volumes.Max(v => v.Position.Y),
                        Lights.Max(l => l.Position.Y)
                    }.Max()
            );

            Boundaries = new Quad(min, max);
        }
    }
}
