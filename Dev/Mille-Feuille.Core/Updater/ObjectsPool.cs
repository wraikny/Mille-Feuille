using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Updater
{
    public class ObjectsPool<T>
    {
        private readonly Stack<T> pool = new Stack<T>();
        private readonly Func<T> create;

        public ObjectsPool(Func<T> create, int count = 0)
        {
            if(create is null)
            {
                throw new ArgumentNullException("create");
            }

            this.create = create;
            
            for(int i = 0; i < count; i++)
            {
                pool.Push(create());
            }
        }

        public T Pop()
        {
            if (pool.Count == 0)
            {
                return create();
            }
            else
            {
                return pool.Pop();
            }
        }

        public void Push(T x) => pool.Push(x);

        public void Clear() => pool.Clear();
    }
}
