using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wraikny.MilleFeuille.Core;

namespace wraikny.MilleFeuille.Core.UI.Button
{
    public enum ButtonState
    {
        Default,
        Hover,
        Hold,
    }

    public enum ButtonOperation
    {
        Enter,
        Push,
        Release,
        Exit,
    }

    public abstract class ButtonComponentBase : Object.Object2DComponent<asd.Object2D>
    {
        public ButtonState State { get; set; }

        public ButtonComponentBase()
        {
            State = ButtonState.Default;
        }

        protected abstract void CallDefault();
        protected abstract void CallHover();
        protected abstract void CallHold();

        protected abstract void CallOnEntered();
        protected abstract void CallOnPushed();
        protected abstract void CallOnSelected();
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
                    CallOnSelected();
                    State = ButtonState.Hover;
                    break;
                case ButtonOperation.Exit:
                    CallOnExited();
                    State = ButtonState.Default;
                    break;
            }
        }
    }

    public class ButtonComponent<T> : ButtonComponentBase
        where T : asd.Object2D
    {

        public ButtonComponent()
            : base()
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

        public event Action<T> OnSelected = delegate { };
        protected override void CallOnSelected() => OnSelected((T)Owner);

        public event Action<T> OnExited = delegate { };
        protected override void CallOnExited() => OnExited((T)Owner);
    }
}
