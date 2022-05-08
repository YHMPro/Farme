﻿using System.Collections.Generic;
using UnityEngine;
using Farme.Tool;
namespace Farme
{
    /// <summary>
    /// Go复用池
    /// </summary>
    public class GoReusePool
    {
        protected GoReusePool() { }
        #region 字段
        private static Dictionary<string, List<GameObject>> m_ReuseGoDic = null;
        private static Dictionary<string, List<GameObject>> ReuseGoDic
        {
            get
            {
                if (m_ReuseGoDic == null)
                {
                    m_ReuseGoDic = new Dictionary<string, List<GameObject>>();
                }
                return m_ReuseGoDic;
            }
        }
        #endregion
        #region 方法
        /// <summary>
        /// 拿去Go实例
        /// </summary>
        /// <param name="reuseGroup">复用组</param>
        /// <returns></returns>
        public static GameObject Take(string reuseGroup)
        {
            if (ReuseGoDic.TryGetValue(reuseGroup, out List<GameObject> goLi)&& goLi.Count>0)
            {            
                GameObject go = goLi[0];              
                goLi.RemoveAt(0);
                go.SetActive(true);
                return go;
            }      
            return null;
        }
        /// <summary>
        /// 拿去Go实例
        /// </summary>
        /// <param name="reuseGroup">复用组</param>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public static bool Take(string reuseGroup, out GameObject result)
        {
            result = Take(reuseGroup);        
            return result!=null;
        }
        /// <summary>
        /// 放入Go实例
        /// </summary>
        /// <param name="reuseGroup">复用组</param>
        /// <param name="target">对象</param>
        public static void Put(string reuseGroup, GameObject target)
        {
            if (target == null)
            {
                return;
            }
            target.SetActive(false);
            if (ReuseGoDic.TryGetValue(reuseGroup, out List<GameObject> goLi))
            {
                goLi.Add(target);
                return;
            }
            ReuseGoDic.Add(reuseGroup, new List<GameObject>() { target });
        }
        /// <summary>
        /// 预热
        /// 基于Resources加载预热
        /// </summary>
        /// <param name="goPath">go路径</param>
        /// <param name="reuseGroup">复用组</param>
        /// <param name="total">总数</param>
        /// <param name="parent">父级</param>
        public static void Preheat(string goPath, string reuseGroup, int total, Transform parent = null)
        {
            while (total > 0)
            {
                if (GoLoad.Take(goPath, out GameObject result, parent))
                {
                    Put(reuseGroup, result);
                }
                total--;
            }
        }
        /// <summary>
        /// 预热
        /// 基于AssetsBundle加载预热
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <param name="reuseGroup">复用组</param>
        /// <param name="total">总数</param>
        /// <param name="parent">父级</param>
        public static void Preheat(string abName,string resName, string reuseGroup, int total, Transform parent = null)
        {
            while (total > 0)
            {
                if (GoLoad.Take(abName,resName,out GameObject result,parent))
                {
                    Put(reuseGroup, result);
                }
                total--;
            }
        }
        #endregion
    }
}
