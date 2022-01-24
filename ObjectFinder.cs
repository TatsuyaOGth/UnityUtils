using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ogsn.Utils
{
    public class ObjectFinder : SingletonMonoBehaviour<ObjectFinder>
    {
        /// <summary>
        /// Object caches
        /// </summary>
        public List<Object> Caches = new List<Object>();

        /// <summary>
        /// This returns the Object that matches the specified type, but return the cache if this has that. It returns null if no Object matches the type.
        /// </summary>
        /// <param name="type">Type of object to find</param>
        /// <param name="includeInactive">Include inactive object</param>
        /// <returns>Object</returns>
        /// <see cref="FindObjectOfTypeOrCache{T}(bool)"/>
        public Object FindObjectOfTypeOrCache(System.Type type, bool includeInactive)
        {
            var cache = Caches.FirstOrDefault(o => o.GetType() == type);
            if (cache)
            {
                return cache;
            }

            var o = FindObjectOfType(type, includeInactive);
            if (o)
            {
                Caches.Add(o);
                return o;
            }

            Debug.LogWarning($"Couldn't find object of type {type}, will return null");
            return null;
        }

        /// <summary>
        /// This returns the Object that matches the specified type, but return the cache if this has that. It returns null if no Object matches the type.
        /// </summary>
        /// <typeparam name="T">Type of object to find</typeparam>
        /// <param name="includeInactive">Include inactive object</param>
        /// <returns>Object as specified type</returns>
        /// <see cref="FindObjectOfTypeOrCache(System.Type, bool)"/>
        public T FindObjectOfTypeOrCache<T>(bool includeInactive = false) where T : Object
        {
            return FindObjectOfTypeOrCache(typeof(T), includeInactive) as T;
        }

        /// <summary>
        /// Caches clear
        /// </summary>
        public void ClearCaches()
        {
            Caches.Clear();
        }

        /// <summary>
        /// Cache of specified type removal
        /// </summary>
        /// <typeparam name="T">Type of object to remove</typeparam>
        /// <returns>Is remove was succeeded</returns>
        public bool RemoveCache<T>()
        {
            return Caches.RemoveAll(o => o.GetType() == typeof(T)) > 0;
        }
    }

    public static class ObjectFinderExtentions
    {
        /// <summary>
        /// This returns the Object that matches the specified type, but return the cache if this has that. It returns null if no Object matches the type.
        /// </summary>
        /// <param name="type">Type of object to find</param>
        /// <param name="includeInactive">Include inactive</param>
        /// <returns>Object</returns>
        public static Object FindObjectOfTypeOrCache(this Object _, System.Type type, bool includeInactive = false)
        {
            return ObjectFinder.Instance.FindObjectOfTypeOrCache(type, includeInactive);
        }

        /// <summary>
        /// This returns the Object that matches the specified type, but return the cache if this has that. It returns null if no Object matches the type.
        /// </summary>
        /// <typeparam name="T">Type of object to find</typeparam>
        /// <param name="includeInactive">Include inactive</param>
        /// <returns>Object as specified type</returns>
        public static T FindObjectOfTypeOrCache<T>(this Object _, bool includeInactive = false)
            where T : Object
        {
            return ObjectFinder.Instance.FindObjectOfTypeOrCache<T>(includeInactive);
        }
    }
}
