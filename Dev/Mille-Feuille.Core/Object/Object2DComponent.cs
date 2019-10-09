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
        private CoroutineManager coroutineManager;
        public ICoroutineManager Coroutine
        {
            get
            {
                if(coroutineManager == null)
                {
                    coroutineManager = new CoroutineManager();
                }
                return coroutineManager;
            }
        }

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

        protected override void OnObjectAdded()
        {
            InvokeAction(OnAddedEvent);
        }

        protected override void OnObjectRemoved()
        {
            InvokeAction(OnRemovedEvent);
        }

        protected override void OnUpdate()
        {
            InvokeAction(OnUpdateEvent);
            coroutineManager?.Update();
        }

        protected override void OnObjectDisposed()
        {
            InvokeAction(OnDisposedEvent);
        }
    }

    public static class Object2DComponentExt
    {
        private const string componentName = "__MilleFeuilleObject2DComponentExt";

        private static Object2DComponent<asd.Object2D> GetObject2DComponent(this asd.Object2D obj)
        {
            var component = (Object2DComponent<asd.Object2D>)obj.GetComponent(componentName);
            if(component == null)
            {
                component = new Object2DComponent<asd.Object2D>(componentName);
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

        /// <summary>
        /// 新しいコルーチンを追加する。
        /// </summary>
        /// <param name="coroutine"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddCoroutine(this asd.Object2D obj, IEnumerator coroutine)

        {
            if (coroutine == null) throw new ArgumentNullException("coroutine");
            obj.GetObject2DComponent().Coroutine.AddCoroutine(coroutine);
        }

        /// <summary>
        /// 新しいコルーチンを追加する。
        /// </summary>
        /// <param name="coroutine"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddCoroutine(this asd.Object2D obj, Func<IEnumerator> coroutine)
        {
            if (coroutine == null) throw new ArgumentNullException("coroutine");
            obj.AddCoroutine(coroutine());
        }

        /// <summary>
        /// サブコルーチンを現在のスタックに追加する。
        /// </summary>
        /// <param name="coroutine"></param>
        /// <exception cref="InvalidOperationException.InvalidOperationException">
        /// Thrown when called outside of current coroutines updating.
        /// </exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        public static void StackCoroutine(this asd.Object2D obj, IEnumerator coroutine)
        {
            if (coroutine == null) throw new ArgumentNullException("coroutine");
            obj.GetObject2DComponent().Coroutine.StackCoroutine(coroutine);
        }

        /// <summary>
        /// サブコルーチンを現在のスタックに追加する。
        /// </summary>
        /// <param name="coroutine"></param>
        /// <exception cref="InvalidOperationException.InvalidOperationException">
        /// Thrown when called outside of current coroutines updating.
        /// </exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        public static void StackCoroutine(this asd.Object2D obj, Func<IEnumerator> coroutine)
        {
            if (coroutine == null) throw new ArgumentNullException("coroutine");
            obj.GetObject2DComponent().Coroutine.StackCoroutine(coroutine());
        }
    }
}
