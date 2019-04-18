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
            switch (self)
            {
                case ButtonDirection.Up: return ButtonDirection.Down;
                case ButtonDirection.Down: return ButtonDirection.Up;
                case ButtonDirection.Right: return ButtonDirection.Left;
                case ButtonDirection.Left: return ButtonDirection.Right;
                default: throw new Exception();
            }
        }
    }

    public interface IControllerButton
    {
        IControllerButton GetButton(ButtonDirection dir);
        IControllerButton SetButton(ButtonDirection dir, IControllerButton button);
        IControllerButton Chain(IControllerButton next, ButtonDirection dir);
        void Update(ButtonOperation operation);
    }

    public class ControllerButtonComponent<T> : ButtonComponent<T>, IControllerButton
        where T : asd.Object2D
    {
        private readonly Dictionary<ButtonDirection, IControllerButton> connectedButtons;

        public IReadOnlyDictionary<ButtonDirection, IControllerButton>
            ConnectedButtons => connectedButtons;

        
        public ControllerButtonComponent()
        {
            connectedButtons = new Dictionary<ButtonDirection, IControllerButton>();
        }

        public IControllerButton GetButton(ButtonDirection dir)
        {
            connectedButtons.TryGetValue(dir, out var button);
            return button;
        }

        public IControllerButton SetButton(ButtonDirection dir, IControllerButton button)
        {
            connectedButtons[dir] = button;
            return this;
        }

        public IControllerButton Chain(IControllerButton next, ButtonDirection dir)
        {
            this.SetButton(dir, next);
            next.SetButton(dir.Reverse(), this);

            return next;
        }

        public void Update(ButtonOperation operation)
        {
            UpdateState(operation);
        }

        public static void ConnetButtons(
            IReadOnlyCollection<IControllerButton> buttons
            , ButtonDirection dir
        )
        {
            var count = buttons.Count();

            for (int i = 0; i < count - 2; i++)
            {
                var b1 = buttons.ElementAt(i);
                var b2 = buttons.ElementAt(i + 1);

                b1.Chain(b2, dir);
            }
        }
    }
}
