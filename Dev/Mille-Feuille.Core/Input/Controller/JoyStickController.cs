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

    /// <summary>
    /// ジョイスティックの入力と操作を対応付けるためのクラス。
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
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
        private readonly Dictionary<TControl, int> axisTiltBinding;

        /// <summary>
        /// コントローラーが有効かどうかを取得する。
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// 入力に対応付けられている操作のコレクションを取得する。
        /// </summary>
        public IEnumerable<TControl> Keys => binding.Keys;

        public JoystickController(int index)
        {
            IsValid = asd.Engine.JoystickContainer.GetIsPresentAt(index);

            joystick = asd.Engine.JoystickContainer.GetJoystickAt(index);

            binding = new Dictionary<TControl, IJoystickInput>();
            axisTiltBinding = new Dictionary<TControl, int>();
        }

        /// <summary>
        /// ボタン入力に操作を対応付ける。
        /// </summary>
        /// <param name="abstractKey"></param>
        /// <param name="buttonIndex"></param>
        public void BindButton(TControl abstractKey, int buttonIndex)
        {
            binding[abstractKey] = new ButtonInput(buttonIndex);
        }

        /// <summary>
        /// スティック入力に操作を対応付ける。
        /// </summary>
        /// <param name="abstractKey"></param>
        /// <param name="axisIndex"></param>
        /// <param name="direction"></param>
        public void BindAxis(TControl abstractKey, int axisIndex, AxisDirection direction)
        {
            binding[abstractKey] = new AxisInput(axisIndex, direction);
        }

        /// <summary>
        /// スティック入力の上下左右に操作を対応付ける。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="up"></param>
        /// <param name="down"></param>
        public void BindDirection(TControl left, TControl right, TControl up, TControl down)
        {
            BindAxis(left, 0, AxisDirection.Negative);
            BindAxis(right, 0, AxisDirection.Positive);
            BindAxis(up, 1, AxisDirection.Negative);
            BindAxis(down, 1, AxisDirection.Positive);
        }

        /// <summary>
        /// 操作に対応する入力状態を取得する。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public asd.ButtonState? GetState(TControl key)
        {
            if (IsValid && binding.TryGetValue(key, out var input))
            {
                return input.GetState(joystick);
            }

            return null;
        }

        /// <summary>
        /// スティックのインデックスに操作を対応付ける。
        /// </summary>
        /// <param name="abstractKey"></param>
        /// <param name="index"></param>
        public void BindAxisTilt(TControl abstractKey, int index)
        {
            axisTiltBinding[abstractKey] = index;
        }

        /// <summary>
        /// 操作に対応する軸の倒し具合を取得する。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float? GetAxisTilt(TControl key)
        {
            if(IsValid && axisTiltBinding.TryGetValue(key, out var index))
            {
                return joystick.GetAxisState(index);
            }
            return null;
        }

        /// <summary>
        /// コントローラーの状態を更新する。
        /// </summary>
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
