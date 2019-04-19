using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Object
{
    public class Object2DComponent<T> : asd.Object2DComponent
        where T : asd.Object2D
    {
        public string Name { get; }

        public Object2DComponent(string name)
        {
            Name = name;
        }

        public event Action<T> OnOwnerAdded = delegate { };
        public event Action<T> OnOwnerRemoved = delegate { };
        public event Action<T> OnOwnerDisposed = delegate { };
        public event Action<T> OnComponentUpdate = delegate { };

        protected override void OnObjectAdded()
        {
            base.OnObjectAdded();
            OnOwnerAdded((T)Owner);
        }

        protected override void OnObjectRemoved()
        {
            base.OnObjectRemoved();
            OnOwnerRemoved((T)Owner);
        }

        protected override void OnObjectDisposed()
        {
            base.OnObjectDisposed();
            OnOwnerDisposed((T)Owner);
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
            OnComponentUpdate((T)Owner);
        }
    }
}
