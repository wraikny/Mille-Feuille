using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.UI.Button
{
    /// <summary>
    /// 隣接するボタンの方向を表す列挙体。
    /// </summary>
    public enum ButtonDirection
    {
        Up,
        Down,
        Right,
        Left,
    }

    public static class ButtonDirectionExt
    {
        /// <summary>
        /// 逆の向きを取得する。
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
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

    /// <summary>
    /// 親クラスによらずコントローラー操作可能なボタンを表すインターフェース。
    /// </summary>
    public interface IControllerButton
    {
        IControllerButton GetButton(ButtonDirection dir);
        IControllerButton SetButton(ButtonDirection dir, IControllerButton button);
        IControllerButton Chain(IControllerButton next, ButtonDirection dir);
        asd.Object2DComponent GetComponent();
        void Update(ButtonOperation operation);
    }


    public static class ControllerButtonConnects
    {
        /// <summary>
        /// コレクションからボタンを相互に接続する。
        /// </summary>
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

    /// <summary>
    /// コントローラー操作可能なボタン機能を提供するコンポーネント。
    /// </summary>
    public sealed class ControllerButtonComponent<T> : ButtonComponent<T>, IControllerButton
        where T : asd.Object2D
    {
        private readonly Dictionary<ButtonDirection, IControllerButton> connectedButtons;

        public IReadOnlyDictionary<ButtonDirection, IControllerButton>
            ConnectedButtons => connectedButtons;

        
        public ControllerButtonComponent(string name)
            : base(name)
        {
            connectedButtons = new Dictionary<ButtonDirection, IControllerButton>();
        }

        /// <summary>
        /// その向きに接続されたボタンを取得する。
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public IControllerButton GetButton(ButtonDirection dir)
        {
            connectedButtons.TryGetValue(dir, out var button);
            return button;
        }

        /// <summary>
        /// その向きにボタンを接続する。
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public IControllerButton SetButton(ButtonDirection dir, IControllerButton button)
        {
            if (button == null)
            {
                throw new ArgumentNullException(nameof(button));
            }

            connectedButtons[dir] = button;
            return this;
        }

        public asd.Object2DComponent GetComponent()
        {
            return this;
        }

        /// <summary>
        /// ボタンを相互に接続し、繋げていく。
        /// </summary>
        /// <param name="next"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public IControllerButton Chain(IControllerButton next, ButtonDirection dir)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            this.SetButton(dir, next);
            next.SetButton(dir.Reverse(), this);

            return next;
        }

        /// <summary>
        /// 操作を元にボタンの状態の更新を行う。
        /// </summary>
        /// <param name="operation"></param>
        public void Update(ButtonOperation operation)
        {
            // Escape
            if(
                (operation == ButtonOperation.Exit) && (State != ButtonState.Default)
            )
            {
                UpdateState(operation);
                return;
            }

            bool UpdateWithState(ButtonState state_, ButtonOperation operation_)
            {
                var flag = (State == state_ && operation == operation_);

                if (flag)
                {
                    UpdateState(operation);
                }

                return flag;
            }

            if (UpdateWithState(ButtonState.Default, ButtonOperation.Enter)) return;
            if (UpdateWithState(ButtonState.Hover, ButtonOperation.Push)) return;
            if (UpdateWithState(ButtonState.Hold, ButtonOperation.Release)) return;
        }
    }
}
