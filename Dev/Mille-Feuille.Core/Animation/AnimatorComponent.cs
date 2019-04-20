using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wraikny.MilleFeuille.Core.Object;

namespace wraikny.MilleFeuille.Core.Animation
{
    public class AnimatorComponent<TObj, TState> : Object2DComponent<TObj>
        where TObj : asd.Object2D
    {
        public AnimationController<TObj, TState> Controller { get; }

        private IEnumerator coroutine;

        private TState state;
        public TState ControllerState {
            get
            {
                return state;
            }
            set
            {
                state = value;
                coroutine = Controller.GetAnimation(state)?.Generator((TObj)Owner);
            }
        }

        public AnimatorComponent(
            string name
            , AnimationController<TObj, TState> controller
        )
            : base(name)
        {
            Controller = controller;
            coroutine = Controller.GetAnimation(state)?.Generator((TObj)Owner);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            coroutine?.MoveNext();
        }
    }
}
