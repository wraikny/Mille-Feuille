using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wraikny.MilleFeuille.Core;

namespace wraikny.MilleFeuille.Core.UI.Button
{
    /// <summary>
    /// ボタンの状態を表す列挙体。
    /// </summary>
    public enum ButtonState
    {
        Default,
        Hover,
        Hold,
    }

    /// <summary>
    /// ボタンに対する操作を表す列挙体。
    /// </summary>
    public enum ButtonOperation
    {
        Enter,
        Push,
        Release,
        Exit,
    }

    /// <summary>
    /// ボタン機能を提供するコンポーネントのクラス。
    /// </summary>
    public class ButtonComponent<T> : Object.Object2DComponent<asd.Object2D>
        where T : asd.Object2D
    {
        /// <summary>
        /// ボタンの状態を取得または設定する。
        /// </summary>
        public ButtonState State { get; set; }


        public ButtonComponent(string name)
            : base(name)
        {
            State = ButtonState.Default;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            switch (State)
            {
                case ButtonState.Default:
                    DefaultEvent((T)Owner);
                    break;
                case ButtonState.Hover:
                    HoverEvent((T)Owner);
                    break;
                case ButtonState.Hold:
                    HoldEvent((T)Owner);
                    break;
            }
        }

        /// <summary>
        /// ボタンに対する操作を元に状態を更新する。
        /// </summary>
        /// <param name="op"></param>
        public void UpdateState(ButtonOperation op)
        {
            switch (op)
            {
                case ButtonOperation.Enter:
                    OnEnteredEvent((T)Owner);
                    State = ButtonState.Hover;
                    break;
                case ButtonOperation.Push:
                    OnPushedEvent((T)Owner);
                    State = ButtonState.Hold;
                    break;
                case ButtonOperation.Release:
                    OnReleasedEvent((T)Owner);
                    State = ButtonState.Hover;
                    break;
                case ButtonOperation.Exit:
                    OnExitedEvent((T)Owner);
                    State = ButtonState.Default;
                    break;
            }
        }

        /// <summary>
        /// 選択ボタンが押されていない時に毎フレーム呼び出されるイベント。
        /// </summary>
        public event Action<T> DefaultEvent = delegate { };

        /// <summary>
        /// ホバー時に毎フレーム呼び出されるイベント。
        /// </summary>
        public event Action<T> HoverEvent = delegate { };

        /// <summary>
        /// 選択ボタンがホールド時に毎フレーム呼び出されるイベント。
        /// </summary>
        public event Action<T> HoldEvent = delegate { };


        /// <summary>
        /// フォーカスが入った時に呼び出されるイベント。
        /// </summary>
        public event Action<T> OnEnteredEvent = delegate { };

        /// <summary>
        /// 選択ボタンが押された時に呼び出されるイベント。
        /// </summary>
        public event Action<T> OnPushedEvent = delegate { };

        /// <summary>
        /// 選択ボタンが離された時に呼び出されるイベント。
        /// </summary>
        public event Action<T> OnReleasedEvent = delegate { };

        /// <summary>
        /// フォーカスが外れた時に呼び出されるイベント。
        /// </summary>
        public event Action<T> OnExitedEvent = delegate { };
    }
}
