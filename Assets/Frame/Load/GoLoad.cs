using UnityEngine;
using UnityEngine.Events;
namespace Farme
{
    /// <summary>
    /// Go加载
    /// </summary> 
    public class GoLoad
    {
        protected GoLoad() { }
        #region 方法       
        /// <summary>
        /// 拿取Go实例
        /// 基于Resources加载方式
        /// </summary>
        /// <param name="goPath">go路径</param>
        /// <param name="parent">父级</param>
        /// <returns>go实例</returns>
        public static GameObject Take(string goPath,Transform parent = null)
        {
            GameObject go = ResLoad.Load<GameObject>(goPath);
            string goName = go.name;
            go = Object.Instantiate(go, parent);
            go.name = goName;
            return go;
        }
        /// <summary>
        /// 拿取Go实例
        /// 基于Resources加载方式
        /// </summary>
        /// <param name="goPath">go路径</param>
        /// <param name="result">拿取的Go实例</param>
        /// <param name="parent">父级</param>
        /// <returns></returns>
        public static bool Take(string goPath, out GameObject result, Transform parent = null)
        {
            result = Take(goPath, parent);
            return result != null;
        }
        /// <summary>
        /// 拿取Go实例
        /// 基于AssetBundle加载方式
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <param name="parent">父级</param>
        /// <returns></returns>
        public static GameObject Take(string abName, string resName, Transform parent = null)
        {
            GameObject go = AssetBundleLoad.LoadAsset<GameObject>(abName, resName);
            string goName = go.name;
            go = Object.Instantiate(go, parent);
            go.name = goName;
            return go;
        }
        /// <summary>
        /// 拿取Go实例
        /// 基于AssetBundle加载方式
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <param name="result">拿取的Go实例</param>
        /// <param name="parent">父级</param>
        /// <returns>是否成功</returns>
        public static bool Take(string abName, string resName, out GameObject result, Transform parent = null)
        {
            result = Take(abName, resName, parent);
            return result != null;
        }
        /// <summary>
        /// 拿取Go实例
        /// 基于Resources加载方式
        /// </summary>
        /// <param name="goPath">go路径</param>
        /// <param name="parent">父级</param>
        /// <param name="callback">回调</param>
        public static void TakeAsync(string goPath, Transform parent = null,UnityAction<GameObject> callback=null)
        {
            ResLoad.LoadAsync<GameObject>(goPath, (go) =>
             {
                 if (go != null)
                 {
                     string goName = go.name;
                     if (parent != null)
                     {
                         go=Object.Instantiate(go, parent);
                     }
                     else
                     {
                         go=Object.Instantiate(go);
                     }
                     go.name = goName;                   
                 }
                 callback?.Invoke(go);
             });                           
        }
        #endregion
    }
}
