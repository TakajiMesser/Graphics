namespace UmamiScriptingCore.Utilities
{
    public static class BehaviorExtensions
    {
        public static bool IsComplete(this BehaviorStatus status) => status == BehaviorStatus.Success || status == BehaviorStatus.Failure;
    }
}
