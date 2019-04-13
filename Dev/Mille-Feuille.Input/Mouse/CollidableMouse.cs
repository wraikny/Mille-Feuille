﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Input.Mouse
{
    public class CollidableMouse : asd.GeometryObject2D
    {
        private readonly asd.CircleCollider collider;

        private readonly asd.CameraObject2D camera = null;
        private readonly asd.RectF area;

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
            SetPosition();
            if (ColliderVisible)
            {
                var inside = InsideArea();
                if(inside != collider.IsVisible)
                {
                    collider.IsVisible = inside;
                }
            }
        }

        private void SetPosition()
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

        public IEnumerable<T> GetCollidedObjects<T>()
            where T : asd.Object2D
        {
            return
                InsideArea()
                ?
                    Collisions2DInfo
                    .Where(x => x.SelfCollider.OwnerObject.Equals(this))
                    .Select(x => x.TheirsCollider.OwnerObject)
                    .Where(x => x.AbsoluteBeingDrawn)
                    .OfType<T>()
                :
                    null
            ;
        }
    }
}
