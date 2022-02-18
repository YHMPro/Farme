using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Farme.Tool;
namespace Farme
{
    /// <summary>
    /// 场景加载
    /// </summary>
    public class SceneLoad
    {
        public static Scene GetSceneByName(string sceneName)
        {
            return SceneManager.GetSceneByName(sceneName);
        }

        public static bool SetActiveScene(Scene scene)
        {
            if (!scene.isLoaded)
            {
                Debuger.LogError(scene.name + "场景无效");
                return false;
            }
            return SceneManager.SetActiveScene(scene);
        }

        #region//异步加载场景
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="loadModel">加载模式</param>
        /// <param name="startLoadCallback">开始加载回调</param>
        /// <param name="endLoadCallback">结束加载回调</param>
        /// <param name="loadProgressCallback">加载进度回调</param>
        public static void LoadSceneAsync(string sceneName, LoadSceneMode loadModel = LoadSceneMode.Single, UnityAction startLoadCallback = null, UnityAction<bool> endLoadCallback = null, UnityAction<float> loadProgressCallback = null)
        {
            //开启协程
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IELoadScene(sceneName, loadModel, startLoadCallback, endLoadCallback, loadProgressCallback));
        }
        /// <summary>
        /// 协程加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="loadModel">加载模式</param>
        /// <param name="startLoadCallback">回调切换场景前</param>
        /// <param name="endLoadCallback">回调切换场景后</param>
        /// <param name="loadProgressCallback">加载进度回调</param>
        /// <returns></returns>
        private static IEnumerator IELoadScene(string sceneName, LoadSceneMode loadModel = LoadSceneMode.Single, UnityAction startLoadCallback = null, UnityAction<bool> endLoadCallback = null, UnityAction<float> loadProgressCallback = null)
        {
            //用于处理加载前场景数据
            startLoadCallback?.Invoke();
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, loadModel);
            if (ao == null)
            {
                Debuger.LogError("场景不存在!");
                endLoadCallback?.Invoke(false);
                yield break;
            }
            while (!ao.isDone)//等待场景加载完成
            {
                loadProgressCallback?.Invoke(ao.progress);
                yield return true;
            }
            //用于初始化数据
            endLoadCallback?.Invoke(true);
        }
        #endregion

        #region 异步卸载场景
        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="startUnLoadCallback">开始卸载回调</param>
        /// <param name="endUnLoadCallback">卸载结束回调</param>
        /// <param name="unLoadProgressCallback">卸载进度回调</param>
        public static void UnLoadSceneAsync(string sceneName, UnityAction startUnLoadCallback = null, UnityAction<bool> endUnLoadCallback = null, UnityAction<float> unLoadProgressCallback = null)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEUnLoadScene(sceneName, startUnLoadCallback, endUnLoadCallback, unLoadProgressCallback));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="startUnLoadCallback">开始卸载回调</param>
        /// <param name="endUnLoadCallback">卸载结束回调</param>
        /// <param name="unLoadProgressCallback">卸载进度回调</param>
        /// <returns></returns>
        private static IEnumerator IEUnLoadScene(string sceneName, UnityAction startUnLoadCallback = null, UnityAction<bool> endUnLoadCallback = null, UnityAction<float> unLoadProgressCallback = null)
        {
            startUnLoadCallback?.Invoke();
            AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneName);
            if (ao == null)
            {
                Debuger.LogError("场景不存在!");
                endUnLoadCallback?.Invoke(false);
                yield break;
            }
            while (!ao.isDone)//等待场景卸载完成
            {
                unLoadProgressCallback?.Invoke(ao.progress);
                yield return true;
            }
            //用于初始化数据
            endUnLoadCallback?.Invoke(true);
        }
        #endregion

    }
}
