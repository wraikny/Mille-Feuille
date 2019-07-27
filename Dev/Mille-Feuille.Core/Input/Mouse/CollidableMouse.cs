using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Input
{
    /// <summary>
    /// 衝突判定を持ったマウスを扱うクラス。
    /// </summary>
    public sealed class CollidableMouse : asd.GeometryObject2D
    {
        private readonly asd.CircleCollider collider;

        private readonly asd.CameraObject2D camera = null;
        private readonly asd.RectF area;

        /// <summary>
        /// コライダーを可視化する真偽を取得または追加する。
        /// </summary>
        public bool ColliderVisible { get; set; }

        public CollidableMouse(float radius, asd.CameraObject2D camera)
        {
            this.camera = camera;
            collider = new asd.CircleCollider() { Radius = radius };
        }

        public CollidableMouse(float radius, asd.RectF area)
        {
            this.area = area;
            collider = new asd.CircleCollider() { Radius = radius };
        }

        public CollidableMouse(float radius)
                : this(
                      radius,
                      new asd.RectF(
                          new asd.Vector2DF(0.0f, 0.0f),
                          asd.Engine.WindowSize.To2DF()))
        {
            collider = new asd.CircleCollider() { Radius = radius };
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            AddCollider(collider);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            CalcPositionFromMouse();

            if (ColliderVisible)
            {
                var inside = InsideArea();
                if (inside != collider.IsVisible)
                {
                    collider.IsVisible = inside;
                }
            }
        }

        /// <summary>
        /// マウスに登録された当たり判定が衝突した情報を取得する。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<asd.Collision2DInfo> GetCollisionInfo()
        {
            return
                InsideArea() ?
                    Collisions2DInfo
                    .Where(x => x.TheirsCollider.OwnerObject.AbsoluteBeingDrawn)
                    .Where(x => x.SelfCollider.OwnerObject.Equals(this))
                :
                    new List<asd.Collision2DInfo>()
            ;
        }

        /// <summary>
        /// マウスの位置を元にレイヤー上での位置を計算する。 
        /// </summary>
        private void CalcPositionFromMouse()
        {
            var pos = asd.Engine.Mouse.Position;

            if (camera == null)
            {
                Position = pos;
            }
            else
            {
                var src = camera.Src.ToF();
                var dst = camera.Dst.ToF();
                Position =
                        ((pos - dst.Position) * src.Size / dst.Size)
                        + src.Position
                ;
            }
        }

        /// <summary>
        /// マウスの位置が指定された範囲内にあるかどうかを取得する。
        /// </summary>
        /// <returns></returns>
        private bool InsideArea()
        {
            var area = (camera != null)
                ? camera.Dst.ToF()
                : this.area
                ;

            var areaRD = area.Position + area.Size;

            var pos = asd.Engine.Mouse.Position;

            return (
                (area.Position.X <= pos.X && pos.X <= areaRD.X) &&
                (area.Position.Y <= pos.Y && pos.Y <= areaRD.Y)
            );
        }
    }
}
