using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Animation
{
    [Serializable]
    public class Animation<TObj>
    {
        public string Name { get; private set; }
        public bool IsLooping { get; private set; }

        public Func<TObj, IEnumerator> Generator { get; private set; }

        public Animation(string name, Func<TObj, IEnumerator> generator)
        {
            Name = name;
            Generator = generator;

            IsLooping = false;
        }
    }
}
