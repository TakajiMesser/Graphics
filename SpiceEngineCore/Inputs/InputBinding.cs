using OpenTK.Input;

namespace SpiceEngineCore.Inputs
{
    public class InputBinding
    {
        // Movement
        public Input Forward { get; set; } = new Input(Key.W);
        public Input Backward { get; set; } = new Input(Key.S);
        public Input Left { get; set; } = new Input(Key.A);
        public Input Right { get; set; } = new Input(Key.D);

        // Other Movement
        public Input Run { get; set; } = new Input(Key.ShiftLeft);
        public Input Crawl { get; set; } = new Input(Key.ControlLeft);
        public Input Block { get; set; } = new Input(Key.Space);
        public Input Evade { get; set; } = new Input(Key.Space);
        public Input In { get; set; } = new Input(Key.Q);
        public Input Out { get; set; } = new Input(Key.E);

        // Actions
        public Input Action { get; set; } = new Input(MouseButton.Left);
        public Input Cover { get; set; } = new Input(MouseButton.Right);
        public Input Use { get; set; } = new Input(Key.F);

        // Inventory
        public Input ItemWheel { get; set; } = new Input(MouseButton.Middle);
        public Input ItemSlot1 { get; set; } = new Input(Key.Number1);
        public Input ItemSlot2 { get; set; } = new Input(Key.Number2);
        public Input ItemSlot3 { get; set; } = new Input(Key.Number3);
        public Input ItemSlot4 { get; set; } = new Input(Key.Number4);
        public Input ItemSlot5 { get; set; } = new Input(Key.Number5);
        public Input ItemSlot6 { get; set; } = new Input(Key.Number6);
        public Input ItemSlot7 { get; set; } = new Input(Key.Number7);
        public Input ItemSlot8 { get; set; } = new Input(Key.Number8);
    }
}
