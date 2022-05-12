using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Farme
{
    /// <summary>
    /// 单例基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonBase<T> where T : class, new()
    {
        /// <summary>
        /// 实例
        /// </summary>
        private static T _instance = null;
        /// <summary>
        /// 防止因线程安全导致的实例未初始化就被引用
        /// (线程安全详情:一个对象的实例过程分为三步:1.开辟内存 2.建立内存引用联系 3.初始化
        /// 线程并发操作会导致对象未初始化就被拿取)
        /// </summary>
        private static readonly object _lock = new object();
        /// <summary>
        /// 是否存在单例实例
        /// </summary>
        public static bool Exists
        {
            get
            {
                return _instance != null;
            }
        }
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static T GetSingleton()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
        /// <summary>
        /// 清除单例
        /// </summary>
        public static void Clear()
        {
            _instance = null;
        }

    }
}
