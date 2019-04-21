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

        /// <summary>
        /// このコンポーネントを持つオブジェクトがレイヤーに登録されたときのイベント。
        /// </summary>
        public event Action<T> OnAddedEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つオブジェクトからレイヤーに登録解除されたときのイベント。
        /// </summary>
        public event Action<T> OnRemovedEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つオブジェクトが更新されるときのイベント。
        /// </summary>
        public event Action<T> OnUpdateEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つオブジェクトが破棄されたときのイベント。
        /// </summary>
        public event Action<T> OnDisposedEvent = delegate { };

        protected override void OnObjectAdded()
        {
            base.OnObjectAdded();
            OnAddedEvent((T)Owner);
        }

        protected override void OnObjectRemoved()
        {
            base.OnObjectRemoved();
            OnRemovedEvent((T)Owner);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            OnUpdateEvent((T)Owner);
        }

        protected override void OnObjectDisposed()
        {
            base.OnObjectDisposed();
            OnDisposedEvent((T)Owner);
        }
    }
}
