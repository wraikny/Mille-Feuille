using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Object
{
    public class SceneComponent<T> : asd.SceneComponent
        where T : asd.Scene
    {
        public string Name { get; }

        public SceneComponent(string name)
        {
            Name = name;
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
            base.OnSceneRegistered();
            OnRegisteredEvent((T)Owner);
        }

        protected override void OnSceneUnregistered()
        {
            base.OnSceneUnregistered();
            OnUnRegisteredEvent((T)Owner);
        }

        protected override void OnStartSceneUpdating()
        {
            base.OnStartSceneUpdating();
            OnStartUpdatingEvent((T)Owner);
        }

        protected override void OnStopSceneUpdating()
        {
            base.OnStopSceneUpdating();
            OnStopUpdatingEvent((T)Owner);
        }

        protected override void OnUpdating()
        {
            base.OnUpdating();
            OnUpdatingEvent((T)Owner);
        }

        protected override void OnUpdated()
        {
            base.OnUpdated();
            OnUpdatedEvent((T)Owner);
        }

        protected override void OnSceneTransitionBegin()
        {
            base.OnSceneTransitionBegin();
            OnTransitionBeginEvent((T)Owner);
        }

        protected override void OnSceneTransitionFinished()
        {
            base.OnSceneTransitionFinished();
            OnTransitionFinishedEvent((T)Owner);
        }

        protected override void OnSceneDisposed()
        {
            base.OnSceneDisposed();
            OnDisposedEvent((T)Owner);
        }
    }
}
