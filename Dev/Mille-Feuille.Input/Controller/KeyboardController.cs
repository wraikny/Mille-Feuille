using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Input.Controller
{
    public class KeyboardController<TControl> : ControllerBase<TControl>
    {
        private readonly Dictionary<TControl, asd.Keys> binding;

        public override IEnumerable<TControl> Keys => binding.Keys;

        public KeyboardController()
        {
            binding = new Dictionary<TControl, asd.Keys>();
        }

        public override asd.ButtonState? GetState(TControl key)
        {
            if (binding.TryGetValue(key, out asd.Keys result))
            {
                return asd.Engine.Keyboard.GetKeyState(result);
            }
            else
            {
                return null;
            }
        }

        public void BindKey(asd.Keys key, TControl abstractKey)
        {
            binding[abstractKey] = key;
        }

        public void BindKeys(IReadOnlyCollection<(asd.Keys, TControl)> collection)
        {
            foreach(var (key, abstractKey) in collection)
            {
                BindKey(key, abstractKey);
            }
        }
    }
}
