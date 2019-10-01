using System;
using SauceEditorCore.Models.Components;
using SpiceEngineCore.Helpers;

namespace SauceEditorCore.Helpers
{
    public static class ComponentFactory
    {
        public static bool IsValidExtension<T>(string extension) where T : IComponent
        {
            return new TypeSwitch<bool>()
                .Case<MapComponent>(() => MapComponent.IsValidExtension(extension))
                .Case<ModelComponent>(() => ModelComponent.IsValidExtension(extension))
                .Case<BehaviorComponent>(() => BehaviorComponent.IsValidExtension(extension))
                .Case<TextureComponent>(() => TextureComponent.IsValidExtension(extension))
                .Case<SoundComponent>(() => SoundComponent.IsValidExtension(extension))
                .Case<MaterialComponent>(() => MaterialComponent.IsValidExtension(extension))
                .Case<ArchetypeComponent>(() => ArchetypeComponent.IsValidExtension(extension))
                .Case<ScriptComponent>(() => ScriptComponent.IsValidExtension(extension))
                .Default(() => throw new NotImplementedException())
                .Match<T>();
        }

        public static IComponent Create<T>(string filePath) where T : IComponent
        {
            return new TypeSwitch<IComponent>()
                .Case<MapComponent>(() => new MapComponent(filePath))
                .Case<ModelComponent>(() => new ModelComponent(filePath))
                .Case<BehaviorComponent>(() => new BehaviorComponent(filePath))
                .Case<TextureComponent>(() => new TextureComponent(filePath))
                .Case<SoundComponent>(() => new SoundComponent(filePath))
                .Case<MaterialComponent>(() => new MaterialComponent(filePath))
                .Case<ArchetypeComponent>(() => new ArchetypeComponent(filePath))
                .Case<ScriptComponent>(() => new ScriptComponent(filePath))
                .Default(() => throw new NotImplementedException())
                .Match<T>();
        }

        /*private RelayCommand GetCreateCommand(IComponentFactory componentFactory) => new TypeSwitch<RelayCommand>()
            .Case<MapComponent>(() => new RelayCommand(p => componentFactory.CreateMap()))
            .Case<ModelComponent>(() => new RelayCommand(p => componentFactory.CreateModel()))
            .Case<BehaviorComponent>(() => new RelayCommand(p => componentFactory.CreateBehavior()))
            .Case<TextureComponent>(() => new RelayCommand(p => componentFactory.CreateTexture()))
            .Case<SoundComponent>(() => new RelayCommand(p => componentFactory.CreateSound()))
            .Case<MaterialComponent>(() => new RelayCommand(p => componentFactory.CreateMaterial()))
            .Case<ArchetypeComponent>(() => new RelayCommand(p => componentFactory.CreateArchetype()))
            .Case<ScriptComponent>(() => new RelayCommand(p => componentFactory.CreateScript()))
            .Default(() => throw new NotImplementedException())
            .Match<T>();*/
    }
}
