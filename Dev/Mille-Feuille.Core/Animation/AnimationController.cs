using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Animation
{
    public interface INode<TState>
        where TState : class
    {
        IEnumerator Generate(object owner);
        TState NextState { get; }
    }

    /// <summary>
    /// アニメーションコントローラーで遷移先と共にアニメーションを保持するクラス。
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    /// <typeparam name="TState"></typeparam>
    [Serializable]
    public class Node<TObj, TState> : INode<TState>
        where TState : class
        where TObj : class
    {
        /// <summary>
        /// ノードが保持するアニメーションを取得または設定する。
        /// </summary>
        public Animation<TObj> Animation { get; set; }

        /// <summary>
        /// 完了後に遷移するアニメーションステートを取得または設定する。
        /// </summary>
        public TState NextState { get; set;  }

        /// <summary>
        /// アニメーション開始時のオブジェクトを参照してコルーチンを生成する
        /// </summary>
        public IEnumerator Generate(object owner)
        {
            return Animation.Generate(owner);
        }

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
    public class AnimationController<TState>
        where TState : class
    {
        public string Name { get; set; }

        /// <summary>
        /// アニメーションを保持するノードをアニメーションステートと紐つけて格納する辞書を取得する。
        /// </summary>
        public Dictionary<TState, INode<TState>> Nodes { get; }

        public AnimationController(
            string name
        )
        {
            Name = name;
            Nodes = new Dictionary<TState, INode<TState>>();
        }

        /// <summary>
        /// タプルのIEnumerableからアニメーションノードを追加するメソッド。
        /// </summary>
        /// <param name="pairs"></param>
        public void AddAnimations(IEnumerable<(TState, INode<TState>)> pairs)
        {
            foreach(var (state, node) in pairs)
            {
                AddAnimation(state, node);
            }
        }

        /// <summary>
        /// アニメーションノードを追加するメソッド。
        /// </summary>
        public AnimationController<TState> AddAnimation(TState state, INode<TState> node)
        {
            Nodes[state] = node;
            return this;
        }

        /// <summary>
        /// ステートを元にアニメーションノードを取得するメソッド。
        /// </summary>
        public INode<TState> GetNode(TState state)
        {
            if (state == null) return null;

            Nodes.TryGetValue(state, out var result);
            return result;
        }
    }
}
