using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wraikny.MilleFeuille.Core.Object;

namespace wraikny.MilleFeuille.Core.Animation
{
    /// <summary>
    /// アニメーションをオブジェクトに適用するコンポーネント。
    /// </summary>
    /// <typeparam name="TOwner"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public class AnimatorComponent<TOwner, TState> : Object2DComponent<TOwner>
        where TOwner : asd.Object2D
        where TState : class
    {
        public AnimationController<TState> Controller { get; }

        private INode<TState> currentNode;

        private IEnumerator coroutine;

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
            set
            {
                state = value;
                UpdateNode(state);
            }
        }

        public AnimatorComponent(
            string name
            , AnimationController<TState> controller
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
                    State = currentNode.NextState;
                }
            }
        }
    }
}
