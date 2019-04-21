using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.ExampleCs
{
    class Program
    {
        static void Main(string[] args)
        {
            asd.Engine.Initialize("Example Cs", 800, 600, new asd.EngineOption());

            var scene = new Core.UI.ControllerButton();

            asd.Engine.ChangeScene(scene);

            while (asd.Engine.DoEvents())
            {
                asd.Engine.Update();
            }

            asd.Engine.Terminate();
        }
    }
}
