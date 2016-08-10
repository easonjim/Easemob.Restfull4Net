using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Script.Serialization;

namespace Easemob.Restfull4Net.Helper
{
    /*
     * 对象转json的自选字段helper
    */

    public class ObjectJsonItem<T>
    {
        public string DefaultKey { get; set; }
        public Expression<Func<T, object>> SetKey { get; set; }
        public object Value { get; set; }

        public string Key
        {
            get { return SetKey == null ? DefaultKey : ObjectJsonHelper.GetPropertyName<T>(SetKey); }
        }
    }

    public class ObjectJsonHelper
    {
        /// <summary>
        /// 创建字符串
        /// </summary>
        /// <typeparam name="T">转换的实体</typeparam>
        /// <param name="objectJsonItems">ObjectJsonItem</param>
        /// <returns>json字符串</returns>
        public static string CreateString<T>(params ObjectJsonItem<T>[] objectJsonItems)
        {
            if (objectJsonItems.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                var dic = objectJsonItems.ToDictionary(objectJsonItem => objectJsonItem.Key, objectJsonItem => objectJsonItem.Value);
                return new JavaScriptSerializer().Serialize(dic);
            }
        }

        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">转换的实体</typeparam>
        /// <typeparam name="T1">要序列化的类型</typeparam>
        /// <param name="objectJsonItems">ObjectJsonItem</param>
        /// <returns>json字符串</returns>
        public static T1 CreateDictionary<T,T1>(params ObjectJsonItem<T>[] objectJsonItems) where T1:IDictionary ,new ()
        {
            if (objectJsonItems.Length == 0)
            {
                return new T1();
            }
            else
            {
                T1 t1 = new T1();
                foreach (var objectJsonItem in objectJsonItems)
                {
                    t1.Add(objectJsonItem.Key,objectJsonItem.Value);
                }
                return t1;
            }
        }

        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> expr)
        {
            var rtn = "";
            if (expr.Body is UnaryExpression)
            {
                rtn = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            }
            else if (expr.Body is MemberExpression)
            {
                rtn = ((MemberExpression)expr.Body).Member.Name;
            }
            else if (expr.Body is ParameterExpression)
            {
                rtn = ((ParameterExpression)expr.Body).Type.Name;
            }
            return rtn;
        }


    }
}
