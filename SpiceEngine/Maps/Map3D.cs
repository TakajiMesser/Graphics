using OpenTK;
using SpiceEngine.Physics.Collision;
using System.Linq;
using System.Runtime.Serialization;

namespace SpiceEngine.Maps
{
    /// <summary>
    /// A map should consist of a collection of static brushes, actors, a camera, and/or a player (cutscenes and menu's won't have a player)
    /// 
    /// </summary>
    public class Map3D : Map
    {
        [IgnoreDataMember]
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
                    }.Min(),
                    Z = new float[]
                    {
                        Actors.Min(a => a.Position.Z),
                        Brushes.Min(b => b.Position.Z),
                        //Volumes.Min(v => v.Position.Z),
                        Lights.Min(l => l.Position.Z)
                    }.Min()
                },
                Max = new Vector3
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
                    }.Max(),
                    Z = new float[]
                    {
                        Actors.Max(a => a.Position.Z),
                        Brushes.Max(b => b.Position.Z),
                        //Volumes.Max(v => v.Position.Z),
                        Lights.Max(l => l.Position.Z)
                    }.Max()
                }
            };
        }
    }
}
