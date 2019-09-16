﻿using System;
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
        private readonly CoroutineManager coroutineManager = new CoroutineManager();
        public ICoroutineManager Coroutine => coroutineManager;

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

        protected override void OnSceneRegistered()
        {
            InvokeAction(OnRegisteredEvent);
        }

        protected override void OnSceneUnregistered()
        {
            InvokeAction(OnUnRegisteredEvent);
        }

        protected override void OnStartSceneUpdating()
        {
            InvokeAction(OnStartUpdatingEvent);
        }

        protected override void OnStopSceneUpdating()
        {
            InvokeAction(OnStopUpdatingEvent);
        }

        protected override void OnUpdating()
        {
            InvokeAction(OnUpdatingEvent);
        }

        protected override void OnUpdated()
        {
            InvokeAction(OnUpdatedEvent);
            coroutineManager.Update();
        }

        protected override void OnSceneTransitionBegin()
        {
            InvokeAction(OnTransitionBeginEvent);
        }

        protected override void OnSceneTransitionFinished()
        {
            InvokeAction(OnTransitionFinishedEvent);
        }

        protected override void OnSceneDisposed()
        {
            InvokeAction(OnDisposedEvent);
        }
    }

    public static class SceneComponentExt
    {
        private const string componentName = "__MilleFeuilleSceneComponent4Ext";

        private static SceneComponent<asd.Scene> GetSceneComponent(this asd.Scene obj)
        {
            var component = (SceneComponent<asd.Scene>)obj.GetComponent(componentName);
            if (component == null)
            {
                component = new SceneComponent<asd.Scene>(componentName);
                component.Attach(obj);
            }

            return component;
        }

        /// <summary>
        /// シーンがエンジンに登録されたときに実行されるイベントを追加する。
        /// </summary>
        public static void AddOnRegisteredEventEvent(this asd.Scene obj, Action action)
        {
            obj.GetSceneComponent().OnRegisteredEvent += _ => action.Invoke();
        }

        /// <summary>
        /// シーンがエンジンから登録解除されたときに実行されるイベントを追加する。
        /// </summary>
        public static void AddOnUnRegisteredEventEvent(this asd.Scene obj, Action action)
        {
            obj.GetSceneComponent().OnUnRegisteredEvent += _ => action.Invoke();
        }

        /// <summary>
        /// シーンの更新が始まるときに実行されるイベントを追加する。
        /// </summary>
        public static void AddOnStartUpdatingEvent(this asd.Scene obj, Action action)
        {
            obj.GetSceneComponent().OnStartUpdatingEvent += _ => action.Invoke();
        }

        /// <summary>
        /// シーンの更新が止まるときに実行されるイベントを追加する。
        /// </summary>
        public static void AddOnStopUpdatingEvent(this asd.Scene obj, Action action)
        {
            obj.GetSceneComponent().OnStopUpdatingEvent += _ => action.Invoke();
        }

        /// <summary>
        /// シーンのUpdateが始まるときに実行されるイベントを追加する。
        /// </summary>
        public static void AddOnUpdatingEvent(this asd.Scene obj, Action action)
        {
            obj.GetSceneComponent().OnUpdatingEvent += _ => action.Invoke();
        }

        /// <summary>
        /// シーンのUpdateが終わるときに実行されるイベントを追加する。
        /// </summary>
        public static void AddOnUpdateEvent(this asd.Scene obj, Action action)
        {
            obj.GetSceneComponent().OnUpdatedEvent += _ => action.Invoke();
        }

        /// <summary>
        /// シーンへの画面遷移が始まるときに実行されるイベントを追加する。
        /// </summary>
        public static void AddOnTransitionBeginEvent(this asd.Scene obj, Action action)
        {
            obj.GetSceneComponent().OnTransitionBeginEvent += _ => action.Invoke();
        }

        /// <summary>
        /// シーンへの画面遷移が完了したときに実行されるイベントを追加する。
        /// </summary>
        public static void AddOnTransitionFinishedEvent(this asd.Scene obj, Action action)
        {
            obj.GetSceneComponent().OnTransitionFinishedEvent += _ => action.Invoke();
        }

        /// <summary>
        /// シーンが破棄されたときに実行されるイベントを追加する。
        /// </summary>
        public static void AddOnDisposedEvent(this asd.Scene obj, Action action)
        {
            obj.GetSceneComponent().OnDisposedEvent += _ => action.Invoke();
        }

        /// <summary>
        /// 新しいコルーチンを追加する。
        /// </summary>
        /// <param name="coroutine"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        public static void AddCoroutine(this asd.Scene obj, IEnumerator coroutine)

        {
            obj.GetSceneComponent().Coroutine.AddCoroutine(coroutine);
        }

        /// <summary>
        /// 新しいコルーチンを追加する。
        /// </summary>
        /// <param name="coroutine"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        public static void AddCoroutine(this asd.Scene obj, Func<IEnumerator> coroutine)
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
        public static void StartSubCoroutine(this asd.Scene obj, IEnumerator subcoroutine)
        {
            obj.GetSceneComponent().Coroutine.StartSubCoroutine(subcoroutine);
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
        public static void StartSubCoroutine(this asd.Scene obj, Func<IEnumerator> subcoroutine)
        {
            obj.StartSubCoroutine(subcoroutine.Invoke());
        }
    }
}
