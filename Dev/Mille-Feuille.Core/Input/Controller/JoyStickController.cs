using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Input.Controller
{
    public enum AxisDirection
    {
        Negative,
        Positive,
    }

    public class JoystickController<TControl> : IController<TControl>
    {
        private interface IJoystickInput
        {
            asd.ButtonState GetState(asd.Joystick joystick);

            void Update(asd.Joystick joystick);
        }

        private class ButtonInput : IJoystickInput
        {
            private readonly int index;

            public ButtonInput(int index)
            {
                this.index = index;
            }

            public asd.ButtonState GetState(asd.Joystick joystick)
            {
                return joystick.GetButtonState(index);
            }

            public void Update(asd.Joystick joystick) { }
        }

        private class AxisInput : IJoystickInput
        {
            private readonly int axisIndex;
            private readonly int direction;
            private bool previousState;
            private bool currentState;

            public AxisInput(int axisIndex, AxisDirection direction)
            {
                this.axisIndex = axisIndex;
                this.direction = direction == AxisDirection.Positive ? 1 : -1;
                previousState = false;
                currentState = false;
            }

            public asd.ButtonState GetState(asd.Joystick joystick)
            {
                if (currentState)
                {
                    return previousState
                        ? asd.ButtonState.Hold
                        : asd.ButtonState.Push;
                }
                else
                {
                    return previousState
                        ? asd.ButtonState.Release
                        : asd.ButtonState.Free;
                }
            }

            public void Update(asd.Joystick joystick)
            {
                previousState = currentState;
                currentState = joystick.GetAxisState(axisIndex) == direction;
            }
        }

        private readonly asd.Joystick joystick;
        private readonly Dictionary<TControl, IJoystickInput> binding;

        public bool IsValid { get; private set; }

        public IEnumerable<TControl> Keys => binding.Keys;

        public JoystickController(int index)
        {
            IsValid = asd.Engine.JoystickContainer.GetIsPresentAt(index);

            joystick = asd.Engine.JoystickContainer.GetJoystickAt(index);

            binding = new Dictionary<TControl, IJoystickInput>();
        }

        public void BindButton(TControl abstractKey, int buttonIndex)
        {
            binding[abstractKey] = new ButtonInput(buttonIndex);
        }

        public void BindAxis(TControl abstractKey, int axisIndex, AxisDirection direction)
        {
            binding[abstractKey] = new AxisInput(axisIndex, direction);
        }

        public void BindDirection(TControl left, TControl right, TControl up, TControl down)
        {
            BindAxis(left, 0, AxisDirection.Negative);
            BindAxis(right, 0, AxisDirection.Positive);
            BindAxis(up, 1, AxisDirection.Negative);
            BindAxis(down, 1, AxisDirection.Positive);
        }

        public asd.ButtonState? GetState(TControl key)
        {
            if (IsValid && binding.TryGetValue(key, out var input))
            {
                return input.GetState(joystick);
            }

            return null;
        }

        public void Update()
        {
            if (!IsValid)
            {
                return;
            }

            foreach (var input in binding.Values)
            {
                input.Update(joystick);
            }
        }
    }
}
