using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Object
{
    public class Object2DComponent : asd.Object2DComponent
    {
        public event Action<asd.Object2D> OnOwnerAdded = delegate { };
        public event Action<asd.Object2D> OnOwnerRemoved = delegate { };
        public event Action<asd.Object2D> OnOwnerDisposed = delegate { };
        public event Action<asd.Object2D> OnComponentUpdate = delegate { };

        protected override void OnObjectAdded()
        {
            base.OnObjectAdded();
            OnOwnerAdded(Owner);
        }

        protected override void OnObjectRemoved()
        {
            base.OnObjectRemoved();
            OnOwnerRemoved(Owner);
        }

        protected override void OnObjectDisposed()
        {
            base.OnObjectDisposed();
            OnOwnerDisposed(Owner);
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
            OnComponentUpdate(Owner);
        }
    }
}
