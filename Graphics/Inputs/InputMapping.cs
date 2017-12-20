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
        public Key Forward { get; set; } = Key.W;
        public Key Backward { get; set; } = Key.S;
        public Key Left { get; set; } = Key.A;
        public Key Right { get; set; } = Key.D;

        // Other Movement
        public Key Run { get; set; } = Key.ShiftLeft;
        public Key Crawl { get; set; } = Key.ControlLeft;
        public Key Evade { get; set; } = Key.Space;
        public Key In { get; set; } = Key.Q;
        public Key Out { get; set; } = Key.E;

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
