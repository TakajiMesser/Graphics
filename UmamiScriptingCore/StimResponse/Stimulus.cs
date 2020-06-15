namespace UmamiScriptingCore.StimResponse
{
    public class Stimulus : IStimulus
    {
        public string Name { get; private set; }

        public static Stimulus Player => new Stimulus() { Name = "Player" };
        public static Stimulus Guard => new Stimulus() { Name = "Guard" };
        public static Stimulus Fire => new Stimulus() { Name = "Fire" };
        public static Stimulus Water => new Stimulus() { Name = "Water" };
    }
}
