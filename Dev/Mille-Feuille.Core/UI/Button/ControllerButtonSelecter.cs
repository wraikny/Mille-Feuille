using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wraikny.MilleFeuille.Core.Input.Controller;

namespace wraikny.MilleFeuille.Core.UI.Button
{
    public enum ControllerSelect
    {
        Up,
        Down,
        Right,
        Left,
        Select,
    }

    class ControllerButtonSelecter : asd.Layer2DComponent
    {
        public ControllerBase<ControllerSelect> Controller { get; }

        public IControllerButton CursorButton { get; private set; }

        public ControllerButtonSelecter(
            ControllerBase<ControllerSelect> controller
            , IControllerButton selectedButton
        )
        {
            Controller = controller;
            CursorButton = selectedButton;
        }

        protected override void OnLayerUpdated()
        {
            base.OnLayerUpdated();

            Controller.Update();

            UpdateButtonsState();
        }

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

        private void UpdateButtonsState()
        {
            ButtonDirection[] dirs = {
                ButtonDirection.Up,
                ButtonDirection.Down,
                ButtonDirection.Right,
                ButtonDirection.Left,
            };

            foreach (var dir in dirs )
            {
                var control = DirectionToControl(dir);
                if (Controller.GetState(control) == asd.ButtonState.Push)
                {
                    var next = CursorButton.GetButton(dir);
                    if (next == null) continue;

                    CursorButton.Update(ButtonOperation.Exit);
                    next.Update(ButtonOperation.Enter);

                    CursorButton = next;

                    break;
                }
            }

            if(Controller.GetState(ControllerSelect.Select) == asd.ButtonState.Push)
            {
                CursorButton.Update(ButtonOperation.Push);
            }

            if (Controller.GetState(ControllerSelect.Select) == asd.ButtonState.Release)
            {
                CursorButton.Update(ButtonOperation.Push);
            }
        }
    }
}
