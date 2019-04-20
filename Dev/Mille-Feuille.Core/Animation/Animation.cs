using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Animation
{
    /// <summary>
    /// コルーチンを生成する関数を保持するクラス。
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    [Serializable]
    public class Animation<TObj>
    {
        public string Name { get; private set; }

        /// <summary>
        /// アニメーション開始時のオブジェクトを参照してコルーチンを生成する関数。
        /// </summary>
        public Func<TObj, IEnumerator> Generator { get; private set; }

        public Animation(string name, Func<TObj, IEnumerator> generator)
        {
            Name = name;
            Generator = generator;
        }
    }
}
