using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Game.GameSettings
{
    public class Settings
    {
        public GameplaySettings GameplaySettings { get; set; }
        public ControlSettings ControlSettings { get; set; }
        public VideoSettings VideoSettings { get; set; }
        public AudioSettings AudioSettings { get; set; }
    }
}
