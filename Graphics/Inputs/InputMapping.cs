using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace Graphics.Inputs
{
    public class InputMapping
    {
        // Movement
        public Input Forward { get; set; } = new Input(Key.W);
        public Input Backward { get; set; } = new Input(Key.S);
        public Input Left { get; set; } = new Input(Key.A);
        public Input Right { get; set; } = new Input(Key.D);

        // Other Movement
        public Input Run { get; set; } = new Input(Key.ShiftLeft);
        public Input Crawl { get; set; } = new Input(Key.ControlLeft);
        public Input Evade { get; set; } = new Input(Key.Space);
        public Input In { get; set; } = new Input(Key.Q);
        public Input Out { get; set; } = new Input(Key.E);

        // Actions
        public MouseButton Action { get; set; } = MouseButton.Left;
        public MouseButton Cover { get; set; } = MouseButton.Right;
        public Key Use { get; set; } = Key.F;

        // Inventory
        public MouseButton ItemWheel { get; set; } = MouseButton.Middle;
        public Key ItemSlot1 { get; set; } = Key.Number1;
        public Key ItemSlot2 { get; set; } = Key.Number2;
        public Key ItemSlot3 { get; set; } = Key.Number3;
        public Key ItemSlot4 { get; set; } = Key.Number4;
        public Key ItemSlot5 { get; set; } = Key.Number5;
        public Key ItemSlot6 { get; set; } = Key.Number6;
        public Key ItemSlot7 { get; set; } = Key.Number7;
        public Key ItemSlot8 { get; set; } = Key.Number8;
    }
}
