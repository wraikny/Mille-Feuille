using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.UI
{
    public enum ButtonState
    {
        Default,
        Hover,
        Hold,
    }

    public abstract class ButtonComponentBase : asd.Object2DComponent
    {
        public ButtonState State { get; set;  }

        public ButtonComponentBase()
        {

        }

        public abstract void CallDefault();
        public abstract void CallOnEnter();
        public abstract void CallHover();
        public abstract void CallOnPushed();
        public abstract void CallHold();
        public abstract void CallOnSelected();
        public abstract void CallOnExit();
    }

    public class ButtonComponent<T> : ButtonComponentBase
        where T : asd.Object2D
    {

        public ButtonComponent()
            : base()
        {

        }

        public event Action<T> Default = delegate { };
        public override void CallDefault() => Default((T)Owner);

        public event Action<T> OnEnter = delegate { };
        public override void CallOnEnter() => OnEnter((T)Owner);

        public event Action<T> Hover = delegate { };
        public override void CallHover() => Hover((T)Owner);

        public event Action<T> OnPushed = delegate { };
        public override void CallOnPushed() => OnPushed((T)Owner);

        public event Action<T> Hold = delegate { };
        public override void CallHold() => Hold((T)Owner);

        public event Action<T> OnSelected = delegate { };
        public override void CallOnSelected() => OnSelected((T)Owner);

        public event Action<T> OnExit = delegate { };
        public override void CallOnExit() => OnExit((T)Owner);
    }
}
