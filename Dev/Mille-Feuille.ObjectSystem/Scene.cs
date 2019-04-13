using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.ObjectSystem
{
    class Scene : asd.Scene
    {
        private bool isSceneChanging = false;

        public bool IsPaused { get; private set; }
        public List<asd.Layer> PauseLayers { get; }
        public List<asd.Layer> NonPauseLayers { get; }

        public Scene()
        {
            IsPaused = false;
            PauseLayers = new List<asd.Layer>();
            NonPauseLayers = new List<asd.Layer>();
        }

        public void Pause(bool paused)
        {
            IsPaused = paused;

            PauseLayers.ForEach(layer =>
            {
                layer.IsUpdated = IsPaused;
                layer.IsDrawn = IsPaused;
            });

            NonPauseLayers.ForEach(layer =>
            {
                layer.IsUpdated = !IsPaused;
            });
        }

        public bool ChangeScene(asd.Scene scene)
        {
            var changeScene = !isSceneChanging;

            if (changeScene)
            {
                isSceneChanging = true;
                asd.Engine.ChangeScene(scene);
            }

            return changeScene;
        }

        public bool ChangeSceneWithTransition(asd.Scene scene, asd.Transition transition, bool doAutoDispose = true)
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
