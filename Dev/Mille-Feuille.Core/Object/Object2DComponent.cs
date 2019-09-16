using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core
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
            UpdateCoroutine();
        }

        protected override void OnObjectDisposed()
        {
            InvokeAction(OnDisposedEvent);
        }

        private readonly HashSet<IEnumerator> registeredCoroutines = new HashSet<IEnumerator>();
        private readonly List<Stack<IEnumerator>> coroutines = new List<Stack<IEnumerator>>();
        private readonly Stack<IEnumerator> subcoroutines = new Stack<IEnumerator>();
        private bool enableSubCoroutine = false;

        /// <summary>
        /// コルーチンを更新する
        /// </summary>
        private void UpdateCoroutine()
        {
            enableSubCoroutine = true;
            foreach (var coroutineStack in coroutines.ToArray())
            {
                if (coroutineStack.Count > 0 && !(coroutineStack.Peek()?.MoveNext() ?? false))
                {
                    registeredCoroutines.Remove(coroutineStack.Pop());
                }

                foreach(var _ in Enumerable.Range(0, subcoroutines.Count))
                {
                    coroutineStack.Push(subcoroutines.Pop());
                }

                if (coroutineStack.Count == 0)
                {
                    coroutines.Remove(coroutineStack);
                }
            }
            enableSubCoroutine = false;
        }

        /// <summary>
        /// 新しいコルーチンを追加する。
        /// </summary>
        /// <param name="coroutine"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        public void AddCoroutine(IEnumerator coroutine)
        {
            if (coroutine == null) throw new ArgumentNullException();

            if (registeredCoroutines.Contains(coroutine)) throw new ArgumentException("coroutine is already added");
            
            var stack = new Stack<IEnumerator>();
            stack.Push(coroutine);
            registeredCoroutines.Add(coroutine);
            coroutines.Add(stack);
        }

        /// <summary>
        /// サブコルーチンを現在のスタックに追加する。
        /// </summary>
        /// <param name="subcoroutine"></param>
        /// <exception cref="InvalidOperationException.InvalidOperationException">
        /// Thrown when called outside of current coroutines updating.
        /// </exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        public void StartSubCoroutine(IEnumerator subcoroutine)
        {
            if (subcoroutine == null) throw new ArgumentNullException();

            if (registeredCoroutines.Contains(subcoroutine)) throw new ArgumentException("coroutine is already added");

            if (!enableSubCoroutine)
            {
                throw new InvalidOperationException("SubCorutine must be started inside of coroutine");
            }

            subcoroutines.Push(subcoroutine);
        }
    }

    public static class Object2DComponentExt
    {
        private const string componentName = "__MilleFeuilleObject2DComponent4Ext";

        private static Object2DComponent<T> GetObject2DComponent<T>(this T obj)
            where T : asd.Object2D
        {
            var component = (Object2DComponent<T>)obj.GetComponent(componentName);
            if(component == null)
            {
                component = new Object2DComponent<T>(componentName);
                component.Attach(obj);
            }

            return component;
        }

        public static void AddOnAddedEvent<T>(this T obj, Action action)
            where T : asd.Object2D
        {
            obj.GetObject2DComponent().OnAddedEvent += _ => action.Invoke();
        }

        public static void AddOnRemovedEvent<T>(this T obj, Action action)
            where T : asd.Object2D
        {
            obj.GetObject2DComponent().OnRemovedEvent += _ => action.Invoke();
        }

        public static void AddOnUpdateEvent<T>(this T obj, Action action)
            where T : asd.Object2D
        {
            obj.GetObject2DComponent().OnUpdateEvent += _ => action.Invoke();
        }

        public static void AddOnDisposedEvent<T>(this T obj, Action action)
            where T : asd.Object2D
        {
            obj.GetObject2DComponent().OnDisposedEvent += _ => action.Invoke();
        }

        /// <summary>
        /// 新しいコルーチンを追加する。
        /// </summary>
        /// <param name="coroutine"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        public static void AddCoroutine<T>(this T obj, IEnumerator coroutine)
            where T : asd.Object2D
        {
            obj.GetObject2DComponent().AddCoroutine(coroutine);
        }

        /// <summary>
        /// 新しいコルーチンを追加する。
        /// </summary>
        /// <param name="coroutine"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        public static void AddCoroutine<T>(this T obj, Func<IEnumerator> coroutine)
            where T : asd.Object2D
        {
            obj.AddCoroutine(coroutine.Invoke());
        }

        /// <summary>
        /// サブコルーチンを現在のスタックに追加する。
        /// </summary>
        /// <param name="subcoroutine"></param>
        /// <exception cref="InvalidOperationException.InvalidOperationException">
        /// Thrown when called outside of current coroutines updating.
        /// </exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        public static void StartSubCoroutine<T>(this T obj, IEnumerator subcoroutine)
            where T : asd.Object2D
        {
            obj.GetObject2DComponent().StartSubCoroutine(subcoroutine);
        }

        /// <summary>
        /// サブコルーチンを現在のスタックに追加する。
        /// </summary>
        /// <param name="subcoroutine"></param>
        /// <exception cref="InvalidOperationException.InvalidOperationException">
        /// Thrown when called outside of current coroutines updating.
        /// </exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        public static void StartSubCoroutine<T>(this T obj, Func<IEnumerator> subcoroutine)
            where T : asd.Object2D
        {
            obj.StartSubCoroutine(subcoroutine.Invoke());
        }
    }
}
