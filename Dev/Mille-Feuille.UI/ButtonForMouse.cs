﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.UI
{
    class ButtonForMouse
    {
        protected const string leftkey = "LeftButton";
        protected const string rightkey = "RightButton";

        public asd.Object2D Owner { get; }

        public ButtonComponentBase Left
        {
            get => (ButtonComponentBase)Owner.GetComponent(leftkey);
            set
            {
                if (Left != null)
                {
                    Owner.RemoveComponent(leftkey);
                }
                Owner.AddComponent(value, leftkey);
            }
        }

        public ButtonComponentBase Right
        {
            get => (ButtonComponentBase)Owner.GetComponent(rightkey);
            set
            {
                if (Right != null)
                {
                    Owner.RemoveComponent(rightkey);
                }
                Owner.AddComponent(value, rightkey);
            }
        }

        public ButtonForMouse(asd.Object2D owner)
        {
            Owner = owner;
        }
    }
}
