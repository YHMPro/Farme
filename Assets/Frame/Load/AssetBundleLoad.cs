using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Farme.Tool;
using System.IO;
namespace Farme
{
    /// <summary>
    /// AB包加载
    /// </summary>
    public class AssetBundleLoad
    {
        /// <summary>
        /// 主包信息  包含子包之间的配置信息与依赖关系
        /// </summary>
        private static AssetBundleManifest m_MainABInfo = null;
        /// <summary>
        /// 主包
        /// 每个工程在进行资源打包时只会存在一个主包,主包本身不存放资源文件，只包含子包之间的依赖或其他
        /// 同一个主包只能加载一次
        /// </summary>
        private static AssetBundle m_MainAB = null;
        /// <summary>
        /// 包容器
        /// </summary>
        private static Dictionary<string, AssetBundle> m_ABDic = new Dictionary<string, AssetBundle>();
        /// <summary>
        /// 主包所属文件夹的地址
        /// </summary>
        private static string m_MainABFile_URL = null;
        /// <summary>
        /// 主包名
        /// </summary>
        private static string m_MainABName = null;
        /// <summary>
        /// 主包所属文件夹的地址
        /// </summary>
        public static string MainABFile_URL
        {
            set
            {
                m_MainABFile_URL = value;
            }
            get
            {
                return m_MainABFile_URL;
            }
            
        }            
        /// <summary>
        /// 主包名
        /// </summary>
        public static string MainABName
        {
            set
            {
                m_MainABName = value;
            }
            get
            {
                return m_MainABName;
            }
        }
        #region 异步初始化主包与子包配置信息
        /// <summary>
        /// 初始化主包
        /// </summary>
        /// <param name="reslutCallback">结果回调</param>
        public static void InitMainABAsync(UnityAction reslutCallback=null)
        {       
            if(!File.Exists(m_MainABFile_URL + "\\"+ m_MainABName))//判定主包文件是否存在
            {
                Debuger.LogError("主包文件路径错误!!!");
                return;
            }
            if (m_MainAB != null)
            {
                reslutCallback?.Invoke();
                return;
            }
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEInitMainAB(reslutCallback));
        }
        private static IEnumerator IEInitMainAB(UnityAction reslutCallback=null)
        {          
                AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(m_MainABFile_URL + m_MainABName);
                while(true)
                {
                    if(abcr.isDone)
                    {
                        Debuger.Log("主包加载成功");
                        break;
                    }                   
                    yield return abcr.progress;
                }
                m_MainAB = abcr.assetBundle;
                AssetBundleRequest abr = m_MainAB.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
                while(true)
                {
                    if(abr.isDone)
                    {
                        Debuger.Log("包依赖信息获取成功");
                        break;
                    }
                    yield return abr.progress;
                }
                m_MainABInfo = abr.asset as AssetBundleManifest;
                reslutCallback?.Invoke();         
        }
        #endregion
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <param name="resultCallback">结果回调</param>
        public static void LoadAssetAsync<T>(string abName, string resName, UnityAction<T> resultCallback) where T : Object
        {
            InitMainABAsync(() => {
                InitAssetBundleDependencies(abName, (ab) => {
                    if(ab==null)
                    {
                        resultCallback?.Invoke(null);
                        return;
                    }
                    MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IELoadAsset(ab, resName, resultCallback));
                });            
            });        
        }
        private static IEnumerator IELoadAsset<T>(AssetBundle ab, string resName, UnityAction<T> resultCallback) where T : Object
        {
            AssetBundleRequest abr = ab.LoadAssetAsync<T>(resName);
            while (true)
            {
                if (abr.isDone)
                {
                    break;
                }
                yield return abr.progress;
            }
            resultCallback?.Invoke(abr.asset as T);         
        }
        /// <summary>
        /// 初始化包的依赖包并加载该包
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="reslutCallback">结果回调</param>
        private static void InitAssetBundleDependencies(string abName, UnityAction<AssetBundle> reslutCallback)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEInitAssetBundleDependencies(abName,reslutCallback));
        }
        private static IEnumerator IEInitAssetBundleDependencies(string abName,UnityAction<AssetBundle> reslutCallback)
        {
            AssetBundle ab;
            AssetBundleCreateRequest abcr;
            //获取存在依赖关系的包   
            string[] dependenciesABundle = m_MainABInfo.GetAllDependencies(abName);
            //逐个遍历所有与自身存在依赖关系的包
            for (int Index = 0; Index < dependenciesABundle.Length; Index++)
            {
                //判断依赖包是否已经加载过
                if (!m_ABDic.ContainsKey(dependenciesABundle[Index]))
                {
                    if(!File.Exists(m_MainABFile_URL + dependenciesABundle[Index]))//判定包文件是否存在
                    {
                        Debuger.LogError(dependenciesABundle[Index] + "包文件不存在");
                        break;
                    }
                    abcr = AssetBundle.LoadFromFileAsync(m_MainABFile_URL + dependenciesABundle[Index]);
                    while (true)
                    {
                        if (abcr.isDone)
                        {
                            break;
                        }
                        yield return abcr.progress;
                    }
                    ab = abcr.assetBundle;
                    //添加到包容器当中
                    m_ABDic.Add(dependenciesABundle[Index], ab);
                }
            }
            if(m_ABDic.TryGetValue(abName,out ab))
            {
                reslutCallback?.Invoke(ab);
            }
            else
            {
                if (!File.Exists(m_MainABFile_URL + abName))//判定包文件是否存在
                {
                    Debuger.LogError(abName + "包文件不存在");
                    reslutCallback?.Invoke(null);
                }
                else
                {
                    abcr = AssetBundle.LoadFromFileAsync(m_MainABFile_URL + abName);
                    while (true)
                    {
                        if (abcr.isDone)
                        {
                            break;
                        }
                        yield return abcr.progress;
                    }
                    ab = abcr.assetBundle;
                    //将子包添加到包容器当中
                    m_ABDic.Add(abName, ab);
                    reslutCallback?.Invoke(ab);
                }             
            }                 
        }
    }
}
