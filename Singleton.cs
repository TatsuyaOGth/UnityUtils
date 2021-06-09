using UnityEngine;

namespace Ogsn.Utils
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                }
                return _instance;
            }
        }
    }

    public class DontDestroySingletonMonoBehaviour<T> : SingletonMonoBehaviour<T> where T : SingletonMonoBehaviour<T>
    {
        public void Awake()
        {
            if (this != Instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(transform.root.gameObject);
        }
    }
}