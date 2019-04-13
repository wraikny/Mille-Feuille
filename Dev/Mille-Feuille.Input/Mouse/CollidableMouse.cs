using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Input.Mouse
{
    class CollidableMouse : asd.GeometryObject2D
    {
        private float radius;
        private asd.CircleCollider collider;

        private asd.CameraObject2D camera = null;
        private asd.RectF area;

        public CollidableMouse(float radius, asd.CameraObject2D camera)
        {
            this.camera = camera;
            InitializeRadius(radius);
        }

        public CollidableMouse(float radius, asd.RectF area)
        {
            this.area = area;
            InitializeRadius(radius);
        }

        public CollidableMouse(float radius)
                : this(
                      radius,
                      new asd.RectF(
                          new asd.Vector2DF(0.0f, 0.0f),
                          asd.Engine.WindowSize.To2DF()))
        { }

        private void InitializeRadius(float radius)
        {
            this.radius = radius;
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
                        (pos - dst.Position) * src.Size / dst.Size
                        + src.Position
                ;
            }
        }

        public IEnumerable<T> GetCollidedObjects<T>()
            where T : asd.Object2D
        {
            return
                Collisions2DInfo
                .Where(x => x.SelfCollider.OwnerObject.Equals(this))
                .Select(x => x.TheirsCollider.OwnerObject)
                .Where(x => x.AbsoluteBeingDrawn)
                .OfType<T>();
        }
    }
}
