using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingTool.Structure
{
    public class GameProject
    {
        public const string FILE_EXTENSION = ".pro";

        public string Name { get; set; }

        public List<string> MapPaths { get; set; }
        public List<string> MeshPaths { get; set; }
        public List<string> BehaviorPaths { get; set; }
        public List<string> TexturePaths { get; set; }
        public List<string> AudioPaths { get; set; }
    }
}
