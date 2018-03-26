using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Scripting.StimResponse
{
    public enum StimType
    {
        Contact,
        Radius,
        Sight
    }

    public class Stimulus
    {
        public StimType StimType { get; set; }
    }
}
