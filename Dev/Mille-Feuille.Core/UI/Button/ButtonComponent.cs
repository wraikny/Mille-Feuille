using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public abstract class ButtonComponentBase : asd.Object2DComponent
    {
        public ButtonState State { get; set; }

        public ButtonComponentBase()
        {
            State = ButtonState.Default;
        }

        protected abstract void CallDefault();
        protected abstract void CallHover();
        protected abstract void CallHold();

        protected abstract void CallOnEnter();
        protected abstract void CallOnPushed();
        protected abstract void CallOnSelected();
        protected abstract void CallOnExit();

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
                    CallOnEnter();
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
                    CallOnEnter();
                    State = ButtonState.Hover;
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

        public event Action<T> OnEnter = delegate { };
        protected override void CallOnEnter() => OnEnter((T)Owner);

        public event Action<T> Hover = delegate { };
        protected override void CallHover() => Hover((T)Owner);

        public event Action<T> OnPushed = delegate { };
        protected override void CallOnPushed() => OnPushed((T)Owner);

        public event Action<T> Hold = delegate { };
        protected override void CallHold() => Hold((T)Owner);

        public event Action<T> OnSelected = delegate { };
        protected override void CallOnSelected() => OnSelected((T)Owner);

        public event Action<T> OnExit = delegate { };
        protected override void CallOnExit() => OnExit((T)Owner);
    }
}
