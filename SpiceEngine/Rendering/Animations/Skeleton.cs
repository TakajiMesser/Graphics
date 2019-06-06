namespace SpiceEngine.Rendering.Animations
{
    /// <summary>
    /// A skeleton defined a specific joint hierarchy that the Actor must match.
    /// Allows for re-using animations across various models.
    /// </summary>
    public class Skeleton
    {
        public string Name { get; set; }
        public Bone Root { get; set; }
    }
}
