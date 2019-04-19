using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Animation
{
    [Serializable]
    public class AnimationNode<TObj, TState>
    {
        public Animation<TObj> Animation { get; }
        public string Name => Animation.Name;
        public List<AnimationNode<TObj, TState>> Nexts { get; }

        private Func<TState, AnimationNode<TObj, TState>> predicate;

        public AnimationNode(
            Animation<TObj> animation
            , Func<TState, AnimationNode<TObj, TState>> predicate
        )
        {
            Animation = animation;
            Nexts = new List<AnimationNode<TObj, TState>>();
            this.predicate = predicate;
        }

        public AnimationNode<TObj, TState> GetNext(TState state)
        {
            return predicate(state);
        }
    }

    [Serializable]
    public class AnimationController<TObj, TState>
    {
        public string Name { get; }

        private AnimationNode<TObj, TState> current;

        private readonly AnimationNode<TObj, TState> anyState;

        private readonly List<AnimationNode<TObj, TState>> nodes;

        public IReadOnlyList<AnimationNode<TObj, TState>> Nodes => nodes;

        public AnimationController(
            string name
            , AnimationNode<TObj, TState> anyState
        )
        {
            Name = name;
            current = null;
            nodes = new List<AnimationNode<TObj, TState>>();
            // nodes.Add(anyState);
            this.anyState = anyState;
        }


    }
}
