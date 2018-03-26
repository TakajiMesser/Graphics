using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.Behaviors;

namespace TakoEngine.Scripting.StimResponse
{
    public class Response
    {
        public Stimulus Stimulus { get; private set; }

        /// <summary>
        /// How often to check for a received stimulus (in ticks)
        /// </summary>
        public int CheckFrequency { get; set; } = 1;

        public event EventHandler<StimulusReceivedEventArgs> StimulusReceived;

        private int _tick = 0;

        public Response(Stimulus stimulus)
        {
            Stimulus = stimulus;
        }

        public void Tick(BehaviorContext context)
        {
            _tick++;

            if (_tick >= CheckFrequency)
            {
                _tick = 0;

                switch (Stimulus.StimType)
                {
                    case StimType.Contact:
                        CheckForContactStimulus(context);
                        break;
                    case StimType.Radius:
                        CheckForRadiusStimulus(context);
                        break;
                    case StimType.Sight:
                        CheckForSightStimulus(context);
                        break;
                }
            }
        }

        private void CheckForContactStimulus(BehaviorContext context)
        {

        }

        private void CheckForRadiusStimulus(BehaviorContext context)
        {

        }

        private void CheckForSightStimulus(BehaviorContext context)
        {

        }
    }
}
