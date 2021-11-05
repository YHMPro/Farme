using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
namespace Farme.Net
{
    /// <summary>
    /// 网络下载工具
    /// </summary>
    public class WebDownloadTool 
    {
        private static float m_DownloadTextureRequestOutTime = 10;
        private static float m_DownloadAssetBundleOutTime = 10;

        #region AB包下载
        public static void WebDownloadAssetBundle(string url, UnityAction<AssetBundle> resultCallback, UnityAction<float> downLoadProgress = null, UnityAction requestTimeOutCallback = null)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEWebDownloadAssetBundle(url, resultCallback, downLoadProgress, requestTimeOutCallback));
        }
        private static IEnumerator IEWebDownloadAssetBundle(string url, UnityAction<AssetBundle> resultCallback, UnityAction<float> downLoadProgress = null, UnityAction requestTimeOutCallback = null)
        {
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);//创建网络请求
            www.SendWebRequest();//发送请求
            bool isRequestTimeOut = false;
            Coroutine cor = null;
            while (true)
            {
                if (www.downloadProgress <= 0.1f)
                {
                    if (!isRequestTimeOut)
                    {
                        isRequestTimeOut = true;
                        if (requestTimeOutCallback != null)
                        {
                            cor = MonoSingletonFactory<ShareMono>.GetSingleton().DelayUAction(m_DownloadAssetBundleOutTime, requestTimeOutCallback);
                            break;
                        }
                    }
                }
                downLoadProgress?.Invoke(www.downloadProgress);//回调下载进度
                if (www.isDone && www.downloadHandler.isDone)
                {
                    if (cor != null)
                    {
                        MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(cor);//撤销下载超时回调
                    }
                    break;
                }
                yield return www.downloadProgress;
            }
            resultCallback?.Invoke(DownloadHandlerAssetBundle.GetContent(www));
        }
        #endregion

        #region 纹理下载
        public static void WebDownloadTexture(string url, UnityAction<Texture2D> resultCallback, UnityAction<float> downLoadProgress = null, UnityAction requestTimeOutCallback=null)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEWebDownloadTexture(url, resultCallback, downLoadProgress, requestTimeOutCallback));
        }       
        private static IEnumerator IEWebDownloadTexture(string url, UnityAction<Texture2D> resultCallback,UnityAction<float> downLoadProgress = null, UnityAction requestTimeOutCallback=null)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);//创建网络请求
            www.SendWebRequest();//发送请求
            bool isRequestTimeOut = false;
            Coroutine cor = null;
            while(true)
            {
                if(www.downloadProgress<=0.1f)
                {
                    if(!isRequestTimeOut)
                    {
                        isRequestTimeOut = true;
                        if (requestTimeOutCallback != null)
                        {
                            cor=MonoSingletonFactory<ShareMono>.GetSingleton().DelayUAction(m_DownloadTextureRequestOutTime, requestTimeOutCallback);
                            break;
                        }
                    }
                }
                downLoadProgress?.Invoke(www.downloadProgress);//回调下载进度
                if (www.isDone&&www.downloadHandler.isDone)
                {
                    if (cor != null)
                    {
                        MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(cor);//撤销下载超时回调
                    }
                    break;
                }
                yield return www.downloadProgress;
            }
            resultCallback?.Invoke(DownloadHandlerTexture.GetContent(www));
        }
        #endregion
    }
}
