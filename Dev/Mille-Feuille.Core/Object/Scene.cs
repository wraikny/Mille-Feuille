using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Object
{
    public class Scene : asd.Scene
    {
        /// <summary>
        /// このシーンが他のシーンに遷移中かどうかを表す変数。
        /// </summary>
        private bool isSceneChanging = false;

        /// <summary>
        /// このシーンが一時停止しているかどうかの真偽値を取得する。
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// 一時停止時に更新を行うレイヤーのリストを取得する。
        /// </summary>
        public List<asd.Layer> PauseLayers { get; }

        /// <summary>
        /// 一時停止時に更新を行わないレイヤーのリストを取得する。
        /// </summary>
        public List<asd.Layer> NonPauseLayers { get; }

        public Scene()
        {
            IsPaused = false;
            PauseLayers = new List<asd.Layer>();
            NonPauseLayers = new List<asd.Layer>();
        }

        /// <summary>
        /// 元にシーンの一時停止を行う。
        /// </summary>
        /// <param name="paused">一時停止するかどうかを表す。trueなら停止する。</param>
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

        /// <summary>
        /// すぐにシーンの遷移を行う。
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 描画遷移効果ありでシーンの遷移を行う。
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="transition"></param>
        /// <param name="doAutoDispose"></param>
        /// <returns></returns>
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
