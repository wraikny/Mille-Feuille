using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Animation
{
    [Serializable]
    public class AnimationController<TObj, TState>
    {
        public string Name { get; }

        private readonly Dictionary<TState, Animation<TObj>> animations;

        public AnimationController(
            string name
        )
        {
            Name = name;
            animations = new Dictionary<TState, Animation<TObj>>();
        }

        public AnimationController<TObj, TState> AddAnimation(TState state, Animation<TObj> anim)
        {
            animations.Add(state, anim);
            return this;
        }

        public Animation<TObj> GetAnimation(TState state)
        {
            animations.TryGetValue(state, out Animation<TObj> result);
            return result;
        }
    }
}
