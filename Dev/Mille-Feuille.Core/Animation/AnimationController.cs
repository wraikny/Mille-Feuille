using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Animation
{
    [Serializable]
    public class Node<TObj, TState>
        where TState : class
    {
        public Animation<TObj> Animation { get; set; }

        public TState NextState { get; set;  }

        public Node(Animation<TObj> anim)
        {
            Animation = anim;
            NextState = null;
        }
    }


    [Serializable]
    public class AnimationController<TObj, TState>
        where TState : class
    {
        public string Name { get; set; }

        public Dictionary<TState, Node<TObj, TState>> Nodes { get; }

        public AnimationController(
            string name
        )
        {
            Name = name;
            Nodes = new Dictionary<TState, Node<TObj, TState>>();
        }

        public void AddAnimations(IEnumerable<(TState, Node<TObj, TState>)> pairs)
        {
            foreach(var (state, node) in pairs)
            {
                AddAnimation(state, node);
            }
        }

        public AnimationController<TObj, TState> AddAnimation(TState state, Node<TObj, TState> node)
        {
            Nodes.Add(state, node);
            return this;
        }

        public Node<TObj, TState> GetNode(TState state)
        {
            Nodes.TryGetValue(state, out var result);
            return result;
        }
    }
}
