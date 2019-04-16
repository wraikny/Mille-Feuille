using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wraikny.MilleFeuille.Core.Input.Controller;

namespace wraikny.MilleFeuille.Core.UI.Button
{
    public enum ControllerControl
    {
        Up,
        Down,
        Right,
        Left,
        Select,
    }

    class ControllerButtonSelecter : asd.Layer2DComponent
    {
        public ControllerBase<ControllerControl> Controller { get; }

        public ControllerButton CursorButton { get; private set; }

        public ControllerButtonSelecter(
            ControllerBase<ControllerControl> controller
            , ControllerButton selectedButton
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

        private ControllerControl DirectionToControl(ButtonDirection dir)
        {
            switch(dir)
            {
                case ButtonDirection.Up:
                    return ControllerControl.Up;
                case ButtonDirection.Down:
                    return ControllerControl.Down;
                case ButtonDirection.Right:
                    return ControllerControl.Right;
                case ButtonDirection.Left:
                    return ControllerControl.Left;
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

                    CursorButton.UpdateButtonState(ButtonOperation.Exit);
                    next.UpdateButtonState(ButtonOperation.Enter);

                    CursorButton = next;

                    break;
                }
            }

            if(Controller.GetState(ControllerControl.Select) == asd.ButtonState.Push)
            {
                CursorButton.UpdateButtonState(ButtonOperation.Push);
            }

            if (Controller.GetState(ControllerControl.Select) == asd.ButtonState.Release)
            {
                CursorButton.UpdateButtonState(ButtonOperation.Push);
            }
        }
    }
}
