﻿using System;
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
    /// ボタン機能を提供するコンポーネントの基底クラス。
    /// </summary>
    public abstract class ButtonComponentBase : Object.Object2DComponent<asd.Object2D>
    {
        /// <summary>
        /// ボタンの状態を取得または設定する。
        /// </summary>
        public ButtonState State { get; set; }

        public ButtonComponentBase(string name)
            : base(name)
        {
            State = ButtonState.Default;
        }


        protected abstract void CallDefault();
        protected abstract void CallHover();
        protected abstract void CallHold();

        protected abstract void CallOnEntered();
        protected abstract void CallOnPushed();
        protected abstract void CallOnReleased();
        protected abstract void CallOnExited();

        protected override void OnUpdate()
        {
            base.OnUpdate();

            switch (State)
            {
                case ButtonState.Default:
                    CallDefault();
                    break;
                case ButtonState.Hover:
                    CallHover();
                    break;
                case ButtonState.Hold:
                    CallHold();
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
                    CallOnEntered();
                    State = ButtonState.Hover;
                    break;
                case ButtonOperation.Push:
                    CallOnPushed();
                    State = ButtonState.Hold;
                    break;
                case ButtonOperation.Release:
                    CallOnReleased();
                    State = ButtonState.Hover;
                    break;
                case ButtonOperation.Exit:
                    CallOnExited();
                    State = ButtonState.Default;
                    break;
            }
        }
    }

    /// <summary>
    /// ボタン機能を提供するコンポーネントのクラス。
    /// </summary>
    public class ButtonComponent<T> : ButtonComponentBase
        where T : asd.Object2D
    {

        public ButtonComponent(string name)
            : base(name)
        {

        }

        /// <summary>
        /// デフォルト時に毎フレーム呼び出されるイベント
        /// </summary>
        public event Action<T> DefaultEvent = delegate { };
        protected override void CallDefault() => DefaultEvent((T)Owner);

        /// <summary>
        /// フォーカスが入った時に呼び出されるイベント
        /// </summary>
        public event Action<T> OnEnteredEvent = delegate { };
        protected override void CallOnEntered() => OnEnteredEvent((T)Owner);

        public event Action<T> HoverEvent = delegate { };

        /// <summary>
        /// ホバー時に毎フレーム呼び出されるイベント
        /// </summary>
        protected override void CallHover() => HoverEvent((T)Owner);

        /// <summary>
        /// 選択ボタンが押された時に呼び出されるイベント
        /// </summary>
        public event Action<T> OnPushedEvent = delegate { };
        protected override void CallOnPushed() => OnPushedEvent((T)Owner);

        /// <summary>
        /// ホールド時に毎フレーム呼び出されるイベント
        /// </summary>
        public event Action<T> HoldEvent = delegate { };
        protected override void CallHold() => HoldEvent((T)Owner);

        /// <summary>
        /// 選択ボタンが話された時に呼び出されるイベント
        /// </summary>
        public event Action<T> OnReleasedEvent = delegate { };
        protected override void CallOnReleased() => OnReleasedEvent((T)Owner);

        /// <summary>
        /// フォーカスが外れた時に呼び出されるイベント
        /// </summary>
        public event Action<T> OnExitedEvent = delegate { };
        protected override void CallOnExited() => OnExitedEvent((T)Owner);
    }
}
