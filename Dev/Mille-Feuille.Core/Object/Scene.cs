using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core
{
    public class Scene : asd.Scene
    {
        /// <summary>
        /// このシーンが他のシーンに遷移中かどうかを取得する。
        /// </summary>
        public bool IsSceneChanging { get; private set; } = false;

        ///// <summary>
        ///// このシーンが一時停止しているかどうかを取得する。
        ///// </summary>
        //public bool IsPaused { get; private set; }

        ///// <summary>
        ///// 一時停止時に更新を行うレイヤーのリストを取得する。
        ///// </summary>
        //public List<asd.Layer> PauseLayers { get; }

        ///// <summary>
        ///// 一時停止時に更新を行わないレイヤーのリストを取得する。
        ///// </summary>
        //public List<asd.Layer> PausedLayers { get; }

        public Scene()
        {
            //IsPaused = false;
            //PauseLayers = new List<asd.Layer>();
            //PausedLayers = new List<asd.Layer>();
        }

        ///// <summary>
        ///// 元にシーンの一時停止を行う。
        ///// </summary>
        ///// <param name="paused">一時停止するかどうかを表す。trueなら停止する。</param>
        //public void Pause(bool paused)
        //{
        //    IsPaused = paused;

        //    PauseLayers.ForEach(layer =>
        //    {
        //        layer.IsUpdated = IsPaused;
        //        layer.IsDrawn = IsPaused;
        //    });

        //    PausedLayers.ForEach(layer =>
        //    {
        //        layer.IsUpdated = !IsPaused;
        //    });
        //}

        /// <summary>
        /// すぐにシーンの遷移を行う。
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public bool ChangeScene(asd.Scene scene)
        {
            var changeScene = !IsSceneChanging;

            if (changeScene)
            {
                IsSceneChanging = true;
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
            var changeScene = !IsSceneChanging;

            if (changeScene)
            {
                IsSceneChanging = true;
                asd.Engine.ChangeSceneWithTransition(scene, transition, doAutoDispose);
            }

            return changeScene;
        }
    }
}
