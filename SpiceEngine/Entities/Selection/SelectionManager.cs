using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Rendering.Processing;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Entities.Selection
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

    public class SelectionManager : ISelectionProvider
    {
        private IEntityProvider _entityProvider;
        private ConcurrentDictionary<int, bool> _selectedByID = new ConcurrentDictionary<int, bool>();

        public SelectionManager(IEntityProvider entityProvider) => _entityProvider = entityProvider;

        //public IEnumerable<int> IDs => _selectedByID.Keys;
        public IEnumerable<int> SelectedIDs => _selectedByID.Where(kvp => kvp.Value).Select(kvp => kvp.Key);

        //public IEnumerable<IEntity> Entities => _selectedByID.Keys.Select(i => _entityProvider.GetEntity(i));
        public IEnumerable<IEntity> SelectedEntities => _selectedByID.Where(kvp => kvp.Value)
            .Select(kvp => _entityProvider.GetEntityOrDefault(kvp.Key))
            .Where(e => e != null);

        public SelectionTypes SelectionType { get; set; }

        //public int Count => _selectedByID.Count;
        public int SelectionCount => _selectedByID.Count(kvp => kvp.Value);

        public Vector3 Position
        {
            get
            {
                var positions = _selectedByID.Where(kvp => kvp.Value).Select(kvp => _entityProvider.GetEntity(kvp.Key).Position);

                return new Vector3()
                {
                    X = positions.Average(p => p.X),
                    Y = positions.Average(p => p.Y),
                    Z = positions.Average(p => p.Z)
                };
            }
        }

        /*public void SetSelectable(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                _selectedByID.AddOrUpdate(id, false, (i, b) => b);
            }
        }*/

        public void Select(int id) => _selectedByID.AddOrUpdate(id, true, (i, b) => true);

        public void Select(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                _selectedByID.AddOrUpdate(id, true, (i, b) => true);
            }
        }

        //public void Deselect(int id) => _selectedByID.TryUpdate(id, false, true);

        //public bool IsSelectable(int id) => _selectedByID.ContainsKey(id);

        public bool IsSelected(int id) => _selectedByID.TryGetValue(id, out bool value) && value;

        /*public IEnumerable<Duplication> DuplicateSelection()
        {
            // Create duplicates and overwrite SelectedEntities with them
            var duplications = new List<Duplication>();

            foreach (var entity in SelectedEntities)
            {
                var duplicateEntity = _entityProvider.DuplicateEntity(entity);
                // Need to duplicate colliders
                // Need to duplicate scripts

                duplications.Add(new Duplication(entity.ID, duplicateEntity.ID));
            }

            ClearSelection();
            SetSelectable(duplications.Select(d => d.DuplicatedID));

            return duplications;
        }*/

        public void Remove(int id) => _selectedByID.TryRemove(id, out bool value);

        /*public void SelectAll()
        {
            foreach (var id in _selectedByID.Keys)
            {
                _selectedByID.TryUpdate(id, true, false);
            }
        }*/

        public void Clear() => _selectedByID.Clear();

        public void ClearSelection()
        {
            foreach (var id in _selectedByID.Keys)
            {
                _selectedByID.TryUpdate(id, false, true);
            }
        }

        public void HandleEntityTransforms(TransformModes transformMode, Vector2 mouseDelta)
        {
            // TODO - Can use entity's current rotation to determine position adjustment by that angle, rather than by MouseDelta.Y
            switch (transformMode)
            {
                case TransformModes.Translate:
                    var translation = GetTranslation(mouseDelta);

                    foreach (var entity in SelectedEntities)
                    {
                        if (entity is ITexturedEntity texturedEntity && texturedEntity.IsInTextureMode)
                        {
                            texturedEntity.TranslateTexture(translation.X, translation.Y);
                        }
                        else
                        {
                            entity.Position += translation;
                        }
                    }
                    break;
                case TransformModes.Rotate:
                    var rotation = GetRotation(mouseDelta);

                    foreach (var entity in SelectedEntities)
                    {
                        if (entity is IRotate rotater)
                        {
                            rotater.Rotation = rotation * rotater.Rotation;
                        }
                    }
                    break;
                case TransformModes.Scale:
                    var scale = GetScale(mouseDelta);

                    foreach (var entity in SelectedEntities)
                    {
                        if (entity is IScale scaler)
                        {
                            scaler.Scale += scale;
                        }
                    }
                    break;
            }
        }

        private Vector3 GetTranslation(Vector2 mouseDelta)
        {
            var translation = Vector3.Zero;

            switch (SelectionType)
            {
                case SelectionTypes.Red:
                    translation.X -= mouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Green:
                    translation.Y -= mouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Blue:
                    translation.Z -= mouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Cyan:
                    translation.Y += mouseDelta.X * 0.002f;
                    translation.Z -= mouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Magenta:
                    translation.Z -= mouseDelta.Y * 0.002f;
                    translation.X += mouseDelta.X * 0.002f;
                    break;
                case SelectionTypes.Yellow:
                    translation.X += mouseDelta.X * 0.002f;
                    translation.Y -= mouseDelta.Y * 0.002f;
                    break;
            }

            return translation;
        }

        private Quaternion GetRotation(Vector2 mouseDelta)
        {
            var rotation = Quaternion.Identity;

            switch (SelectionType)
            {
                case SelectionTypes.Red:
                    //rotation.X -= mouseDelta.Y * 0.002f;
                    //rotation *= Quaternion.FromAxisAngle(Vector3.UnitZ, -mouseDelta.Y * 0.002f);
                    rotation = Quaternion.FromEulerAngles(-mouseDelta.Y * 0.002f, 0.0f, 0.0f) * rotation;
                    break;
                case SelectionTypes.Green:
                    //rotation.Y -= mouseDelta.Y * 0.002f;
                    //rotation *= Quaternion.FromAxisAngle(Vector3.UnitX, -mouseDelta.Y * 0.002f);
                    rotation = Quaternion.FromEulerAngles(0.0f, -mouseDelta.Y * 0.002f, 0.0f) * rotation;
                    break;
                case SelectionTypes.Blue:
                    //rotation.Z -= mouseDelta.Y * 0.002f;
                    //rotation *= Quaternion.FromAxisAngle(Vector3.UnitY, -mouseDelta.Y * 0.002f);
                    rotation = Quaternion.FromEulerAngles(0.0f, 0.0f, -mouseDelta.Y * 0.002f) * rotation;
                    break;
                case SelectionTypes.Cyan:
                    rotation.Y += mouseDelta.X * 0.002f;
                    rotation.Z -= mouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Magenta:
                    rotation.Z -= mouseDelta.Y * 0.002f;
                    rotation.X += mouseDelta.X * 0.002f;
                    break;
                case SelectionTypes.Yellow:
                    rotation.X += mouseDelta.X * 0.002f;
                    rotation.Y -= mouseDelta.Y * 0.002f;
                    break;
            }

            return rotation;
        }

        private Vector3 GetScale(Vector2 mouseDelta)
        {
            var scale = Vector3.One;

            switch (SelectionType)
            {
                case SelectionTypes.Red:
                    scale.X -= mouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Green:
                    scale.Y -= mouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Blue:
                    scale.Z -= mouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Cyan:
                    scale.Y += mouseDelta.X * 0.002f;
                    scale.Z -= mouseDelta.Y * 0.002f;
                    break;
                case SelectionTypes.Magenta:
                    scale.Z -= mouseDelta.Y * 0.002f;
                    scale.X += mouseDelta.X * 0.002f;
                    break;
                case SelectionTypes.Yellow:
                    scale.X += mouseDelta.X * 0.002f;
                    scale.Y -= mouseDelta.Y * 0.002f;
                    break;
            }

            return scale;
        }
    }
}
