using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TakoEngine.Game;

namespace SauceEditor.Structure
{
    public enum ViewTypes
    {
        All,
        Perspective,
        X,
        Y,
        Z
    }

    public class EditorSettings
    {
        public const string FILE_EXTENSION = ".user";

        public Tools DefaultTool { get; set; }
        public ViewTypes DefaultView { get; set; }

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
    }
}
