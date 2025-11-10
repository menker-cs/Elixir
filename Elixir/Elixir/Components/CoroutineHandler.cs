using System.Collections;
using UnityEngine;

namespace Elixir.Components
{
    public class CoroutineHandler : MonoBehaviour
    {
        private static CoroutineHandler _instance;

        public static CoroutineHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("CoroutineHandler");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<CoroutineHandler>();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Start a coroutine from a static context.
        /// </summary>
        public static Coroutine StartStaticCoroutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }
    }
}
