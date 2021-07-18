using SpiceEngine.GLFWBindings.Inputs;
using SpiceEngineCore.Helpers;
using System.Collections.Generic;

namespace TangyHIDCore.Inputs
{
    public class InputMapping
    {
        private ListDictionary<string, InputBinding> _bindingsByName = new ListDictionary<string, InputBinding>();

        public void AddBinding(string name, InputBinding binding) => _bindingsByName.Add(name, binding);

        public void AddBinding(string name, Keys key) => AddBinding(name, new InputBinding(name) { Key = key });
        public void AddBinding(string name, MouseButtons mouseButton) => AddBinding(name, new InputBinding(name) { MouseButton = mouseButton });
        public void AddBinding(string name, GamePadButtons gamePadButton) => AddBinding(name, new InputBinding(name) { GamePadButton = gamePadButton });

        public IEnumerable<InputBinding> GetBindings(string name) => _bindingsByName.GetValues(name);

        public static InputMapping Default()
        {
            var mapping = new InputMapping();

            // Movement
            mapping.AddBinding("Forward", Keys.W);
            mapping.AddBinding("Backward", Keys.S);
            mapping.AddBinding("Left", Keys.A);
            mapping.AddBinding("Right", Keys.D);

            // Other Movement
            mapping.AddBinding("Run", Keys.LeftShift);
            mapping.AddBinding("Crawl", Keys.LeftControl);
            mapping.AddBinding("Block", Keys.Space);
            mapping.AddBinding("Evade", Keys.Space);
            mapping.AddBinding("In", Keys.Q);
            mapping.AddBinding("Out", Keys.E);

            // Actions 
            mapping.AddBinding("Action", MouseButtons.Left);
            mapping.AddBinding("Cover", MouseButtons.Right);
            mapping.AddBinding("Use", Keys.F);

            // Inventory
            mapping.AddBinding("ItemWheel", MouseButtons.Middle);
            mapping.AddBinding("ItemSlot1", Keys.Num1);
            mapping.AddBinding("ItemSlot2", Keys.Num2);
            mapping.AddBinding("ItemSlot3", Keys.Num3);
            mapping.AddBinding("ItemSlot4", Keys.Num4);
            mapping.AddBinding("ItemSlot5", Keys.Num5);
            mapping.AddBinding("ItemSlot6", Keys.Num6);
            mapping.AddBinding("ItemSlot7", Keys.Num7);
            mapping.AddBinding("ItemSlot8", Keys.Num8);

            return mapping;
        }
    }
}
