using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wraikny.MilleFeuille.Core.Object;

namespace wraikny.MilleFeuille.Core.Animation
{
    public class AnimatorComponent<TObj, TState> : Object2DComponent<TObj>
        where TObj : asd.Object2D
    {
        public AnimationController<TObj, TState> Controller { get; }

        TState State { get; set; }

        public AnimatorComponent(
            string name
            , AnimationController<TObj, TState> controller
        )
            : base(name)
        {
            Controller = controller;
        }
    }
}
