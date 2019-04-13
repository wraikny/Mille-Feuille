using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.UI
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
            return "__MouseButton_" + key.ToString();
        }

        public ButtonComponentBase GetButtonComponent(asd.MouseButtons key)
        {
            var key_ = GetKeyStringFromMouseButton(key);
            return (ButtonComponentBase)Owner.GetComponent(key.ToString());
        }

        public void SetButtonComponent(asd.MouseButtons key, ButtonComponentBase component)
        {
            var key_ = GetKeyStringFromMouseButton(key);
            Owner.RemoveComponent(key_);
            Owner.AddComponent(component, key_);
        }

        public void Update(asd.MouseButtons key, asd.CollisionType collision, asd.ButtonState state)
        {
            var component = GetButtonComponent(key);
            if (component == null) return;

            if (
                component.State != ButtonState.Default &&
                collision == asd.CollisionType.Exit
                )
            {
                component.Update(ButtonOperation.Exit);
                return;
            }

            switch (component.State)
            {
                case ButtonState.Default:
                    if(
                        collision != asd.CollisionType.Exit &&
                        state == asd.ButtonState.Free
                        )
                    {
                        component.Update(ButtonOperation.Enter);
                    }
                    break;
                case ButtonState.Hover:
                    if(state == asd.ButtonState.Push)
                    {
                        component.Update(ButtonOperation.Push);
                    }
                    break;
                case ButtonState.Hold:
                    if (state == asd.ButtonState.Release)
                    {
                        component.Update(ButtonOperation.Release);
                    }
                    break;
            }
        }
    }
}
