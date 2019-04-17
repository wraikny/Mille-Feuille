using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.UI.Button
{
    public enum ButtonDirection
    {
        Up,
        Down,
        Right,
        Left,
    }

    public static class ButtonDirectionExt
    {
        public static ButtonDirection Reverse(this ButtonDirection self)
        {
            switch(self)
            {
                case ButtonDirection.Up: return ButtonDirection.Down;
                case ButtonDirection.Down: return ButtonDirection.Up;
                case ButtonDirection.Right: return ButtonDirection.Left;
                case ButtonDirection.Left: return ButtonDirection.Right;
                default: throw new Exception();
            }
        }
    }

    public class ControllerButton
    {
        public asd.Object2D Owner { get; }

        private readonly Dictionary<ButtonDirection, ControllerButton>
            connectedButtons;

        public IReadOnlyDictionary<ButtonDirection, ControllerButton>
            ConnectedButtons => connectedButtons;

        public ControllerButton(asd.Object2D owner)
        {
            Owner = owner;
            connectedButtons = new Dictionary<ButtonDirection, ControllerButton>();
        }

        public ControllerButton GetButton(ButtonDirection dir)
        {
            connectedButtons.TryGetValue(dir, out var button);
            return button;
        }

        public ControllerButton SetButton(ButtonDirection dir, ControllerButton button)
        {
            connectedButtons[dir] = button;
            return this;
        }

        public ControllerButton Chain(ControllerButton next, ButtonDirection dir)
        {
            this.SetButton(dir, next);
            next.SetButton(dir.Reverse(), this);

            return next;
        }

        public static void ConnetButtons(IReadOnlyCollection<ControllerButton> buttons, ButtonDirection dir)
        {
            var count = buttons.Count();

            for(int i = 0; i < count - 2; i++)
            {
                var b1 = buttons.ElementAt(i);
                var b2 = buttons.ElementAt(i + 1);

                b1.Chain(b2, dir);
            }
        }

        private static string GetKeyString()
        {
            return "__MilleFeuille_ControllerButton";
        }

        public ButtonComponentBase GetButtonComponent()
        {
            var key_ = GetKeyString();
            return (ButtonComponentBase)Owner.GetComponent(key_);
        }

        public void SetButtonComponent(ButtonComponentBase component)
        {
            var key_ = GetKeyString();
            Owner.RemoveComponent(key_);
            Owner.AddComponent(component, key_);
        }

        public bool HasButtonComponent()
        {
            return GetButtonComponent() != null;
        }

        public void UpdateButtonState(ButtonOperation operation)
        {
            var component = GetButtonComponent();
            if (component == null) return;

            component.UpdateState(operation);
        }
    }
}
