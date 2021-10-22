using System.Collections.Generic;
using UnityEngine;
namespace Farme
{
    /// <summary>
    /// Go复用池
    /// </summary>
    public class GoReusePool
    {
        protected GoReusePool() { }
        #region 字段
        private static Dictionary<string,List<GameObject>> _reuseGoDic = null;
        #endregion
        #region 方法
        /// <summary>
        /// 拿去Go实例
        /// </summary>
        /// <param name="reuseGroup">复用组</param>
        /// <param name="result">结果</param>
        /// <param name="parent">父级</param>
        /// <returns></returns>
        public static bool Take(string reuseGroup, out GameObject result)
        {
            result = null;
            if(_reuseGoDic==null)
            {
                _reuseGoDic = new Dictionary<string, List<GameObject>>();
            }
            if (_reuseGoDic.TryGetValue(reuseGroup,out List<GameObject> goLi))
            {
                foreach(var go in goLi)
                {
                    if(go!=null)
                    {
                        result = go;
                        result.SetActive(true);                       
                        goLi.Remove(result);
                        return true;
                    }
                }
                if(result==null)
                {
                    goLi.Clear();               
                }          
            }       
            return false;
        }
        /// <summary>
        /// 放入Go实例
        /// </summary>
        /// <param name="reuseGroup">复用组</param>
        /// <param name="target">对象</param>
        public static void Put(string reuseGroup,GameObject target)
        {
            if (_reuseGoDic == null)
            {
                _reuseGoDic = new Dictionary<string, List<GameObject>>();
            }
            if(target==null)
            {
                return;
            }
            target.SetActive(false);
            if (_reuseGoDic.TryGetValue(reuseGroup,out List<GameObject> goLi))
            {
                goLi.Add(target);
                return;
            }
            _reuseGoDic.Add(reuseGroup, new List<GameObject>() { target });
        }
        /// <summary>
        /// 预热
        /// </summary>
        /// <param name="goPath">go路径</param>
        /// <param name="reuseGroup">复用组</param>
        /// <param name="total">总数</param>
        /// <param name="parent">父级</param>
        public static void Preheat(string goPath,string reuseGroup,int total,Transform parent=null)
        {
            while (total>0)
            {
                if (GoLoad.Take(goPath, out GameObject result, parent))
                {
                    Put(reuseGroup, result);
                }
                total--;
            }
        }
        #endregion
    }
}
