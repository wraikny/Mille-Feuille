using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.UI
{
    class ButtonComponent<T> : asd.Object2DComponent
        where T : asd.Object2D
    {
        public event Action<T> Default = delegate { };
        public void CallDefault()
        {
            Default((T)this.Owner);
        }

        public event Action<T> OnEntered = delegate { };
        public void CallOnEntered()
        {
            OnEntered((T)this.Owner);
        }

        public event Action<T> Hover = delegate { };
        public void CallHover()
        {
            Hover((T)this.Owner);
        }

        public event Action<T> OnPushed = delegate { };
        public void CallOnPushed()
        {
            OnPushed((T)this.Owner);
        }

        public event Action<T> Hold = delegate { };
        public void CallHold()
        {
            Hold((T)this.Owner);
        }

        public event Action<T> OnSelected = delegate { };
        public void CallOnSelected()
        {
            OnSelected((T)this.Owner);
        }

        public event Action<T> OnExit = delegate { };
        public void CallOnExit()
        {
            OnExit((T)this.Owner);
        }
    }
}
