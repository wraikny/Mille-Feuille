using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wraikny.MilleFeuille.Core.Object;

namespace wraikny.MilleFeuille.Core.Animation
{
    public class AnimatorComponent<T> : Object2DComponent<T>
        where T : asd.Object2D
    {
        public AnimationController<T> Controller { get; }

        
        public AnimatorComponent(string name, AnimationController<T> controller)
            : base(name)
        {
            Controller = controller;
        }
    }
}
