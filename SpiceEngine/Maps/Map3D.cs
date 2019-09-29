using Newtonsoft.Json;
using OpenTK;
using SpiceEngineCore.Physics.Collisions;
using System.Linq;

namespace SpiceEngine.Maps
{
    /// <summary>
    /// A map should consist of a collection of static brushes, actors, a camera, and/or a player (cutscenes and menu's won't have a player)
    /// 
    /// </summary>
    public class Map3D : Map
    {
        [JsonIgnore]
        public Oct Boundaries { get; private set; }

        public Map3D() { }

        protected override void CalculateBounds()
        {
            Boundaries = new Oct()
            {
                Min = new Vector3
                {
                    X = new float[]
                    {
                        Actors.Any() ? Actors.Min(a => a.Position.X) : 0.0f,
                        Brushes.Any() ? Brushes.Min(b => b.Position.X) : 0.0f,
                        //Volumes.Min(v => v.Position.X),
                        Lights.Any() ? Lights.Min(l => l.Position.X) : 0.0f
                    }.Min(),
                    Y = new float[]
                    {
                        Actors.Any() ? Actors.Min(a => a.Position.Y) : 0.0f,
                        Brushes.Any() ? Brushes.Min(b => b.Position.Y) : 0.0f,
                        //Volumes.Min(v => v.Position.Y),
                        Lights.Any() ? Lights.Min(l => l.Position.Y) : 0.0f
                    }.Min(),
                    Z = new float[]
                    {
                        Actors.Any() ? Actors.Min(a => a.Position.Z) : 0.0f,
                        Brushes.Any() ? Brushes.Min(b => b.Position.Z) : 0.0f,
                        //Volumes.Min(v => v.Position.Z),
                        Lights.Any() ? Lights.Min(l => l.Position.Z) : 0.0f
                    }.Min()
                },
                Max = new Vector3
                {
                    X = new float[]
                    {
                        Actors.Any() ? Actors.Max(a => a.Position.X) : 0.0f,
                        Brushes.Any() ? Brushes.Max(b => b.Position.X) : 0.0f,
                        //Volumes.Max(v => v.Position.X),
                        Lights.Any() ? Lights.Max(l => l.Position.X) : 0.0f
                    }.Max(),
                    Y = new float[]
                    {
                        Actors.Any() ? Actors.Max(a => a.Position.Y) : 0.0f,
                        Brushes.Any() ? Brushes.Max(b => b.Position.Y) : 0.0f,
                        //Volumes.Max(v => v.Position.Y),
                        Lights.Any() ? Lights.Max(l => l.Position.Y) : 0.0f
                    }.Max(),
                    Z = new float[]
                    {
                        Actors.Any() ? Actors.Max(a => a.Position.Z) : 0.0f,
                        Brushes.Any() ? Brushes.Max(b => b.Position.Z) : 0.0f,
                        //Volumes.Max(v => v.Position.Z),
                        Lights.Any() ? Lights.Max(l => l.Position.Z) : 0.0f
                    }.Max()
                }
            };
        }
    }
}
