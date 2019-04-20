using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wraikny.MilleFeuille.Core.Input.Controller;

namespace wraikny.MilleFeuille.Core.UI.Button
{
    /// <summary>
    /// コントローラー操作可能なボタンに対する操作を表す列挙体。
    /// </summary>
    public enum ControllerSelect
    {
        Up,
        Down,
        Right,
        Left,
        Select,
        Cancel,
    }

    /// <summary>
    /// コントローラーボタンに対する操作を行うレイヤーコンポーネント。
    /// </summary>
    public class ControllerButtonSelecter : asd.Layer2DComponent
    {
        private readonly List<IController<ControllerSelect>> controllers;

        /// <summary>
        /// 操作するコントローラーのコレクションを取得する。
        /// </summary>
        public IEnumerable<IController<ControllerSelect>> Controllers => controllers;

        /// <summary>
        /// 現在カーソルがあるボタンを取得する。
        /// </summary>
        public IControllerButton CursorButton { get; private set; }

        public ControllerButtonSelecter(
            IControllerButton selectedButton
        )
        {
            controllers = new List<IController<ControllerSelect>>();
            CursorButton = selectedButton;
            selectedButton.Update(ButtonOperation.Enter);
        }

        protected override void OnLayerUpdated()
        {
            base.OnLayerUpdated();

            foreach(var controller in Controllers)
            {
                controller.Update();
            }

            UpdateButtonsState();
        }

        /// <summary>
        /// 操作を行うコントローラーを追加する。
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public ControllerButtonSelecter AddController(IController<ControllerSelect> controller)
        {
            controllers.Add(controller);
            return this;
        }

        /// <summary>
        /// コレクションから操作を行うコントローラーを追加する。
        /// </summary>
        /// <param name="controllers"></param>
        /// <returns></returns>
        public ControllerButtonSelecter AddControllers(IReadOnlyCollection<IController<ControllerSelect>> controllers)
        {
            this.controllers.AddRange(controllers);
            return this;
        }

        /// <summary>
        /// ボタンの方向から対応する操作を取得する。
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private ControllerSelect DirectionToControl(ButtonDirection dir)
        {
            switch(dir)
            {
                case ButtonDirection.Up:
                    return ControllerSelect.Up;
                case ButtonDirection.Down:
                    return ControllerSelect.Down;
                case ButtonDirection.Right:
                    return ControllerSelect.Right;
                case ButtonDirection.Left:
                    return ControllerSelect.Left;
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// コントローラーお操作を元にボタンの状態を更新する。
        /// </summary>
        private void UpdateButtonsState()
        {
            ButtonDirection[] dirs = {
                ButtonDirection.Up,
                ButtonDirection.Down,
                ButtonDirection.Right,
                ButtonDirection.Left,
            };

            foreach (var controller in Controllers)
            {
                // Escape
                if (controller.GetState(ControllerSelect.Cancel) != asd.ButtonState.Free)
                {
                    CursorButton.Update(ButtonOperation.Exit);
                    return;
                }

                if (controller.GetState(ControllerSelect.Select) == asd.ButtonState.Push)
                {
                    CursorButton.Update(ButtonOperation.Push);
                    return;
                }

                if (controller.GetState(ControllerSelect.Select) == asd.ButtonState.Release)
                {
                    CursorButton.Update(ButtonOperation.Release);
                    return;
                }

                foreach (var dir in dirs)
                {
                    var control = DirectionToControl(dir);
                    if (controller.GetState(control) == asd.ButtonState.Push)
                    {
                        var next = CursorButton.GetButton(dir);
                        if (next == null) continue;

                        CursorButton.Update(ButtonOperation.Exit);
                        next.Update(ButtonOperation.Enter);

                        CursorButton = next;

                        return;
                    }
                }

            }
        }
    }
}
