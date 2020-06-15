using SavoryPhysicsCore.Shapes;

namespace SauceEditorCore.Models.BSP
{
    public enum SolidTypes
    {
        Additive,
        Subtractive
    }

    public interface ISolid
    {
        SolidTypes SolidType { get; }

        IShape ToShape();
    }
}
