using Newtonsoft.Json;
using SavoryPhysicsCore.Partitioning;
using System.Linq;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
            var min = new Vector2
            {
                X = new float[]
                    {
                        Actors.Min(a => a.Position.X),
                        Brushes.Min(b => b.Position.X),
                        //Volumes.Min(v => v.Position.X),
                        Lights.Min(l => l.Position.X)
                    }.Min(),
                Y = new float[]
                    {
                        Actors.Min(a => a.Position.Y),
                        Brushes.Min(b => b.Position.Y),
                        //Volumes.Min(v => v.Position.Y),
                        Lights.Min(l => l.Position.Y)
                    }.Min()
            };

            var max = new Vector2
            {
                X = new float[]
                    {
                        Actors.Max(a => a.Position.X),
                        Brushes.Max(b => b.Position.X),
                        //Volumes.Max(v => v.Position.X),
                        Lights.Max(l => l.Position.X)
                    }.Max(),
                Y = new float[]
                    {
                        Actors.Max(a => a.Position.Y),
                        Brushes.Max(b => b.Position.Y),
                        //Volumes.Max(v => v.Position.Y),
                        Lights.Max(l => l.Position.Y)
                    }.Max()
            };

            Boundaries = new Quad(min, max);
        }
    }
}
