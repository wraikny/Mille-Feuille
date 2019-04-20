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

        public event Action<T> OnAdded = delegate { };
        public event Action<T> OnRemoved = delegate { };
        public event Action<T> OnDisposed = delegate { };
        public event Action<T> OnComponentUpdate = delegate { };

        protected override void OnObjectAdded()
        {
            base.OnObjectAdded();
            OnAdded((T)Owner);
        }

        protected override void OnObjectRemoved()
        {
            base.OnObjectRemoved();
            OnRemoved((T)Owner);
        }

        protected override void OnObjectDisposed()
        {
            base.OnObjectDisposed();
            OnDisposed((T)Owner);
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
            OnComponentUpdate((T)Owner);
        }
    }
}
