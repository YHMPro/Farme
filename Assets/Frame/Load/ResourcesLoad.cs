﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Farme.Tool;
using System.Collections.Generic;

namespace Farme
{
    /// <summary>
    /// Resources加载
    /// </summary>
    public class ResourcesLoad
    {
        private static Dictionary<string, Object> m_Objects = new Dictionary<string, Object>();
        protected ResourcesLoad() { }
        #region//同步加载资源
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resPath">资源路径</param>
        /// <param name="result">结果</param>
        /// <param name="isLoadCache">是否缓存</param>
        /// <returns></returns>
        public static bool Load<T>(string resPath, out T result, bool isLoadCache = false) where T : Object
        {
            if (isLoadCache)
            {
                if (m_Objects.TryGetValue(resPath, out Object obj))
                {
                    result = (T)obj;
                }
                else
                {
                    result = Resources.Load<T>(resPath);
                    if (result == null)
                    {
                        return false;
                    }
                    m_Objects.Add(resPath, result);
                }
                return true;
            }
            else
            {
                result = Resources.Load<T>(resPath);
                if (result == null)
                {
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resPath">资源路径</param>
        /// <param name="isLoadCache">是否加载缓存</param>
        /// <returns>结果</returns>
        public static T Load<T>(string resPath, bool isLoadCache = false) where T : Object
        {
            T result = null;
            if (isLoadCache)
            {
                if (m_Objects.TryGetValue(resPath, out Object obj))
                {
                    result = (T)obj;
                }
                else
                {
                    result = Resources.Load<T>(resPath);
                    m_Objects.Add(resPath, result);
                }
            }
            else
            {
                if (m_Objects.ContainsKey(resPath))
                {
                    Debuger.LogWarning(resPath + "路径下的该对象之前已缓存,但却使用不通过缓存加载的方式进行加载。");
                }
                result = Resources.Load<T>(resPath);
            }
            return result;
        }
        #endregion
        #region//异步加载资源
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">资源路径</param>
        /// <param name="callBack">回调</param>
        /// <param name="isLoadCache">是否加载缓存</param>
        public static void LoadAsync<T>(string resPath, UnityAction<T> callBack, bool isLoadCache = false) where T : Object
        {
            if (isLoadCache)
            {
                if (m_Objects.TryGetValue(resPath, out Object obj))
                {
                    callBack?.Invoke((T)obj);
                    return;
                }
            }
            //开启协程进行资源加载
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IELoadAsync(resPath, callBack));
        }
        /// <summary>
        /// 协程启动异步加载Res资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">资源路径</param>
        /// <param name="callBack">回调</param>
        /// <param name="isLoadCache">是否加载缓存</param>
        private static IEnumerator IELoadAsync<T>(string resPath, UnityAction<T> callBack, bool isLoadCache = false) where T : Object
        {
            //异步加载资源
            ResourceRequest rr = Resources.LoadAsync<T>(resPath);
            yield return rr;//等待请求完成
            T assets = rr.asset as T;
            if (assets == null)
            {
                Debuger.LogError("[" + resPath + "]" + "路径下的资源加载失败。");
            }
            if (isLoadCache)
            {
                m_Objects.Add(resPath, assets);
            }
            callBack?.Invoke(assets);
        }
        #endregion
        /// <summary>
        /// 清除缓存(指定某路径下的资源)
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool ClearCache(string path)
        {
            if (m_Objects.TryGetValue(path, out Object obj))
            {
                Resources.UnloadAsset(obj);
                return m_Objects.Remove(path);
            }
            return false;
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void ClearAllCache()
        {
            foreach (var path in m_Objects.Keys)
            {
                if (m_Objects.TryGetValue(path, out Object obj))
                {
                    Resources.UnloadAsset(obj);
                }
            }
            m_Objects.Clear();
        }
    }
}
