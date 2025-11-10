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

        public static Coroutine StartCoroutine1(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }
    }
}
