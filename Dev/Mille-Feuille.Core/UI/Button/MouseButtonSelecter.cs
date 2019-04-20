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
        private readonly List<IMouseButton> buttons;

        public MouseButtonSelecter(CollidableMouse mouse)
        {
            this.Mouse = mouse;
            buttons = new List<IMouseButton>();
        }

        protected override void OnLayerUpdated()
        {
            base.OnLayerUpdated();

            UpdateButtonsState();
        }

        public MouseButtonSelecter AddButton(IMouseButton button)
        {
            buttons.Add(button);
            return this;
        }

        private void UpdateButtonsState()
        {
            var collisionsDict = GetCollisionsInfo();

            foreach (var button in buttons)
            {
                var key = button.TriggerButton;
                var state = asd.Engine.Mouse.GetButtonInputState(key);


                if(collisionsDict.TryGetValue(button, out var info))
                {
                    button.Update(info.CollisionType, state);
                }
                else
                {
                    button.Update(asd.CollisionType.Exit, state);
                }
            }
        }

        private IReadOnlyDictionary<IMouseButton, asd.Collision2DInfo> GetCollisionsInfo()
        {
            var result = new List<(IMouseButton, asd.Collision2DInfo)>();

            foreach (var info in Mouse.GetCollisionInfo())
            {
                var collidedObject = info.TheirsCollider.OwnerObject;

                var collidedButton = buttons
                    .FirstOrDefault(button =>
                        collidedObject.Equals(button.ButtOn)
                    );

                if (collidedButton != null)
                {
                    result.Add(
                        (collidedButton, info)
                    );
                }
            }

            return result.ToDictionary(t => t.Item1, t => t.Item2);
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