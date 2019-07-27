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

        /// <summary>
        /// 描画に関する全ての同期設定を有効にして、指定した2Dオブジェクトを子オブジェクトとしてこのインスタンスに登録する。
        /// </summary>
        public static void AddDrawnChildWithAll(this asd.DrawnObject2D obj, asd.DrawnObject2D child)
        {
            obj.AddDrawnChild(
                child,
                asd.ChildManagementMode.RegistrationToLayer |
                asd.ChildManagementMode.Disposal |
                asd.ChildManagementMode.IsUpdated |
                asd.ChildManagementMode.IsDrawn,
                asd.ChildTransformingMode.All,
                asd.ChildDrawingMode.Color |
                asd.ChildDrawingMode.DrawingPriority
            );
        }

        /// <summary>
        /// 描画に関する色以外の全ての同期設定を有効にして、指定した2Dオブジェクトを子オブジェクトとしてこのインスタンスに登録する。
        /// </summary>
        public static void AddDrawnChildWithoutColor(this asd.DrawnObject2D obj, asd.DrawnObject2D child)
        {
            obj.AddDrawnChild(
                child,
                asd.ChildManagementMode.RegistrationToLayer |
                asd.ChildManagementMode.Disposal |
                asd.ChildManagementMode.IsUpdated |
                asd.ChildManagementMode.IsDrawn,
                asd.ChildTransformingMode.All,
                asd.ChildDrawingMode.DrawingPriority
            );
        }

        /// <summary>
        /// 描画テキストの横方向の描画領域を計算する。
        /// </summary>
        public static asd.Vector2DI HorizontalSize(this asd.Font obj, string text)
        {
            return obj.CalcTextureSize(text, asd.WritingDirection.Horizontal);
        }

        /// <summary>
        /// 描画テキストの縦方向の描画領域を計算する。
        /// </summary>
        public static asd.Vector2DI VerticalSize(this asd.Font obj, string text)
        {
            return obj.CalcTextureSize(text, asd.WritingDirection.Vertical);
        }
    }
}
