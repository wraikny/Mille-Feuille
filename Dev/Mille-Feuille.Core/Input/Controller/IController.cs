using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Input.Controller
{
    public interface IController<TControl>
    {
        IEnumerable<TControl> Keys { get; }

        asd.ButtonState? GetState(TControl key);

        void Update();
    }
}
