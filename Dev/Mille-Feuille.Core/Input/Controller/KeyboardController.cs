using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Input.Controller
{
    /// <summary>
    /// キーボードの入力と操作を対応付けるためのクラス。
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    public sealed class KeyboardController<TControl> : IController<TControl>
    {
        private readonly Dictionary<TControl, asd.Keys> binding;

        /// <summary>
        /// 入力に対応付けられている操作のコレクションを取得する。
        /// </summary>
        public IEnumerable<TControl> Keys => binding.Keys;

        public KeyboardController()
        {
            binding = new Dictionary<TControl, asd.Keys>();
        }

        /// <summary>
        /// 操作に対応する入力状態を取得する。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

        /// <summary>
        /// キー入力に操作を対応付ける。
        /// </summary>
        /// <param name="abstractKey"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public KeyboardController<TControl> BindKey(TControl abstractKey, asd.Keys key)
        {
            binding[abstractKey] = key;
            return this;
        }

        /// <summary>
        /// コレクションを元にキー入力に操作を対応付ける。
        /// </summary>
        /// <param name="collection"></param>
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
