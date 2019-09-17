using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wraikny.MilleFeuille;

namespace wraikny.MilleFeuille
{
    public static class ObjectExtension
    {
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

        /// <summary>
        /// 指定したMouseButtonSelecterをこのレイヤーに登録する。
        /// </summary>
        public static void AddMouseButtonSelecter(this asd.Layer2D obj, UI.MouseButtonSelecter selecter, string key)
        {
            obj.AddObject(selecter.Mouse);
            obj.AddComponent(selecter, key);
        }


    }
}
