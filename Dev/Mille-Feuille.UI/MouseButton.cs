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

        public asd.Object2D Owner { get; }

        public MouseButton(asd.Object2D owner, asd.Collider2D collider)
        {
            Owner = owner;
            Owner.AddCollider(collider);
        }
    }
}
