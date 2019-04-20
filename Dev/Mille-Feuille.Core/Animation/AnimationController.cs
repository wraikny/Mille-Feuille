using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Animation
{
    /// <summary>
    /// アニメーションコントローラーで遷移先と共にアニメーションを保持するクラス。
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    /// <typeparam name="TState"></typeparam>
    [Serializable]
    public class Node<TObj, TState>
        where TState : class
    {
        /// <summary>
        /// ノードが保持するアニメーションを取得または設定する。
        /// </summary>
        public Animation<TObj> Animation { get; set; }

        /// <summary>
        /// 完了後に遷移するアニメーションステートを取得または設定する。
        /// </summary>
        public TState NextState { get; set;  }

        public Node(Animation<TObj> anim)
        {
            Animation = anim;
            NextState = null;
        }
    }


    /// <summary>
    /// アニメーションとステートに応じた遷移関係を保持するクラス。
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    /// <typeparam name="TState"></typeparam>
    [Serializable]
    public class AnimationController<TObj, TState>
        where TState : class
    {
        public string Name { get; set; }

        /// <summary>
        /// アニメーションを保持するノードをアニメーションステートと紐つけて格納する辞書を取得する。
        /// </summary>
        public Dictionary<TState, Node<TObj, TState>> Nodes { get; }

        public AnimationController(
            string name
        )
        {
            Name = name;
            Nodes = new Dictionary<TState, Node<TObj, TState>>();
        }

        /// <summary>
        /// タプルのIEnumerableからアニメーションノードを追加するメソッド。
        /// </summary>
        /// <param name="pairs"></param>
        public void AddAnimations(IEnumerable<(TState, Node<TObj, TState>)> pairs)
        {
            foreach(var (state, node) in pairs)
            {
                AddAnimation(state, node);
            }
        }

        /// <summary>
        /// アニメーションノードを追加するメソッド。
        /// </summary>
        public AnimationController<TObj, TState> AddAnimation(TState state, Node<TObj, TState> node)
        {
            Nodes.Add(state, node);
            return this;
        }

        /// <summary>
        /// ステートを元にアニメーションノードを取得するメソッド。
        /// </summary>
        public Node<TObj, TState> GetNode(TState state)
        {
            Nodes.TryGetValue(state, out var result);
            return result;
        }
    }
}
