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
    /// <typeparam name="TObj"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public class AnimatorComponent<TObj, TState> : Object2DComponent<TObj>
        where TObj : asd.Object2D
        where TState : class
    {
        public AnimationController<TObj, TState> Controller { get; }

        private Node<TObj, TState> currentNode;

        private IEnumerator coroutine;

        private void UpdateNode(TState state)
        {
            currentNode = Controller.GetNode(state);
            coroutine = currentNode.Animation.Generator((TObj)Owner);
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
                    State = currentNode.NextState;
                }
            }
        }
    }
}
