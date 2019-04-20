using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Animation
{
    [Serializable]
    public class Node<TObj>
    {
        public Animation<TObj> Animation { get; set; }

        public Node<TObj> Next { get; set; }

        public Node(Animation<TObj> anim)
        {
            Animation = anim;
            Next = null;
        }
    }


    [Serializable]
    public class AnimationController<TObj, TState>
    {
        public string Name { get; set; }

        public Dictionary<TState, Node<TObj>> Nodes { get; }

        public AnimationController(
            string name
        )
        {
            Name = name;
            Nodes = new Dictionary<TState, Node<TObj>>();
        }

        public AnimationController<TObj, TState> AddAnimation(TState state, Node<TObj> node)
        {
            Nodes.Add(state, node);
            return this;
        }

        public Node<TObj> GetNode(TState state)
        {
            Nodes.TryGetValue(state, out var result);
            return result;
        }
    }
}
