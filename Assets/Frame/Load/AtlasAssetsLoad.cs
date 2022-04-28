using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.Tool;
using UnityEngine.Events;
namespace Farme
{
    /// <summary>
    /// 图集资源加载
    /// </summary>
    public class AtlasAssetsLoad 
    { 
        protected AtlasAssetsLoad() { }
        #region 同步加载
        
        /// <summary>
        /// 加载图集中的资源
        /// </summary>
        /// <param name="atlasPath">图集路径</param>
        /// <param name="sprName">资源名</param>
        /// <returns></returns>
        public static Sprite Load(string atlasPath, string sprName)
        {           
            return FindSprite(ResLoad.LoadAll<Sprite>(atlasPath), sprName);      
        }
        /// <summary>
        /// 加载图集中的资源
        /// </summary>
        /// <param name="atlasPath">图集路径</param>
        /// <param name="sprName">资源名</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool Load(string atlasPath, string sprName, out Sprite result)
        {
            result = Load(atlasPath, sprName);
            return result != null;
        }
        /// <summary>
        /// 加载图集中的资源
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="sprName">资源名</param>
        /// <returns></returns>
        public static Sprite Load(string abName,string sprName, bool empty = false)
        {
            return FindSprite(AssetBundleLoad.LoadAllAssets<Sprite>(abName), sprName);
        }
        /// <summary>
        /// 加载图集中的资源
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="sprName">资源名</param>
        /// <param name="spr"></param>
        /// <returns></returns>
        public static bool Load(string abName,string sprName,out Sprite spr,bool empty=false)
        {
            spr = Load(abName,sprName);
            return spr != null;
        }
        #endregion
        #region 异步加载
        /// <summary>
        /// 加载图集中的资源
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="sprName">资源名</param>
        /// <param name="callback">回调</param>
        public static void LoadAsync(string abName,string sprName,UnityAction<Sprite> callback)
        {
            AssetBundleLoad.LoadAllAssetAsync<Sprite>(abName, (sprList) =>
            {
                callback?.Invoke(FindSprite(sprList.ToArray(), sprName));
                return;
            });
            callback?.Invoke(null);
        }
        #endregion
        /// <summary>
        /// 寻找
        /// </summary>
        /// <param name="sprs"></param>
        /// <param name="sprName"></param>
        /// <returns></returns>
        private static Sprite FindSprite(Sprite[] sprs,string sprName )
        {
            foreach(Sprite spr in sprs)
            {
                if (spr.name == sprName)
                {
                    return spr;
                }
            }
            return null;
        }
    }
}
