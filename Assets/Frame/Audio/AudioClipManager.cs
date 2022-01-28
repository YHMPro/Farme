using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Farme.Extend;
namespace Farme.Audio
{
    /// <summary>
    /// 音效剪辑管理器
    /// </summary>
    public class AudioClipManager
    {
        #region 字段
        /// <summary>
        /// 音效剪辑容器   key:路径或xx包{xx资源}为拿取依据  value:音效剪辑
        /// </summary>
        private static Dictionary<string, AudioClip> m_AudioClipDic = null;
        private static Dictionary<string, AudioClip> AudioClipDic
        {
            get
            {
                if (m_AudioClipDic == null)
                {
                    m_AudioClipDic = new Dictionary<string, AudioClip>();
                }
                return m_AudioClipDic;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        ///获取音效剪辑
        /// 基于Resources加载
        /// </summary>
        /// <param name="audioClipPath">音效剪辑路径</param>
        /// <param name="callback">回调</param>
        public static void GetAudioClip(string audioClipPath, UnityAction<AudioClip> callback)
        {
            if (AudioClipDic.TryGetValue(audioClipPath, out AudioClip result))
            {
                callback?.Invoke(result);
            }
            else
            {
                ResourcesLoad.LoadAsync<AudioClip>(audioClipPath, (clip) =>
                {
                    AudioClipDic.Add(audioClipPath, clip);
                    callback?.Invoke(result);
                });
            }
        }
        /// <summary>
        /// 获取音效剪辑
        /// 基于Resources加载
        /// </summary>
        /// <param name="audioClipPath">音效剪辑路径</param>
        /// <param name="result">结果</param>
        /// <returns>是否获取成功</returns>
        public static bool GetAudioClip(string audioClipPath, out AudioClip result)
        {
            if (AudioClipDic.TryGetValue(audioClipPath, out result))
            {
                return true;
            }
            else
            {
                if (ResourcesLoad.Load(audioClipPath, out result))
                {
                    AudioClipDic.Add(audioClipPath, result);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取音效剪辑
        /// 基于Resources加载
        /// </summary>
        /// <param name="audioClipPath">音效剪辑路径</param>
        /// <returns></returns>
        public static AudioClip GetAudioClip(string audioClipPath)
        {
            if (AudioClipDic.TryGetValue(audioClipPath, out AudioClip result))
            {
                return result;
            }
            else
            {
                if (ResourcesLoad.Load(audioClipPath, out result))
                {
                    AudioClipDic.Add(audioClipPath, result);
                    return result;
                }
            }
            return null;
        }
        /// <summary>
        /// 基于AssetsBundle加载
        /// 异步加载
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <param name="callback">回调</param>
        public static void GetAudioClip(string abName, string resName, UnityAction<AudioClip> callback)
        {
            if (AudioClipDic.TryGetValue(abName + "{" + resName + "}", out AudioClip result))
            {
                callback?.Invoke(result);
            }
            else
            {
                AssetBundleLoad.LoadAssetAsync<AudioClip>(abName, resName, (clip) =>
                {
                    AudioClipDic.Add(abName + "{" + resName + "}", clip);
                    callback?.Invoke(result);
                });
            }
        }
        /// <summary>
        /// 获取音频剪辑
        /// 基于AssetsBundle加载
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public static bool GetAudioClip(string abName, string resName, out AudioClip result)
        {
            if (AudioClipDic.TryGetValue(abName + "{" + resName + "}", out result))
            {
                return true;
            }
            else
            {
                if (AssetBundleLoad.LoadAsset(abName, resName, out result))
                {
                    AudioClipDic.Add(abName + "{" + resName + "}", result);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取音频剪辑
        /// 基于AssetsBundle加载
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <returns></returns>
        public static AudioClip GetAudioClip(string abName, string resName)
        {
            if (AudioClipDic.TryGetValue(abName + "{" + resName + "}", out AudioClip result))
            {
                return result;
            }
            else
            {
                if (AssetBundleLoad.LoadAsset(abName, resName, out result))
                {
                    AudioClipDic.Add(abName + "{" + resName + "}", result);
                    return result;
                }
            }
            return null;
        }
        #endregion

        #region Clear
        /// <summary>
        /// 卸载缓存的音效剪辑
        /// </summary>
        /// <param name="clipKey">剪辑唯一标识(剪辑路径或AB包名+剪辑名)</param>
        /// <returns></returns>
        public static bool UnLoadAudioClip(string clipKey)
        {
            if (AudioClipDic.TryGetValue(clipKey, out AudioClip result))
            {
                result.UnloadAudioData();
                AudioClipDic.Remove(clipKey);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 依照忽略来进行采写(默认为不忽略)
        /// </summary>
        /// <param name="ignoreClipKeys">忽略剪辑数组</param>
        public static void UnLoadAllAudioClip(string[] ignoreClipKeys = null)
        {
            bool isUnload;
            List<string> unloadKeys = null;
            if (ignoreClipKeys != null)
            {
                unloadKeys = new List<string>();
            }
            foreach (string clipKey in AudioClipDic.Keys)
            {
                isUnload = true;
                if (ignoreClipKeys != null)
                {
                    foreach (var ignoreClipKey in ignoreClipKeys)
                    {
                        if (Equals(ignoreClipKey, clipKey))
                        {
                            isUnload = false;
                            break;//跳出ignoreClipKeys循环
                        }
                    }
                    if (!isUnload)
                    {
                        continue;//跳过本次
                    }
                }
                if (AudioClipDic.TryGetValue(clipKey, out AudioClip result))
                {
                    if (unloadKeys != null)
                    {
                        unloadKeys.Add(clipKey);
                    }
                    result.UnloadAudioData();
                }
            }
            if (ignoreClipKeys != null)
            {
                foreach (var unloadKey in unloadKeys)
                {
                    AudioClipDic.Remove(unloadKey);
                }
            }
            else
            {
                AudioClipDic.Clear();
            }

        }
        #endregion
    }
}
