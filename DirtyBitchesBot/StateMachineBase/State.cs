namespace DirtyBitchesBot.StateMachineBase
{
    public class State
    {
        public string? StateName { get; set; }
        public dynamic StateObject { get; set; } = new System.Dynamic.ExpandoObject();
    }
}
