using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wraikny.MilleFeuille.Core.Object;
using wraikny.MilleFeuille.Core.UI;
using wraikny.MilleFeuille.Core.UI.Button;
using wraikny.MilleFeuille.Core.Input.Controller;


namespace wraikny.MilleFeuille.ExampleCs.Core.UI
{
    class ControllerButton : Scene
    {
        asd.Layer2D uiLayer;

        public ControllerButton()
        {
            uiLayer = new asd.Layer2D();
        }

        protected override void OnRegistered()
        {
            base.OnRegistered();

            AddLayer(uiLayer);

            uiLayer.AddObject(
                new asd.GeometryObject2D()
                {
                    Shape = new asd.RectangleShape()
                    {
                        DrawingArea = new asd.RectF(
                            new asd.Vector2DF(0.0f, 0.0f)
                            , asd.Engine.WindowSize.To2DF()
                        )
                    }
                    , Color = new asd.Color(0, 100, 150)
                }
            );

            var button1 = CreateButton(1, -100.0f, -100.0f);
            var button2 = CreateButton(2, -100.0f,  100.0f);
            var button3 = CreateButton(3,  100.0f,  100.0f);
            var button4 = CreateButton(4,  100.0f, -100.0f);

            button1
                .Chain(button2, ButtonDirection.Down)
                .Chain(button3, ButtonDirection.Right)
                .Chain(button4, ButtonDirection.Up)
                .Chain(button1, ButtonDirection.Left)
            ;

            uiLayer.AddObject(button1.GetComponent().Owner);
            uiLayer.AddObject(button2.GetComponent().Owner);
            uiLayer.AddObject(button3.GetComponent().Owner);
            uiLayer.AddObject(button4.GetComponent().Owner);

            var selecter = new ControllerButtonSelecter(button1);

            var keyboard = new KeyboardController<ControllerSelect>();
            keyboard
                .BindKey(ControllerSelect.Up, asd.Keys.Up)
                .BindKey(ControllerSelect.Down, asd.Keys.Down)
                .BindKey(ControllerSelect.Right, asd.Keys.Right)
                .BindKey(ControllerSelect.Left, asd.Keys.Left)
                .BindKey(ControllerSelect.Select, asd.Keys.Space)
                .BindKey(ControllerSelect.Cancel, asd.Keys.Escape)
            ;

            selecter.AddController(keyboard);

            uiLayer.AddComponent(selecter, "Selecter");
        }

        private static IControllerButton CreateButton(int index, float x, float y)
        {
            var defaultColor = new asd.Color(255, 255, 255);
            var hoverColor = new asd.Color(150, 150, 150);
            var holdColor = new asd.Color(50, 50, 50);

            var size = new asd.Vector2DF(150.0f, 150.0f);
            var buttonArea = new asd.RectF(-size / 2.0f, size);

            var buttonObj =
                new asd.GeometryObject2D()
                {
                    Shape = new asd.RectangleShape()
                    {
                        DrawingArea = buttonArea
                    }
                    ,
                    Color = defaultColor
                    ,
                    Position =
                        asd.Engine.WindowSize.To2DF() / 2.0f
                        + (new asd.Vector2DF(x, y))
                }
            ;

            var button =
                new ControllerButtonComponent
                    <asd.GeometryObject2D>("Button");

            //button.Default += owner => { };
            //button.Hover += owner => { };
            //button.Hold += owner => { };
            button.OnEntered += owner => {
                Console.WriteLine("Button{0}: OnEntered", index);
                owner.Color = hoverColor;
            };
            button.OnPushed += owner => {
                Console.WriteLine("Button{0}: OnPushed", index);
                owner.Color = holdColor;
            };
            button.OnReleased += owner => {
                Console.WriteLine("Button{0}: OnReleased", index);
                owner.Color = hoverColor;
            };
            button.OnExited += owner => {
                Console.WriteLine("Button{0}: OnExited", index);
                owner.Color = defaultColor;
            };

            buttonObj.AddComponent(button, button.Name);

            return button;
        }
    }
}
