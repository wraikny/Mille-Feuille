using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wraikny.MilleFeuille.Core;

namespace wraikny.MilleFeuille.Core.UI.Button
{
    /// <summary>
    /// ボタンの状態を表す列挙体。
    /// </summary>
    public enum ButtonState
    {
        Default,
        Hover,
        Hold,
    }

    /// <summary>
    /// ボタンに対する操作を表す列挙体。
    /// </summary>
    public enum ButtonOperation
    {
        Enter,
        Push,
        Release,
        Exit,
    }

    /// <summary>
    /// ボタン機能を提供するコンポーネントの基底クラス。
    /// </summary>
    public abstract class ButtonComponentBase : Object.Object2DComponent<asd.Object2D>
    {
        /// <summary>
        /// ボタンの状態を取得または設定する。
        /// </summary>
        public ButtonState State { get; set; }

        public ButtonComponentBase(string name)
            : base(name)
        {
            State = ButtonState.Default;
        }


        protected abstract void CallDefault();
        protected abstract void CallHover();
        protected abstract void CallHold();

        protected abstract void CallOnEntered();
        protected abstract void CallOnPushed();
        protected abstract void CallOnReleased();
        protected abstract void CallOnExited();

        protected override void OnUpdate()
        {
            base.OnUpdate();

            switch (State)
            {
                case ButtonState.Default:
                    CallDefault();
                    break;
                case ButtonState.Hover:
                    CallHover();
                    break;
                case ButtonState.Hold:
                    CallHold();
                    break;
            }
        }

        /// <summary>
        /// ボタンに対する操作を元に状態を更新する。
        /// </summary>
        /// <param name="op"></param>
        public void UpdateState(ButtonOperation op)
        {
            switch (op)
            {
                case ButtonOperation.Enter:
                    CallOnEntered();
                    State = ButtonState.Hover;
                    break;
                case ButtonOperation.Push:
                    CallOnPushed();
                    State = ButtonState.Hold;
                    break;
                case ButtonOperation.Release:
                    CallOnReleased();
                    State = ButtonState.Hover;
                    break;
                case ButtonOperation.Exit:
                    CallOnExited();
                    State = ButtonState.Default;
                    break;
            }
        }
    }

    /// <summary>
    /// ボタン機能を提供するコンポーネントのクラス。
    /// </summary>
    public class ButtonComponent<T> : ButtonComponentBase
        where T : asd.Object2D
    {

        public ButtonComponent(string name)
            : base(name)
        {

        }

        public event Action<T> Default = delegate { };
        protected override void CallDefault() => Default((T)Owner);

        public event Action<T> OnEntered = delegate { };
        protected override void CallOnEntered() => OnEntered((T)Owner);

        public event Action<T> Hover = delegate { };
        protected override void CallHover() => Hover((T)Owner);

        public event Action<T> OnPushed = delegate { };
        protected override void CallOnPushed() => OnPushed((T)Owner);

        public event Action<T> Hold = delegate { };
        protected override void CallHold() => Hold((T)Owner);

        public event Action<T> OnReleased = delegate { };
        protected override void CallOnReleased() => OnReleased((T)Owner);

        public event Action<T> OnExited = delegate { };
        protected override void CallOnExited() => OnExited((T)Owner);
    }
}
