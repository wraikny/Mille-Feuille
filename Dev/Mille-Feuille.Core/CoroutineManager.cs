using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille
{
    public interface ICoroutineManager
    {
        void AddCoroutine(IEnumerator coroutine);
        void StackCoroutine(IEnumerator subcoroutine);
    }
    public sealed class CoroutineManager : ICoroutineManager
    {
        public CoroutineManager()
        {

        }

        private readonly HashSet<IEnumerator> registeredCoroutines = new HashSet<IEnumerator>();
        private readonly List<Stack<IEnumerator>> coroutines = new List<Stack<IEnumerator>>();
        private readonly Queue<IEnumerator> subcoroutines = new Queue<IEnumerator>();
        private bool enableSubCoroutine = false;

        /// <summary>
        /// コルーチンを更新する
        /// </summary>
        public void Update()
        {
            if(coroutines.Count > 0)
            {
                enableSubCoroutine = true;
                foreach (var coroutineStack in coroutines.ToArray())
                {
                    if (coroutineStack.Count > 0 && !(coroutineStack.Peek()?.MoveNext() ?? false))
                    {
                        registeredCoroutines.Remove(coroutineStack.Pop());
                    }

                    while(subcoroutines.Count > 0)
                    {
                        coroutineStack.Push(subcoroutines.Dequeue());
                    }

                    if (coroutineStack.Count == 0)
                    {
                        coroutines.Remove(coroutineStack);
                    }
                }
                enableSubCoroutine = false;
            }
        }

        /// <summary>
        /// 新しいコルーチンを追加する。
        /// </summary>
        /// <param name="coroutine"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        public void AddCoroutine(IEnumerator coroutine)
        {
            if (coroutine == null) throw new ArgumentNullException();
            if (registeredCoroutines.Contains(coroutine)) throw new ArgumentException("coroutine is already added");

            var stack = new Stack<IEnumerator>();
            stack.Push(coroutine);
            registeredCoroutines.Add(coroutine);
            coroutines.Add(stack);
        }

        /// <summary>
        /// サブコルーチンを現在のスタックに追加する。
        /// </summary>
        /// <param name="subcoroutine"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown when coroutine have been already added</exception>
        public void StackCoroutine(IEnumerator subcoroutine)
        {

            if (enableSubCoroutine)
            {
                if (subcoroutine == null) throw new ArgumentNullException();
                if (registeredCoroutines.Contains(subcoroutine)) throw new ArgumentException("coroutine is already added");
                subcoroutines.Enqueue(subcoroutine);
            }
            else
            {
                AddCoroutine(subcoroutine);
            }
        }
    }
}
