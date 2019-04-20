using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mille_Feuille.Example.Cs
{
    class Program
    {
        static void Main(string[] args)
        {
            asd.Engine.Initialize("Example Cs", 800, 600, new asd.EngineOption());



            while(asd.Engine.DoEvents())
            {
                asd.Engine.Update();
            }

            asd.Engine.Terminate();
        }
    }
}
