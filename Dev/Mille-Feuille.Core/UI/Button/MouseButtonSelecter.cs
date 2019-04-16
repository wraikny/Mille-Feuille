using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wraikny.MilleFeuille.Core.Input.Mouse;

namespace wraikny.MilleFeuille.Core.UI.Button
{
    public class MouseButtonSelecter : asd.Layer2DComponent
    {
        public CollidableMouse Mouse { get; }
        public List<MouseButton> Buttons { get; }

        public MouseButtonSelecter(CollidableMouse mouse)
        {
            this.Mouse = mouse;
            Buttons = new List<MouseButton>();
        }

        protected override void OnLayerUpdated()
        {
            base.OnLayerUpdated();

            UpdateButtonsState();
        }

        public void AddButton(MouseButton button) => Buttons.Add(button);

        private void UpdateButtonsState()
        {
            foreach (var key in allMouseButtons)
            {
                var state = asd.Engine.Mouse.GetButtonInputState(key);
                foreach (var (button, info) in GetCollidedButtons())
                {
                    button.UpdateButtonState(key, info.CollisionType, state);
                }
            }
        }

        private IEnumerable<(MouseButton, asd.Collision2DInfo)> GetCollidedButtons()
        {
            var result = new List<(MouseButton, asd.Collision2DInfo)>();

            foreach (var info in Mouse.GetCollisionInfo())
            {
                var collidedObject = info.TheirsCollider.OwnerObject;

                var collidedButton = Buttons
                    .FirstOrDefault(button =>
                        collidedObject.Equals(button.Owner)
                    );

                if (collidedButton != null)
                {
                    result.Add(
                        (collidedButton, info)
                    );
                }
            }

            return result;
        }

        private static readonly asd.MouseButtons[] allMouseButtons = {
            asd.MouseButtons.ButtonLeft,
            asd.MouseButtons.ButtonRight,
            asd.MouseButtons.ButtonMiddle,
            asd.MouseButtons.SubButton1,
            asd.MouseButtons.SubButton2,
            asd.MouseButtons.SubButton3,
            asd.MouseButtons.SubButton4,
            asd.MouseButtons.SubButton5
        };
    }
}