using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille
{
    public class Object2DComponent<T> : asd.Object2DComponent
        where T : asd.Object2D
    {
        public string Name { get; }

        public Object2DComponent(string name)
        {
            Name = name;
        }


        public void Attach(T owner)
        {
            owner.AddComponent(this, Name);
        }

        /// <summary>
        /// このコンポーネントを持つオブジェクトがレイヤーに登録されたときに実行されるイベント。
        /// </summary>
        public event Action<T> OnAddedEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つオブジェクトからレイヤーに登録解除されたときに実行されるイベント。
        /// </summary>
        public event Action<T> OnRemovedEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つオブジェクトが更新されるときに実行されるイベント。
        /// </summary>
        public event Action<T> OnUpdateEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つオブジェクトが破棄されたときに実行されるイベント。
        /// </summary>
        public event Action<T> OnDisposedEvent = delegate { };

        protected override void OnObjectAdded()
        {
            OnAddedEvent(Owner as T);
        }

        protected override void OnObjectRemoved()
        {
            OnRemovedEvent(Owner as T);
        }

        protected override void OnUpdate()
        {
            OnUpdateEvent(Owner as T);
        }

        protected override void OnObjectDisposed()
        {
            OnDisposedEvent(Owner as T);
        }
    }

    public static class Object2DComponentExt
    {
        private const string ComponentName = "__MilleFeuilleObject2DComponentExt";
        private const string CoroutineComponentName = "__MilleFeuilleObject2DComponentExt_Coroutine";

        private static Object2DComponent<asd.Object2D> GetObject2DComponent(this asd.Object2D obj)
        {
            var component = (Object2DComponent<asd.Object2D>)obj.GetComponent(ComponentName);
            if(component == null)
            {
                component = new Object2DComponent<asd.Object2D>(ComponentName);
                component.Attach(obj);
            }

            return component;
        }

        /// <summary>
        /// オブジェクトがレイヤーに登録されたときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnAddedEvent(this asd.Object2D obj, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            obj.GetObject2DComponent().OnAddedEvent += _ => action();
        }

        /// <summary>
        /// オブジェクトからレイヤーに登録解除されたときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnRemovedEvent(this asd.Object2D obj, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            obj.GetObject2DComponent().OnRemovedEvent += _ => action();
        }

        /// <summary>
        /// オブジェクトが更新されるときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnUpdateEvent(this asd.Object2D obj, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            obj.GetObject2DComponent().OnUpdateEvent += _ => action();
        }

        /// <summary>
        /// オブジェクトが破棄されたときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnDisposedEvent(this asd.Object2D obj, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            obj.GetObject2DComponent().OnDisposedEvent += _ => action();
        }

        sealed class CoroutineComponent : asd.Object2DComponent
        {
            private readonly CoroutineUpdater coroutineManager = new CoroutineUpdater();
            public CoroutineManager Coroutine => coroutineManager;

            protected override void OnUpdate()
            {
                coroutineManager.Update();
            }
        }

        /// <summary>
        /// コルーチンを管理するクラスを取得する。
        /// </summary>
        public static CoroutineManager CoroutineManager(this asd.Object2D obj)
        {
            var component = (CoroutineComponent)obj.GetComponent(CoroutineComponentName);
            if (component == null)
            {
                component = new CoroutineComponent();
                obj.AddComponent(component, CoroutineComponentName);
            }

            return component.Coroutine;
        }
    }
}
