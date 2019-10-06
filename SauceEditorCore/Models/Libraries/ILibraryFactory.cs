using SauceEditorCore.Models.Components;
using System.Collections.Generic;

namespace SauceEditorCore.Models.Libraries
{
    public interface ILibraryFactory
    {
        LibraryNode RootNode { get; }

        string Path { get; set; }

        Library<MapComponent> MapLibrary { get; }
        Library<ModelComponent> ModelLibrary { get; }
        Library<BehaviorComponent> BehaviorLibrary { get; }
        Library<TextureComponent> TextureLibrary { get; }
        Library<SoundComponent> SoundLibrary { get; }
        Library<MaterialComponent> MaterialLibrary { get; }
        Library<ArchetypeComponent> ArchetypeLibrary { get; }
        Library<ScriptComponent> ScriptLibrary { get; }

        IEnumerable<IPathInfo> GetMapLibraryPaths();
        IEnumerable<IPathInfo> GetModelLibraryPaths();
        IEnumerable<IPathInfo> GetBehaviorLibraryPaths();
        IEnumerable<IPathInfo> GetTextureLibraryPaths();
        IEnumerable<IPathInfo> GetSoundLibraryPaths();
        IEnumerable<IPathInfo> GetMaterialLibraryPaths();
        IEnumerable<IPathInfo> GetArchetypeLibraryPaths();
        IEnumerable<IPathInfo> GetScriptLibraryPaths();

        IComponent GetComponent(string path);
        LibraryInfo GetNodeInfo(string path);
        IEnumerable<ILibrary> GetLibraries();
        IEnumerable<LibraryInfo> GetBaseLibraryPaths();

        void Save();
        void Load();
    }
}
