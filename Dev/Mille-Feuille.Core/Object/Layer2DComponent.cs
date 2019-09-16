using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille
{
    public class Layer2DComponent<T> : asd.Layer2DComponent
        where T : asd.Layer2D
    {
        public string Name { get; }

        public Layer2DComponent(string name)
        {
            Name = name;
        }

        public void Attach(T owner)
        {
            owner.AddComponent(this, Name);
        }

        /// <summary>
        /// このコンポーネントを持つレイヤーがシーンに登録されたときに実行されるイベント。
        /// </summary>
        public event Action<T> OnAddedEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つレイヤーがシーンから登録解除される直前に実行されるイベント。
        /// </summary>
        public event Action<T> OnRemovedEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つレイヤーが更新される直前に実行されるイベント。
        /// </summary>
        public event Action<T> OnUpdatingEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つレイヤーが更新される直後に実行されるイベント。
        /// </summary>
        public event Action<T> OnUpdatedEvent = delegate { };

        private void InvokeAction(Action<T> action)
        {
            if (Owner is T obj)
            {
                action.Invoke(obj);
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        protected override void OnLayerAdded()
        {
            InvokeAction(OnAddedEvent);
        }
        protected override void OnLayerRemoved()
        {
            InvokeAction(OnRemovedEvent);
        }

        protected override void OnUpdating()
        {
            InvokeAction(OnUpdatingEvent);
        }

        protected override void OnLayerUpdated()
        {
            InvokeAction(OnUpdatedEvent);
        }
    }
}
