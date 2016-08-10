using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace Easemob.Restfull4Net.Helper
{
	/// <summary>
	/// ������صĲ�����
    /// Copyright (C)     
	/// </summary>
	public class CacheHelper
	{
        /// <summary>
        /// HttpRuntime.Cache����
        /// </summary>
        private static readonly System.Web.Caching.Cache Cache = HttpRuntime.Cache;

        /// <summary>
        /// ������л���
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
        /// ��ȡ����
        /// </summary>
        public static object Get(string key)
        {
            return Cache[key];
        }

        /// <summary>
        /// ���뻺��
        /// </summary>
        public static void Insert(string key, object obj)
        {
            Insert(key, obj, null, 1);
        }

        /// <summary>
        /// ���뻺��
        /// </summary>
        public static void Insert(string key, object obj, int seconds)
        {
            Insert(key, obj, null, seconds);
        }

        /// <summary>
        /// ���뻺�棬����CacheDependency
        /// </summary>
        public static void Insert(string key, object obj, CacheDependency dep)
        {
            Insert(key, obj, dep, 0x21c0);
        }

        /// <summary>
        /// ���뻺�棬�������ȼ�CacheItemPriority
        /// </summary>
        public static void Insert(string key, object obj, int seconds, CacheItemPriority priority)
        {
            Insert(key, obj, null, seconds, priority);
        }

        /// <summary>
        /// ���뻺�棬����CacheDependency
        /// </summary>
        public static void Insert(string key, object obj, CacheDependency dep, int seconds)
        {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        /// <summary>
        /// ���뻺�棬����CacheDependency���������ȼ�CacheItemPriority
        /// </summary>
        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority)
        {
            if (obj != null)
            {
                Cache.Insert(key, obj, dep, DateTime.Now.AddSeconds(seconds), TimeSpan.Zero, priority, null);
            }
        }

        /// <summary>
        /// ���뻺�棬ʱ�����
        /// </summary>
        public static void Max(string key, object obj)
        {
            Max(key, obj, null);
        }

        /// <summary>
        /// ���뻺�棬ʱ���������CacheDependency
        /// </summary>
        public static void Max(string key, object obj, CacheDependency dep)
        {
            if (obj != null)
            {
                Cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        public static void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// ������棬ָ������
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
