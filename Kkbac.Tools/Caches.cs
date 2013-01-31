using System;
using System.Runtime.Caching;

namespace Kkbac.Tools
{
    public class Caches
    {
        /// <summary>
        /// 3600 * 24
        /// </summary>
        private long _maxSeconds = 3600 * 24;
        /// <summary>
        /// 过期时间秒 300
        /// </summary>
        const int expirationSeconds = 300;

        private MemoryCache _cache;

        public Caches(MemoryCache cache = null)
        {
            if (cache == null)
            {
                cache = MemoryCache.Default;
            }
            _cache = cache;
        }

        /// <summary>
        /// 获取计算机上缓存可使用的内存量（以兆字节为单位）。
        /// </summary>
        /// <returns></returns>
        public long GetMemoryLimit()
        {
            return _cache.CacheMemoryLimit;
        }

        /// <summary>
        /// 获取指定的缓存
        /// </summary>
        /// <param name="key">Cache的名称</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return _cache.Get(key);
        }

        /// <summary>
        /// 获取指定的缓存
        /// </summary>
        /// <param name="key">Cache的名称</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            T t = default(T);
            object o = Get(key);
            if (o != null && o is T)
            {
                t = (T)o;
            }
            return t;
        }

        /// <summary>
        /// 返回指定的缓存,如果不存在就用函数返回的值,自动加入缓存.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public T Get<T>(string key, Func<T> fun, long seconds = expirationSeconds)
        {
            T t;
            object o = Get(key);
            if (o != null && o is T)
            {
                t = (T)o;
            }
            else
            {
                t = fun();
                Set(key, t, seconds);
            }
            return t;
        }

        /// <summary>
        /// 删除指定的缓存
        /// </summary>
        /// <param name="key">Cache的名称</param>
        /// <returns> 如果在缓存中找到该项，则为已移除的缓存项；否则为 null。</returns>
        public object Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            var cache = _cache.Remove(key);
            return cache;
        }

        /// <summary>
        /// 删除指定的缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Caches operator -(Caches caches, string key)
        {
            caches.Remove(key);
            return caches;
        }

        /// <summary>
        /// 设置Cache的值,过期时间:到时间绝对过期. 小于等于0: 24小时过期;  时间秒. 默认:1800
        /// </summary>
        /// <param name="key">Cache的名称</param>
        /// <param name="value">Cache的值</param>
        /// <param name="seconds">过期时间,相对当前时间: 秒. 默认:1800</param> 
        public void Set(string key, object value, long seconds = expirationSeconds)
        {
            if (value == null || string.IsNullOrEmpty(key))
            {
                return;
            }
            if (seconds < 1)
            {
                seconds = _maxSeconds;
            }
            _cache.Set(key, value, DateTimeOffset.UtcNow.AddSeconds(seconds));
        }

    }

}
