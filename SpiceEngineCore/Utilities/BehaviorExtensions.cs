namespace SpiceEngineCore.Utilities
{
    public enum BehaviorStatus
    {
        Dormant,
        Running,
        Success,
        Failure
    }

    public static class BehaviorExtensions
    {
        public static bool IsComplete(this BehaviorStatus status) => status == BehaviorStatus.Success || status == BehaviorStatus.Failure;
    }
}
