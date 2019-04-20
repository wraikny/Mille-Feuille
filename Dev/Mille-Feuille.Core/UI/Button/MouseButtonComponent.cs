using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.UI.Button
{
    /// <summary>
    /// 親クラスによらずマウス操作可能なボタンを表すインターフェース。
    /// </summary>
    public interface IMouseButton
    {
        asd.Object2D Button { get; }
        asd.MouseButtons TriggerButton { get; }
        void Update(asd.CollisionType collision, asd.ButtonState state);
    }

    public class MouseButtonComponent<T> : ButtonComponent<T>, IMouseButton
        where T : asd.Object2D
    {
        public asd.Object2D Button { get; private set; }
        public asd.MouseButtons TriggerButton { get; }

        public MouseButtonComponent(string name, asd.MouseButtons button)
            : base(name)
        {
            TriggerButton = button;
        }

        protected override void OnObjectAdded()
        {
            base.OnObjectAdded();
            Button = Owner;
        }

        public void Update(asd.CollisionType collision, asd.ButtonState state)
        {
            if (
                State != ButtonState.Default &&
                collision == asd.CollisionType.Exit
                )
            {
                UpdateState(ButtonOperation.Exit);
                return;
            }

            switch (State)
            {
                case ButtonState.Default:
                    if (
                        collision != asd.CollisionType.Exit &&
                        state == asd.ButtonState.Free
                        )
                    {
                        UpdateState(ButtonOperation.Enter);
                    }
                    break;
                case ButtonState.Hover:
                    if (state == asd.ButtonState.Push)
                    {
                        UpdateState(ButtonOperation.Push);
                    }
                    break;
                case ButtonState.Hold:
                    if (state == asd.ButtonState.Release)
                    {
                        UpdateState(ButtonOperation.Release);
                    }
                    break;
            }
        }
    }
}
