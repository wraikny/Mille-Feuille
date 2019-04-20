using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Input.Controller
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
}
