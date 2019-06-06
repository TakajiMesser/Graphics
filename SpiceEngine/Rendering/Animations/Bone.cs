namespace SpiceEngine.Rendering.Animations
{
    public class Bone
    {
        public string Name { get; set; }
        public List<Bone> Children { get; set; } = new List<Bone>();

        public Bone() { }
        public Bone(Assimp.Node node)
        {
            Name = node.Name;

            foreach (var childNode in node.Children)
            {
                Children.Add(new Bone(childNode));
            }
        }
    }
}
