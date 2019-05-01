using OpenTK;

namespace SauceEditor.ViewModels.Properties
{
    public class TransformProperty : ViewModel
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public TransformProperty(Vector3 vector)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
        }

        public Vector3 ToVector3() => new Vector3((float)X, (float)Y, (float)Z);
    }
}