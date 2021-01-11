namespace TangyHIDCore.Inputs
{
    public class InputBinding
    {
        // Movement
        public Input Forward { get; set; } = new Input(Keys.W);
        public Input Backward { get; set; } = new Input(Keys.S);
        public Input Left { get; set; } = new Input(Keys.A);
        public Input Right { get; set; } = new Input(Keys.D);

        // Other Movement
        public Input Run { get; set; } = new Input(Keys.LeftShift);
        public Input Crawl { get; set; } = new Input(Keys.LeftControl);
        public Input Block { get; set; } = new Input(Keys.Space);
        public Input Evade { get; set; } = new Input(Keys.Space);
        public Input In { get; set; } = new Input(Keys.Q);
        public Input Out { get; set; } = new Input(Keys.E);

        // Actions
        public Input Action { get; set; } = new Input(MouseButtons.Left);
        public Input Cover { get; set; } = new Input(MouseButtons.Right);
        public Input Use { get; set; } = new Input(Keys.F);

        // Inventory
        public Input ItemWheel { get; set; } = new Input(MouseButtons.Middle);
        public Input ItemSlot1 { get; set; } = new Input(Keys.Num1);
        public Input ItemSlot2 { get; set; } = new Input(Keys.Num2);
        public Input ItemSlot3 { get; set; } = new Input(Keys.Num3);
        public Input ItemSlot4 { get; set; } = new Input(Keys.Num4);
        public Input ItemSlot5 { get; set; } = new Input(Keys.Num5);
        public Input ItemSlot6 { get; set; } = new Input(Keys.Num6);
        public Input ItemSlot7 { get; set; } = new Input(Keys.Num7);
        public Input ItemSlot8 { get; set; } = new Input(Keys.Num8);
    }
}
