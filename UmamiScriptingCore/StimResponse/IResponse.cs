using System;
using UmamiScriptingCore.Behaviors;

namespace UmamiScriptingCore.StimResponse
{
    public interface IResponse
    {
        IStimulus Stimulus { get; set; }

        bool TriggerOnContact { get; set; }
        bool TriggerOnProximity { get; set; }
        bool TriggerOnSight { get; set; }

        double ProximityRadius { get; set; }
        double SightDistance { get; set; }
        double SightAngle { get; set; }

        event EventHandler<StimulusTriggeredEventArgs> Triggered;

        void Tick(BehaviorContext context);
    }
}
