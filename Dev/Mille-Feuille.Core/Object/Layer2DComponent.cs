using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Object
{
    public class Layer2DComponent<T> : asd.Layer2DComponent
        where T : asd.Layer2D
    {
        public string Name { get; }

        public Layer2DComponent(string name)
        {
            Name = name;
        }

        public event Action<T> OnOwnerAdded = delegate { };
        public event Action<T> OnOwnerUpdated = delegate { };
        public event Action<T> OnOwnerRemoved = delegate { };
        public event Action<T> OnComponentUpdate = delegate { };

        protected override void OnLayerAdded()
        {
            base.OnLayerAdded();
            OnOwnerAdded((T)Owner);
        }

        protected override void OnLayerUpdated()
        {
            base.OnLayerUpdated();

            OnOwnerUpdated((T)Owner);
        }

        protected override void OnLayerRemoved()
        {
            base.OnLayerRemoved();
            OnOwnerRemoved((T)Owner);
        }

        protected override void OnUpdating()
        {
            base.OnUpdating();
            OnComponentUpdate((T)Owner);
        }
    }
}
