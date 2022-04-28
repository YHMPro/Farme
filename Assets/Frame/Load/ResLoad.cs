using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Farme.Tool;
using System.Collections.Generic;

namespace Farme
{
    /// <summary>
    /// Resources加载
    /// </summary>
    public class ResLoad
    {
        protected ResLoad() { }
        #region//同步加载资源
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resPath">资源路径</param>
        /// <returns>结果</returns>
        public static T Load<T>(string resPath) where T : Object
        {
            T result = Resources.Load<T>(resPath);
            if (result == null)
            {
                Debuger.LogError("[" + resPath + "]" + "路径下的资源加载失败。");
            }
            return result;
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resPath">资源路径</param>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public static bool Load<T>(string resPath, out T result) where T : Object
        {
            result = Load<T>(resPath);
            return result != null;
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resPath"></param>
        /// <returns></returns>
        public static T[] LoadAll<T>(string resPath) where T : Object
        {
            T[] ts = Resources.LoadAll<T>(resPath);
            if (ts == null)
            {
                Debuger.LogError("[" + resPath + "]" + "路径下的资源加载失败。");
            }
            return ts;
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resPath"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        public static bool LoadAll<T>(string resPath,out T[] results)where T : Object
        {
            results = LoadAll<T>(resPath);
            return results != null;
        } 
        #endregion
        #region//异步加载资源
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">资源路径</param>
        /// <param name="callback">回调</param>
        public static void LoadAsync<T>(string resPath, UnityAction<T> callback) where T : Object
        {            
            //开启协程进行资源加载
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IELoadAsync(resPath, callback));
        }
        /// <summary>
        /// 协程启动异步加载Res资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">资源路径</param>
        /// <param name="callback">回调</param>
        private static IEnumerator IELoadAsync<T>(string resPath, UnityAction<T> callback) where T : Object
        {
            //异步加载资源
            ResourceRequest rr = Resources.LoadAsync<T>(resPath);
            yield return rr;//等待请求完成
            T t = rr.asset as T;
            if (t == null)
            {
                Debuger.LogError("[" + resPath + "]" + "路径下的资源加载失败。");
            }
            callback?.Invoke(t);
        }
        #endregion      
    }
}
