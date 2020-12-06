using SpiceEngineCore.Entities;
using SpiceEngineCore.Geometry.Quaternions;
using SpiceEngineCore.Geometry.Vectors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SweetGraphicsCore.Selection
{
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

                return new Vector3(
                    positions.Average(p => p.X),
                    positions.Average(p => p.Y),
                    positions.Average(p => p.Z)
                );
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
                        if (entity is ITexturedEntity texturedEntity && texturedEntity.IsInTextureMode)
                        {
                            var angle = -mouseDelta.Y * 0.002f;
                            texturedEntity.RotateTexture(angle);
                        }
                        else if (entity is IRotate rotater)
                        {
                            rotater.Rotation = rotation * rotater.Rotation;
                        }
                    }
                    break;
                case TransformModes.Scale:
                    var scale = GetScale(mouseDelta);

                    foreach (var entity in SelectedEntities)
                    {
                        if (entity is ITexturedEntity texturedEntity && texturedEntity.IsInTextureMode)
                        {
                            texturedEntity.ScaleTexture(scale.X, scale.Y);
                        }
                        else if (entity is IScale scaler)
                        {
                            scaler.Scale += scale;
                        }
                    }
                    break;
            }
        }

        private Vector3 GetTranslation(Vector2 mouseDelta)
        {
            switch (SelectionType)
            {
                case SelectionTypes.Red:
                    return new Vector3(-mouseDelta.Y * 0.002f, 0.0f, 0.0f);
                case SelectionTypes.Green:
                    return new Vector3(0.0f, -mouseDelta.Y * 0.002f, 0.0f);
                case SelectionTypes.Blue:
                    return new Vector3(0.0f, 0.0f, -mouseDelta.Y * 0.002f);
                case SelectionTypes.Cyan:
                    return new Vector3(0.0f, mouseDelta.X * 0.002f, -mouseDelta.Y * 0.002f);
                case SelectionTypes.Magenta:
                    return new Vector3(mouseDelta.X * 0.002f, 0.0f, -mouseDelta.Y * 0.002f);
                case SelectionTypes.Yellow:
                    return new Vector3(mouseDelta.X * 0.002f, -mouseDelta.Y * 0.002f, 0.0f);
                case SelectionTypes.None:
                    return Vector3.Zero;
            }

            throw new ArgumentOutOfRangeException("Cannot handle selection type " + SelectionType);
        }

        private Quaternion GetRotation(Vector2 mouseDelta)
        {
            switch (SelectionType)
            {
                case SelectionTypes.Red:
                    return Quaternion.FromEulerAngles(-mouseDelta.Y * 0.002f, 0.0f, 0.0f);
                case SelectionTypes.Green:
                    return Quaternion.FromEulerAngles(0.0f, -mouseDelta.Y * 0.002f, 0.0f);
                case SelectionTypes.Blue:
                    return Quaternion.FromEulerAngles(0.0f, 0.0f, -mouseDelta.Y * 0.002f);
                case SelectionTypes.Cyan:
                    return new Quaternion(0.0f, mouseDelta.X * 0.002f, -mouseDelta.Y * 0.002f, 1.0f);
                case SelectionTypes.Magenta:
                    return new Quaternion(mouseDelta.X * 0.002f, 0.0f, -mouseDelta.Y * 0.002f, 1.0f);
                case SelectionTypes.Yellow:
                    return new Quaternion(mouseDelta.X * 0.002f, -mouseDelta.Y * 0.002f, 0.0f, 1.0f);
                case SelectionTypes.None:
                    return Quaternion.Identity;
            }

            throw new ArgumentOutOfRangeException("Cannot handle selection type " + SelectionType);
        }

        private Vector3 GetScale(Vector2 mouseDelta)
        {
            switch (SelectionType)
            {
                case SelectionTypes.Red:
                    return new Vector3(-mouseDelta.Y * 0.002f, 1.0f, 1.0f);
                case SelectionTypes.Green:
                    return new Vector3(1.0f, -mouseDelta.Y * 0.002f, 1.0f);
                case SelectionTypes.Blue:
                    return new Vector3(1.0f, 1.0f, -mouseDelta.Y * 0.002f);
                case SelectionTypes.Cyan:
                    return new Vector3(1.0f, mouseDelta.X * 0.002f, -mouseDelta.Y * 0.002f);
                case SelectionTypes.Magenta:
                    return new Vector3(mouseDelta.X * 0.002f, 1.0f, -mouseDelta.Y * 0.002f);
                case SelectionTypes.Yellow:
                    return new Vector3(mouseDelta.X * 0.002f, -mouseDelta.Y * 0.002f, 1.0f);
                case SelectionTypes.None:
                    return Vector3.One;
            }

            throw new ArgumentOutOfRangeException("Cannot handle selection type " + SelectionType);
        }
    }
}
