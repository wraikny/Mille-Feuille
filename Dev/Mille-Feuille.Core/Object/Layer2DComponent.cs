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
        private readonly CoroutineManager coroutineManager = new CoroutineManager();
        public ICoroutineManager Coroutine => coroutineManager;

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
            coroutineManager.Update();
        }
    }

    public static class Layer2DComponentExt
    {
        private const string componentName = "__MilleFeuilleLayer2DComponentExt";

        private static Layer2DComponent<asd.Layer2D> GetLayer2DComponent(this asd.Layer2D obj)
        {
            var component = (Layer2DComponent<asd.Layer2D>)obj.GetComponent(componentName);
            if (component == null)
            {
                component = new Layer2DComponent<asd.Layer2D>(componentName);
                component.Attach(obj);
            }

            return component;
        }

        /// <summary>
        /// レイヤーがシーンに登録されたときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnAddedEvent(this asd.Layer2D obj, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            obj.GetLayer2DComponent().OnAddedEvent += _ => action();
        }

        /// <summary>
        /// オブジェクトからレイヤーに登録解除されたときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnRemovedEvent(this asd.Layer2D obj, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            obj.GetLayer2DComponent().OnRemovedEvent += _ => action();
        }

        /// <summary>
        /// レイヤーが更新される直前に実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnUpdatingEvent(this asd.Layer2D obj, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            obj.GetLayer2DComponent().OnUpdatingEvent += _ => action();
        }

        /// <summary>
        /// レイヤーが更新される直後に実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnUpdateEvent(this asd.Layer2D obj, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            obj.GetLayer2DComponent().OnUpdatedEvent += _ => action();
        }

        /// <summary>
        /// 新しいコルーチンを追加する。
        /// </summary>
        /// <param name="coroutine"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddCoroutine(this asd.Layer2D obj, IEnumerator coroutine)

        {
            if (coroutine == null) throw new ArgumentNullException("coroutine");
            obj.GetLayer2DComponent().Coroutine.AddCoroutine(coroutine);
        }

        /// <summary>
        /// 新しいコルーチンを追加する。
        /// </summary>
        /// <param name="coroutine"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddCoroutine(this asd.Layer2D obj, Func<IEnumerator> coroutine)
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
        public static void StackCoroutine(this asd.Layer2D obj, IEnumerator coroutine)
        {
            if (coroutine == null) throw new ArgumentNullException("coroutine");
            obj.GetLayer2DComponent().Coroutine.StackCoroutine(coroutine);
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
        public static void StackCoroutine(this asd.Layer2D obj, Func<IEnumerator> coroutine)
        {
            if (coroutine == null) throw new ArgumentNullException("coroutine");
            obj.GetLayer2DComponent().Coroutine.StackCoroutine(coroutine());
        }
    }
}
