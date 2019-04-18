using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Input.Controller
{
    public class KeyboardController<TControl> : IController<TControl>
    {
        private readonly Dictionary<TControl, asd.Keys> binding;

        public IEnumerable<TControl> Keys => binding.Keys;

        public KeyboardController()
        {
            binding = new Dictionary<TControl, asd.Keys>();
        }

        public asd.ButtonState? GetState(TControl key)
        {
            if (binding.TryGetValue(key, out var result))
            {
                return asd.Engine.Keyboard.GetKeyState(result);
            }
            else
            {
                return null;
            }
        }

        public void BindKey(TControl abstractKey, asd.Keys key)
        {
            binding[abstractKey] = key;
        }

        public void BindKeys(IReadOnlyCollection<(TControl, asd.Keys)> collection)
        {
            foreach (var (abstractKey, key) in collection)
            {
                BindKey(abstractKey, key);
            }
        }

        public void Update() { }
    }
}
