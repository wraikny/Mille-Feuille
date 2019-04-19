using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Animation
{


    [Serializable]
    public class AnimationController<T>
    {
        private readonly List<Animation<T>> animations;

        public string Name { get; }
        public IReadOnlyList<Animation<T>> Animations => animations;

        public AnimationController(string name)
        {
            Name = name;

            animations = new List<Animation<T>>();
        }
    }
}
