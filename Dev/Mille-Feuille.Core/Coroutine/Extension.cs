using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille
{
    public static class CoroutineManagerExt
    {
        private static IEnumerator GetExecuteWhileCoroutine(Func<bool> predicate, Action action, Action continueAction)
        {
            while(predicate())
            {
                action?.Invoke(); yield return null;
            }

            continueAction?.Invoke(); yield return null;
        }

        /// <summary>
        /// 条件を満たす間に処理を繰り返し実行するコルーチンを追加する
        /// </summary>
        /// <param name="action">条件を満たす間、毎フレーム実行する処理</param>
        /// <param name="continueAction">条件が終了したあとに一度だけ実行する処理</param>
        public static void ExecuteWhile(this CoroutineManager obj, Func<bool> predicate, Action action, Action continueAction = null)
        {
            obj.AddCoroutineAsParallel(GetExecuteWhileCoroutine(predicate, action, continueAction));
        }

        /// <summary>
        /// frame回処理を繰り返し実行するコルーチンを追加する
        /// </summary>
        /// <param name="frame">処理を繰り返し実行する回数</param>
        /// <param name="action">frame回実行される処理</param>
        /// <param name="continueAction">frame回経過後に一度だけ実行する処理</param>
        public static void ExecuteFrames(this CoroutineManager obj, int frame, Action<int> action, Action continueAction = null)
        {
            if (frame < 1) throw new ArgumentException("frame should be grater than 0");
            int i = 0;
            obj.ExecuteWhile(
                () => i < frame,
                () => { action?.Invoke(i); i++; },
                continueAction
            );
        }

        /// <summary>
        /// frame回待機後に一度だけ処理を実行するコルーチンを追加する
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="frame">待機するフレーム数</param>
        /// <param name="action">実行する処理</param>
        public static void SleepThen(this CoroutineManager obj, int frame, Action action)
        {
            if (frame < 1) throw new ArgumentException("frame should be grater than 0");
            obj.ExecuteFrames(frame, null, action);
        }
    }
}