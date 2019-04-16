using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilleFeuille.Core.UI.Button
{
    public class MouseButton
    {
        public asd.Object2D Owner { get; }

        public MouseButton(asd.Object2D owner, asd.Collider2D collider)
        {
            Owner = owner;
            Owner.AddCollider(collider);
        }

        private static string GetKeyStringFromMouseButton(asd.MouseButtons key)
        {
            return "__MilleFeuille_MouseButton_" + key.ToString();
        }

        public ButtonComponentBase GetButtonComponent(asd.MouseButtons key)
        {
            var key_ = GetKeyStringFromMouseButton(key);
            return (ButtonComponentBase)Owner.GetComponent(key_);
        }

        public void SetButtonComponent(asd.MouseButtons key, ButtonComponentBase component)
        {
            var key_ = GetKeyStringFromMouseButton(key);
            Owner.RemoveComponent(key_);
            Owner.AddComponent(component, key_);
        }

        public bool HasButtonComponent(asd.MouseButtons key)
        {
            return GetButtonComponent(key) != null;
        }

        public void UpdateButtonState(asd.MouseButtons key, asd.CollisionType collision, asd.ButtonState state)
        {
            var component = GetButtonComponent(key);
            if (component == null) return;

            if (
                component.State != ButtonState.Default &&
                collision == asd.CollisionType.Exit
                )
            {
                component.UpdateState(ButtonOperation.Exit);
                return;
            }

            switch (component.State)
            {
                case ButtonState.Default:
                    if (
                        collision != asd.CollisionType.Exit &&
                        state == asd.ButtonState.Free
                        )
                    {
                        component.UpdateState(ButtonOperation.Enter);
                    }
                    break;
                case ButtonState.Hover:
                    if (state == asd.ButtonState.Push)
                    {
                        component.UpdateState(ButtonOperation.Push);
                    }
                    break;
                case ButtonState.Hold:
                    if (state == asd.ButtonState.Release)
                    {
                        component.UpdateState(ButtonOperation.Release);
                    }
                    break;
            }
        }
    }
}
