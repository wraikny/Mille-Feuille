using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Input
{
    /// <summary>
    /// 入力と操作を対応付けるためのインターフェース。
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    public interface IController<TControl>
    {
        /// <summary>
        /// 入力に対応付けられている操作のコレクションを取得する。
        /// </summary>
        IEnumerable<TControl> Keys { get; }

        /// <summary>
        /// 指定した操作に対応する入力の状態を取得する。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        asd.ButtonState? GetState(TControl key);

        /// <summary>
        /// コントローラの状態を更新する。
        /// </summary>
        void Update();
    }

    public static class IControllerExt
    {
        public static bool StateIs<T>(this IController<T> controller, T key, asd.ButtonState state)
        {
            return (controller.GetState(key) == state);
        }

        public static bool IsFree<T>(this IController<T> controller, T key)
        {
            return controller.StateIs(key, asd.ButtonState.Free);
        }

        public static bool IsPush<T>(this IController<T> controller, T key)
        {
            return controller.StateIs(key, asd.ButtonState.Push);
        }

        public static bool IsHold<T>(this IController<T> controller, T key)
        {
            return controller.StateIs(key, asd.ButtonState.Hold);
        }

        public static bool IsRelease<T>(this IController<T> controller, T key)
        {
            return controller.StateIs(key, asd.ButtonState.Release);
        }
    }
}
