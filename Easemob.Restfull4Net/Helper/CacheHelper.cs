using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace Easemob.Restfull4Net.Helper
{
	/// <summary>
	/// 缓存相关的操作类
    /// Copyright (C)     
	/// </summary>
	public class CacheHelper
	{
        /// <summary>
        /// HttpRuntime.Cache对象
        /// </summary>
        private static readonly System.Web.Caching.Cache Cache = HttpRuntime.Cache;

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void Clear()
        {
            IDictionaryEnumerator enumerator = Cache.GetEnumerator();
            ArrayList list = new ArrayList();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Key);
            }
            foreach (string str in list)
            {
                Cache.Remove(str);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public static object Get(string key)
        {
            return Cache[key];
        }

        /// <summary>
        /// 插入缓存
        /// </summary>
        public static void Insert(string key, object obj)
        {
            Insert(key, obj, null, 1);
        }

        /// <summary>
        /// 插入缓存
        /// </summary>
        public static void Insert(string key, object obj, int seconds)
        {
            Insert(key, obj, null, seconds);
        }

        /// <summary>
        /// 插入缓存，依赖CacheDependency
        /// </summary>
        public static void Insert(string key, object obj, CacheDependency dep)
        {
            Insert(key, obj, dep, 0x21c0);
        }

        /// <summary>
        /// 插入缓存，设置优先级CacheItemPriority
        /// </summary>
        public static void Insert(string key, object obj, int seconds, CacheItemPriority priority)
        {
            Insert(key, obj, null, seconds, priority);
        }

        /// <summary>
        /// 插入缓存，依赖CacheDependency
        /// </summary>
        public static void Insert(string key, object obj, CacheDependency dep, int seconds)
        {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        /// <summary>
        /// 插入缓存，依赖CacheDependency，设置优先级CacheItemPriority
        /// </summary>
        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority)
        {
            if (obj != null)
            {
                Cache.Insert(key, obj, dep, DateTime.Now.AddSeconds(seconds), TimeSpan.Zero, priority, null);
            }
        }

        /// <summary>
        /// 插入缓存，时间最大
        /// </summary>
        public static void Max(string key, object obj)
        {
            Max(key, obj, null);
        }

        /// <summary>
        /// 插入缓存，时间最大，依赖CacheDependency
        /// </summary>
        public static void Max(string key, object obj, CacheDependency dep)
        {
            if (obj != null)
            {
                Cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
            }
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 清除缓存，指定正则
        /// </summary>
        public static void RemoveByPattern(string pattern)
        {
            IDictionaryEnumerator enumerator = Cache.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            while (enumerator.MoveNext())
            {
                if (regex.IsMatch(enumerator.Key.ToString()))
                {
                    Cache.Remove(enumerator.Key.ToString());
                }
            }
        }
    }
}
