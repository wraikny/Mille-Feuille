using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core
{
    /// <summary>
    /// コルーチンを生成する関数を保持するクラス。
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    [Serializable]
    public sealed class Animation<TObj>
        where TObj : class
    {
        public string Name { get; private set; }

        public readonly Func<TObj, IEnumerator> generator;

        /// <summary>
        /// アニメーション開始時のオブジェクトを参照してコルーチンを生成する
        /// </summary>
        public IEnumerator Generate(object owner)
        {
            if(owner is TObj owner_)
            {
                return generator(owner_);
            }
            else
            {
                throw new Exception("Type is mismatch");
            }
        }

        public Animation(string name, Func<TObj, IEnumerator> generator)
        {
            Name = name;
            this.generator = generator;
        }
    }
}
