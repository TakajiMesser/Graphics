namespace StarchUICore.Traversal
{
    public enum LayoutSteps
    {
        X,
        Y,
        Width,
        Height
    }

    public struct LayoutDependency
    {
        public LayoutDependency(int entityID, LayoutSteps step)
        {
            EntityID = entityID;
            Step = step;
        }

        public int EntityID { get; private set; }
        public LayoutSteps Step { get; private set; }

        public static LayoutDependency X(int entityID) => new LayoutDependency(entityID, LayoutSteps.X);
        public static LayoutDependency Y(int entityID) => new LayoutDependency(entityID, LayoutSteps.Y);
        public static LayoutDependency Width(int entityID) => new LayoutDependency(entityID, LayoutSteps.Width);
        public static LayoutDependency Height(int entityID) => new LayoutDependency(entityID, LayoutSteps.Height);
    }
}
