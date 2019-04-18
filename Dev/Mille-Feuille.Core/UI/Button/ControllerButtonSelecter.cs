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

    public class ControllerButtonSelecter : asd.Layer2DComponent
    {
        private readonly List<ControllerBase<ControllerSelect>> controllers;
        public IEnumerable<ControllerBase<ControllerSelect>> Controllers => controllers;

        public IControllerButton CursorButton { get; private set; }

        public ControllerButtonSelecter(
            IControllerButton selectedButton
        )
        {
            controllers = new List<ControllerBase<ControllerSelect>>();
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

        public ControllerButtonSelecter AddController(ControllerBase<ControllerSelect> controller)
        {
            controllers.Add(controller);
            return this;
        }

        public ControllerButtonSelecter AddControllers(IReadOnlyCollection<ControllerBase<ControllerSelect>> controllers)
        {
            this.controllers.AddRange(controllers);
            return this;
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

            foreach(var controller in Controllers)
            {
                foreach (var dir in dirs )
                {
                    var control = DirectionToControl(dir);
                    if (controller.GetState(control) == asd.ButtonState.Push)
                    {
                        var next = CursorButton.GetButton(dir);
                        if (next == null) continue;

                        CursorButton.Update(ButtonOperation.Exit);
                        next.Update(ButtonOperation.Enter);

                        CursorButton = next;

                        break;
                    }
                }

                if(controller.GetState(ControllerSelect.Select) == asd.ButtonState.Push)
                {
                    CursorButton.Update(ButtonOperation.Push);
                }

                if (controller.GetState(ControllerSelect.Select) == asd.ButtonState.Release)
                {
                    CursorButton.Update(ButtonOperation.Release);
                }
            }
        }
    }
}
