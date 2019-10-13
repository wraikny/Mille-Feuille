using System;
using System.Collections;
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

        protected override void OnLayerAdded()
        {
            OnAddedEvent(Owner as T);
        }
        protected override void OnLayerRemoved()
        {
            OnRemovedEvent(Owner as T);
        }

        protected override void OnUpdating()
        {
            OnUpdatingEvent(Owner as T);
        }

        protected override void OnLayerUpdated()
        {
            OnUpdatedEvent(Owner as T);
        }
    }

    public static class Layer2DComponentExt
    {
        private const string ComponentName = "__MilleFeuilleLayer2DComponentExt";
        private const string CoroutineComponentName = "__MilleFeuilleLayer2DComponentExt_Coroutine";

        private static Layer2DComponent<asd.Layer2D> GetLayer2DComponent(this asd.Layer2D obj)
        {
            var component = (Layer2DComponent<asd.Layer2D>)obj.GetComponent(ComponentName);
            if (component == null)
            {
                component = new Layer2DComponent<asd.Layer2D>(ComponentName);
                component.Attach(obj);
            }

            return component;
        }

        /// <summary>
        /// レイヤーがシーンに登録されたときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnAddedEvent(this asd.Layer2D layer, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            layer.GetLayer2DComponent().OnAddedEvent += _ => action();
        }

        /// <summary>
        /// オブジェクトからレイヤーに登録解除されたときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnRemovedEvent(this asd.Layer2D layer, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            layer.GetLayer2DComponent().OnRemovedEvent += _ => action();
        }

        /// <summary>
        /// レイヤーが更新される直前に実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnUpdatingEvent(this asd.Layer2D layer, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            layer.GetLayer2DComponent().OnUpdatingEvent += _ => action();
        }

        /// <summary>
        /// レイヤーが更新される直後に実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnUpdateEvent(this asd.Layer2D layer, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            layer.GetLayer2DComponent().OnUpdatedEvent += _ => action();
        }

        sealed class CoroutineComponent : asd.Layer2DComponent
        {
            private readonly CoroutineUpdater coroutineManager = new CoroutineUpdater();
            public CoroutineManager Coroutine => coroutineManager;

            protected override void OnUpdating()
            {
                coroutineManager.Update();
            }
        }

        /// <summary>
        /// コルーチンを管理するクラスを取得する。
        /// </summary>
        public static CoroutineManager CoroutineManager(this asd.Layer2D layer)
        {
            var component = (CoroutineComponent)layer.GetComponent(CoroutineComponentName);
            if (component == null)
            {
                component = new CoroutineComponent();
                layer.AddComponent(component, CoroutineComponentName);
            }

            return component.Coroutine;
        }
    }
}
