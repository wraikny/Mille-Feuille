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

        private Node<TObj> currentNode;
        private IEnumerator coroutine;
        private void UpdateNode(TState state)
        {
            currentNode = Controller.GetNode(state);
            coroutine = currentNode.Animation.Generator((TObj)Owner);
        }

        private TState state;
        public TState State {
            get
            {
                return state;
            }
            set
            {
                state = value;
                UpdateNode(state);
            }
        }

        public AnimatorComponent(
            string name
            , AnimationController<TObj, TState> controller
        )
            : base(name)
        {
            Controller = controller;
            currentNode = null;
            coroutine = null;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if(coroutine != null)
            {
                if(coroutine.MoveNext())
                {

                }
                else
                {
                    currentNode = currentNode.Next;
                }
            }
        }
    }
}
