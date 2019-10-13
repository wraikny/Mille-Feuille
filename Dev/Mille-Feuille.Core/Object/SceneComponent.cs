using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille
{
    public class SceneComponent<T> : asd.SceneComponent
        where T : asd.Scene
    {
        public string Name { get; }

        public SceneComponent(string name)
        {
            Name = name;
        }

        public void Attach(T owner)
        {
            owner.AddComponent(this, Name);
        }

        /// <summary>
        /// このコンポーネントを持つシーンがエンジンに登録されたときに実行されるイベント。
        /// </summary>
        public event Action<T> OnRegisteredEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つシーンがエンジンから登録解除されたときに実行されるイベント。
        /// </summary>
        public event Action<T> OnUnRegisteredEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つシーンの更新が始まるときに実行されるイベント。
        /// </summary>
        public event Action<T> OnStartUpdatingEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つシーンの更新が止まるときに実行されるイベント。
        /// </summary>
        public event Action<T> OnStopUpdatingEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つシーンのUpdateが始まるときに実行されるイベント。
        /// </summary>
        public event Action<T> OnUpdatingEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つシーンのUpdateが終わるときに実行されるイベント。
        /// </summary>
        public event Action<T> OnUpdatedEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つシーンへの画面遷移が始まるときに実行されるイベント。
        /// </summary>
        public event Action<T> OnTransitionBeginEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つシーンへの画面遷移が完了したときに実行されるイベント。
        /// </summary>
        public event Action<T> OnTransitionFinishedEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つシーンが破棄されたときに実行されるイベント。
        /// </summary>
        public event Action<T> OnDisposedEvent = delegate { };

        protected override void OnSceneRegistered()
        {
            OnRegisteredEvent(Owner as T);
        }

        protected override void OnSceneUnregistered()
        {
            OnUnRegisteredEvent(Owner as T);
        }

        protected override void OnStartSceneUpdating()
        {
            OnStartUpdatingEvent(Owner as T);
        }

        protected override void OnStopSceneUpdating()
        {
            OnStopUpdatingEvent(Owner as T);
        }

        protected override void OnUpdating()
        {
            OnUpdatingEvent(Owner as T);
        }

        protected override void OnUpdated()
        {
            OnUpdatedEvent(Owner as T);
        }

        protected override void OnSceneTransitionBegin()
        {
            OnTransitionBeginEvent(Owner as T);
        }

        protected override void OnSceneTransitionFinished()
        {
            OnTransitionFinishedEvent(Owner as T);
        }

        protected override void OnSceneDisposed()
        {
            OnDisposedEvent(Owner as T);
        }
    }

    public static class SceneComponentExt
    {
        private const string ComponentName = "__MilleFeuilleSceneComponentExt";
        private const string CoroutineComponentName = "__MilleFeuilleSceneComponentExt_Coroutine";

        private static SceneComponent<asd.Scene> GetSceneComponent(this asd.Scene scene)
        {
            var component = (SceneComponent<asd.Scene>)scene.GetComponent(ComponentName);
            if (component == null)
            {
                component = new SceneComponent<asd.Scene>(ComponentName);
                component.Attach(scene);
            }

            return component;
        }

        /// <summary>
        /// シーンがエンジンに登録されたときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnRegisteredEventEvent(this asd.Scene scene, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            scene.GetSceneComponent().OnRegisteredEvent += _ => action();
        }

        /// <summary>
        /// シーンがエンジンから登録解除されたときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnUnRegisteredEventEvent(this asd.Scene scene, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            scene.GetSceneComponent().OnUnRegisteredEvent += _ => action();
        }

        /// <summary>
        /// シーンの更新が始まるときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnStartUpdatingEvent(this asd.Scene scene, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            scene.GetSceneComponent().OnStartUpdatingEvent += _ => action();
        }

        /// <summary>
        /// シーンの更新が止まるときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnStopUpdatingEvent(this asd.Scene scene, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            scene.GetSceneComponent().OnStopUpdatingEvent += _ => action();
        }

        /// <summary>
        /// シーンのUpdateが始まるときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnUpdatingEvent(this asd.Scene scene, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            scene.GetSceneComponent().OnUpdatingEvent += _ => action();
        }

        /// <summary>
        /// シーンのUpdateが終わるときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnUpdateEvent(this asd.Scene scene, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            scene.GetSceneComponent().OnUpdatedEvent += _ => action();
        }

        /// <summary>
        /// シーンへの画面遷移が始まるときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnTransitionBeginEvent(this asd.Scene scene, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            scene.GetSceneComponent().OnTransitionBeginEvent += _ => action();
        }

        /// <summary>
        /// シーンへの画面遷移が完了したときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnTransitionFinishedEvent(this asd.Scene scene, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            scene.GetSceneComponent().OnTransitionFinishedEvent += _ => action();
        }

        /// <summary>
        /// シーンが破棄されたときに実行されるイベントを追加する。
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOnDisposedEvent(this asd.Scene scene, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            scene.GetSceneComponent().OnDisposedEvent += _ => action();
        }

        sealed class CoroutineComponent : asd.SceneComponent
        {
            private readonly CoroutineUpdater coroutineManager = new CoroutineUpdater();
            public CoroutineManager Coroutine => coroutineManager;

            protected override void OnUpdated()
            {
                coroutineManager.Update();
            }
        }

        /// <summary>
        /// コルーチンを管理するクラスを取得する。
        /// </summary>
        public static CoroutineManager CoroutineManager(this asd.Scene scene)
        {
            var component = (CoroutineComponent)scene.GetComponent(CoroutineComponentName);
            if (component == null)
            {
                component = new CoroutineComponent();
                scene.AddComponent(component, CoroutineComponentName);
            }

            return component.Coroutine;
        }
    }
}
