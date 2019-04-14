using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wraikny.MilleFeuille.Input.Mouse;

namespace wraikny.MilleFeuille.UI
{
    public class MouseButtonSelecter : asd.Layer2DComponent
    {
        private readonly CollidableMouse mouse;
        private readonly List<MouseButton> buttons;

        public MouseButtonSelecter(CollidableMouse mouse)
        {
            this.mouse = mouse;
            buttons = new List<MouseButton>();
        }

        protected override void OnLayerUpdated()
        {
            base.OnLayerUpdated();

            UpdateButtons();
        }

        public void AddButton(MouseButton button) => buttons.Add(button);

        private IEnumerable<(MouseButton, asd.Collision2DInfo)> GetCollidedButtons(asd.MouseButtons key)
        {
            var result = new List<(MouseButton, asd.Collision2DInfo)>();

            foreach(var info in mouse.GetCollisionInfo())
            {
                var collidedObject = info.TheirsCollider.OwnerObject;

                var collidedButton = buttons
                    .FirstOrDefault(button => collidedObject.Equals(button.Owner));

                if (collidedButton != null)
                {
                    result.Add(
                        (collidedButton, info)
                    );
                }
            }

            return result;
        }

        private void UpdateButtons()
        {
            foreach(var key in allMouseButtons)
            {
                var state = asd.Engine.Mouse.GetButtonInputState(key);
                foreach (var (button, info) in GetCollidedButtons(key))
                {
                    button.Update(key, info.CollisionType, state);
                }
            }
        }

        private static asd.MouseButtons[] allMouseButtons = {
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
