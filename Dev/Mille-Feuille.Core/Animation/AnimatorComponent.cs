using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core
{
    /// <summary>
    /// アニメーションをオブジェクトに適用するコンポーネント。
    /// </summary>
    /// <typeparam name="TOwner"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public sealed class AnimatorComponent<TOwner, TState> : Object2DComponent<TOwner>
        where TOwner : asd.Object2D
        where TState : class
    {
        public AnimationController<TState> Controller { get; }

        private IAnimationNode<TState> currentNode;

        private IEnumerator coroutine;

        public AnimatorComponent(string name, AnimationController<TState> controller)
            : base(name)
        {
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));
            currentNode = null;
            coroutine = null;
        }

        private void UpdateNode(TState state)
        {
            if (state == null) return;
            currentNode = Controller.GetNode(state);
            coroutine = currentNode.Generate((TOwner)Owner);
        }

        private TState state;

        /// <summary>
        /// 現在のアニメーションステートを取得または設定する。
        /// </summary>
        public TState State {
            get
            {
                return state;
            }
            private set
            {
                state = value;
                UpdateNode(state);
            }
        }

        public void Start(TState state)
        {
            State = state;
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
                    State = currentNode.NextState;
                }
            }
        }
    }
}
