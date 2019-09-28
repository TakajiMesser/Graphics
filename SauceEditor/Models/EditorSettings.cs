using OpenTK.Graphics;
using SauceEditor.Helpers;
using SpiceEngine.Game;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace SauceEditor.Models
{
    public enum ViewTypes
    {
        All,
        Perspective,
        X,
        Y,
        Z
    }

    public sealed class EditorSettings
    {
        public const string FILE_EXTENSION = ".user";

        private static Lazy<EditorSettings> _instance = new Lazy<EditorSettings>(() => LoadOrDefault());

        private EditorSettings() { }

        public Tools DefaultTool { get; set; }
        public ViewTypes DefaultView { get; set; }

        public int WireframeThickness { get; set; }
        public Color4 WireframeColor { get; set; }

        public int WireframeSelectedThickness { get; set; }
        public Color4 WireframeSelectedColor { get; set; }

        public int WireframeSelectedLightThickness { get; set; }
        public Color4 WireframeSelectedLightColor { get; set; }

        public string InitialProjectDirectory { get; set; } = FilePathHelper.INITIAL_PROJECT_DIRECTORY;
        public string InitialMapDirectory { get; set; } = FilePathHelper.INITIAL_MAP_DIRECTORY;
        public string InitialModelDirectory { get; set; } = FilePathHelper.INITIAL_MODEL_DIRECTORY;
        public string InitialBehaviorDirectory { get; set; } = FilePathHelper.INITIAL_BEHAVIOR_DIRECTORY;
        public string InitialTextureDirectory { get; set; } = FilePathHelper.INITIAL_TEXTURE_DIRECTORY;
        public string InitialSoundDirectory { get; set; } = FilePathHelper.INITIAL_SOUND_DIRECTORY;
        public string InitialMaterialDirectory { get; set; } = FilePathHelper.INITIAL_MATERIAL_DIRECTORY;
        public string InitialArchetypeDirectory { get; set; } = FilePathHelper.INITIAL_ARCHETYPE_DIRECTORY;
        public string InitialScriptDirectory { get; set; } = FilePathHelper.INITIAL_SCRIPT_DIRECTORY;

        public static EditorSettings Instance => _instance.Value;

        public void Save(string path)
        {
            using (var writer = XmlWriter.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                serializer.WriteObject(writer, this);
            }
        }

        public static EditorSettings Load(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                return serializer.ReadObject(reader, true) as EditorSettings;
            }
        }

        public static void Reload() => _instance = new Lazy<EditorSettings>(() => LoadOrDefault());

        private static EditorSettings LoadOrDefault()
        {
            var path = Helpers.FilePathHelper.SETTINGS_PATH;
            return File.Exists(path) ? Load(path) : new EditorSettings();
        }
    }
}
