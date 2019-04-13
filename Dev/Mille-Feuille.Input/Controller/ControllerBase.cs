using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Input.Controller
{
    abstract class ControllerBase<TControl>
    {
        public abstract IEnumerable<TControl> Keys { get; }

        public abstract asd.ButtonState? GetState(TControl key);

        public virtual void Update()
        {
        }
    }
}
