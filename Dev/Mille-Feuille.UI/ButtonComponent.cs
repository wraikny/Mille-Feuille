using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.UI
{
    public abstract class ButtonComponentBase : asd.Object2DComponent
    {
        public abstract void CallDefault();
        public abstract void CallOnEntered();
        public abstract void CallHover();
        public abstract void CallOnPushed();
        public abstract void CallHold();
        public abstract void CallOnSelected();
        public abstract void CallOnExit();
    }

    public class ButtonComponent<T> : ButtonComponentBase
        where T : asd.Object2D
    {
        public event Action<T> Default = delegate { };
        public override void CallDefault() => Default((T)Owner);

        public event Action<T> OnEntered = delegate { };
        public override void CallOnEntered() => OnEntered((T)Owner);

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
