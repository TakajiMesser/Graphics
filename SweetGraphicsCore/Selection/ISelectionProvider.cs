using SpiceEngineCore.Entities;
using System.Collections.Generic;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SweetGraphicsCore.Selection
{
    public enum SelectionTypes
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow
    }

    public enum TransformModes
    {
        Translate,
        Rotate,
        Scale
    }

    public interface ISelectionProvider
    {
        //IEnumerable<int> IDs { get; }
        IEnumerable<int> SelectedIDs { get; }

        //IEnumerable<IEntity> Entities { get; }
        IEnumerable<IEntity> SelectedEntities { get; }

        SelectionTypes SelectionType { get; set; }

        //int Count { get; }
        int SelectionCount { get; }

        Vector3 Position { get; }

        //void SetSelectable(IEnumerable<int> ids);

        void Select(int id);
        void Select(IEnumerable<int> ids);
        //void Deselect(int id);

        //bool IsSelectable(int id);
        bool IsSelected(int id);

        //IEnumerable<Duplication> DuplicateSelection();

        void Remove(int id);
        //void SelectAll();

        void Clear();
        void ClearSelection();
    }
}
