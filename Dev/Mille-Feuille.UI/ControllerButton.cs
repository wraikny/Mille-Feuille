using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.UI
{
    public enum ButtonDirection
    {
        Up,
        Down,
        Right,
        Left,
    }

    public class ControllerButton
    {
        public asd.Object2D Owner { get; }

        private readonly Dictionary<ButtonDirection, ControllerButton>
            connectedButtons;

        public IReadOnlyDictionary<ButtonDirection, ControllerButton>
            ConnectedButtons => connectedButtons;

        public ControllerButton()
        {
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

        public ControllerButton(asd.Object2D owner)
        {
            Owner = owner;
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
