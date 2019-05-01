using OpenTK;
using OpenTK.Graphics;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SauceEditor.Views;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Utilities;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SauceEditor.ViewModels.Properties
{
    public class EntityPropertiesViewModel : ViewModel
    {
        private string _behaviorFilePath;

        public EntityProperties Entity { get; set; }

        public EditorEntity EditorEntity { get; set; }
        public Visibility EntitySelected { get; set; }
        public string EntityType { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public Color Color { get; set; }
        public GridLength NameRowHeight { get; set; }
        public GridLength RotationRowHeight { get; set; }
        public GridLength ScaleRowHeight { get; set; }
        public GridLength ColorRowHeight { get; set; }
        public GridLength ScriptRowHeight { get; set; }

        public TransformPanelViewModel PositionViewModel { get; set; }
        public TransformPanelViewModel RotationViewModel { get; set; }
        public TransformPanelViewModel ScaleViewModel { get; set; }

        private RelayCommand _openScriptCommand;
        public RelayCommand OpenScriptCommand
        {
            get
            {
                return _openScriptCommand ?? (_openScriptCommand = new RelayCommand(
                    p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath)),
                    p => true
                ));
            }
        }

        //public event EventHandler<EntityEventArgs> EntityUpdated;
        public event EventHandler<FileEventArgs> ScriptOpened;

        public void OnPositionChanged()
        {
            if (EditorEntity != null)
            {
                EditorEntity.Entity.Position = Position;
                EditorEntity.MapEntity.Position = Position;
            }
        }

        public void OnRotationChanged()
        {
            if (EditorEntity != null && EditorEntity.Entity is IRotate rotator)
            {
                rotator.Rotation = Quaternion.FromEulerAngles(Rotation.ToRadians());
                EditorEntity.MapEntity.Rotation = Rotation;
            }
        }

        public void OnScaleChanged()
        {
            if (EditorEntity != null && EditorEntity.Entity is IScale scaler)
            {
                scaler.Scale = Scale;
                EditorEntity.MapEntity.Scale = Scale;
            }
        }

        public void OnPositionViewModelChanged() => AddChild(PositionViewModel, (s, args) => Position = PositionViewModel.Transform);
        public void OnRotationViewModelChanged() => AddChild(RotationViewModel, (s, args) => Rotation = RotationViewModel.Transform);
        public void OnScaleViewModelChanged() => AddChild(ScaleViewModel, (s, args) => Scale = ScaleViewModel.Transform);

        public void UpdateFromModel(EditorEntity editorEntity)
        {
            //Entity = editorEntity != null ? new EntityProperties(editorEntity) : null;

            /*Entity = new EntityProperties(editorEntity?.MapEntity);
            EditorEntity = editorEntity;

            EntitySelected = editorEntity != null ? Visibility.Visible : Visibility.Collapsed;
            EntityType = editorEntity != null ? editorEntity.Entity.GetType().Name : "No Properties to Show";

            ID = editorEntity != null ? editorEntity.Entity.ID.ToString() : "";*/

            return;

            if (editorEntity != null && editorEntity.MapEntity is MapActor mapActor)
            {
                Name = mapActor.Name;
                NameRowHeight = GridLength.Auto;
                ScriptRowHeight = GridLength.Auto;
                _behaviorFilePath = mapActor.Behavior.FilePath;
            }
            else
            {
                Name = "";
                NameRowHeight = new GridLength(0);
                ScriptRowHeight = new GridLength(0);
                _behaviorFilePath = "";
            }

            PositionViewModel.UpdateFromModel(editorEntity != null ? editorEntity.MapEntity.Position : Vector3.Zero);

            if (editorEntity != null && editorEntity.Entity is IRotate)
            {
                RotationViewModel.UpdateFromModel(editorEntity.MapEntity.Rotation);
                RotationRowHeight = GridLength.Auto;
            }
            else
            {
                RotationViewModel.UpdateFromModel(Vector3.Zero);
                RotationRowHeight = new GridLength(0);
            }

            if (editorEntity != null && editorEntity.Entity is IScale)
            {
                ScaleViewModel.UpdateFromModel(editorEntity.MapEntity.Scale);
                ScaleRowHeight = GridLength.Auto;
            }
            else
            {
                ScaleViewModel.UpdateFromModel(Vector3.Zero);
                ScaleRowHeight = new GridLength(0);
            }

            if (editorEntity != null && editorEntity.MapEntity is MapLight mapLight)
            {
                Color = mapLight.Color.ToMediaColor();
                ColorRowHeight = GridLength.Auto;
            }
            else
            {
                Color = Color4.White.ToMediaColor();
                ColorRowHeight = new GridLength(0);
            }

            CommandManager.InvalidateRequerySuggested();
        }
    }
}