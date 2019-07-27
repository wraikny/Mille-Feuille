using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 指定したコンポネントをこのオブジェクトに追加する。
        /// </summary>
        public static void AddComponent<T, U>(this T obj, Object2DComponent<U> component)
            where T : asd.Object2D, U
            where U : asd.Object2D
        {
            obj.AddComponent(component, component.Name);
        }

        /// <summary>
        /// 指定したコンポネントをこのレイヤーに追加する。
        /// </summary>
        public static void AddComponent<T, U>(this T obj, Layer2DComponent<U> component)
            where T : asd.Layer2D, U
            where U : asd.Layer2D
        {
            obj.AddComponent(component, component.Name);
        }

        /// <summary>
        /// 指定したコンポネントをこのシーンに追加する。
        /// </summary>
        public static void AddComponent<T, U>(this T obj, SceneComponent<U> component)
            where T : asd.Scene, U
            where U : asd.Scene
        {
            obj.AddComponent(component, component.Name);
        }
    }
}
