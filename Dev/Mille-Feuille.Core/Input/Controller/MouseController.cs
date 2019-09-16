using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Input.Controller
{
    /// <summary>
    /// マウスの入力と操作を対応付けるためのクラス。
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    public sealed class MouseController<TControl> : IController<TControl>
    {
        private readonly Dictionary<TControl, asd.MouseButtons> binding;

        /// <summary>
        /// 入力に対応付けられている操作のコレクションを取得する。
        /// </summary>
        public IEnumerable<TControl> Keys => binding.Keys;

        public MouseController()
        {
            binding = new Dictionary<TControl, asd.MouseButtons>();
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
                return asd.Engine.Mouse.GetButtonInputState(result);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ボタン入力に操作を対応付ける。
        /// </summary>
        /// <param name="abstractKey"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public MouseController<TControl> BindKey(TControl abstractKey, asd.MouseButtons button)
        {
            binding[abstractKey] = button;
            return this;
        }

        /// <summary>
        /// コレクションを元にボタン入力に操作を対応付ける。
        /// </summary>
        /// <param name="collection"></param>
        public void BindKeys(IReadOnlyCollection<(TControl, asd.MouseButtons)> collection)
        {
            foreach (var (abstractKey, button) in collection)
            {
                BindKey(abstractKey, button);
            }
        }

        public void Update() { }
    }
}
