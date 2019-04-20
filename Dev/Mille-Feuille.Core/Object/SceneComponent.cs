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

        public event Action<T> OnRegistered = delegate { };
        public event Action<T> OnUnRegistered = delegate { };

        public event Action<T> OnStartUpdating = delegate { };
        public event Action<T> OnStopUpdating = delegate { };

        public event Action<T> OnTransitionBegin = delegate { };
        public event Action<T> OnTransitionFinished = delegate { };

        public event Action<T> OnDisposed = delegate { };

        public event Action<T> OnSelfUpdating = delegate { };
        public event Action<T> OnSelfUpdated = delegate { };

        protected override void OnSceneRegistered()
        {
            base.OnSceneRegistered();
            OnRegistered((T)Owner);
        }

        protected override void OnSceneUnregistered()
        {
            base.OnSceneUnregistered();
            OnUnRegistered((T)Owner);
        }

        protected override void OnStartSceneUpdating()
        {
            base.OnStartSceneUpdating();
            OnStartUpdating((T)Owner);
        }

        protected override void OnStopSceneUpdating()
        {
            base.OnStopSceneUpdating();
            OnStopUpdating((T)Owner);
        }

        protected override void OnSceneTransitionBegin()
        {
            base.OnSceneTransitionBegin();
            OnTransitionBegin((T)Owner);
        }

        protected override void OnSceneTransitionFinished()
        {
            base.OnSceneTransitionFinished();
            OnTransitionFinished((T)Owner);
        }

        protected override void OnSceneDisposed()
        {
            base.OnSceneDisposed();
            OnDisposed((T)Owner);
        }

        protected override void OnUpdating()
        {
            base.OnUpdating();
            OnSelfUpdating((T)Owner);
        }

        protected override void OnUpdated()
        {
            base.OnUpdated();
            OnSelfUpdated((T)Owner);
        }
    }
}
