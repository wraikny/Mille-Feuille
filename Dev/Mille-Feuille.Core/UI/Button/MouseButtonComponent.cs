﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.UI.Button
{
    public interface IMouseButton
    {
        asd.Object2D ButtonOwner { get; }
        asd.MouseButtons TriggerButton { get; }
        void Update(asd.CollisionType collision, asd.ButtonState state);
    }

    public class MouseButtonComponent<T> : ButtonComponent<T>, IMouseButton
        where T : asd.Object2D
    {
        public asd.Object2D ButtonOwner { get; private set; }
        public asd.MouseButtons TriggerButton { get; }

        public MouseButtonComponent(asd.MouseButtons button)
        {
            TriggerButton = button;
        }

        protected override void OnObjectAdded()
        {
            base.OnObjectAdded();
            ButtonOwner = Owner;
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
