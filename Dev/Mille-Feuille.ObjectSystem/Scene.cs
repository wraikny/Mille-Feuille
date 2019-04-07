using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilleFeuille.ObjectSystem
{
    class Scene : asd.Scene
    {
        private bool isSceneChanging = false;

        protected bool ChangeScene(asd.Scene scene)
        {
            var changeScene = !isSceneChanging;

            if (changeScene)
            {
                isSceneChanging = true;
                asd.Engine.ChangeScene(scene);
            }

            return changeScene;
        }

        protected bool ChangeSceneWithTransition(asd.Scene scene, asd.Transition transition, bool doAutoDispose = true)
        {
            var changeScene = !isSceneChanging;

            if (changeScene)
            {
                isSceneChanging = true;
                asd.Engine.ChangeSceneWithTransition(scene, transition, doAutoDispose);
            }

            return changeScene;
        }
    }
}
