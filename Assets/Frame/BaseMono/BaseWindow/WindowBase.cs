using UnityEngine;
namespace Farme
{   
    /// <summary>
    /// 窗口基类
    /// </summary>
    public abstract class BaseWindow : BaseMono
    {
        protected BaseWindow() { }
        protected override void Awake()
        {
            base.Awake();          
        }
        #region 字段
        /// <summary>
        /// 自身依赖的画布
        /// </summary>
        public Canvas m_RelyCanvas = null;         
        #endregion
        #region 属性
        /// <summary>
        /// 矩形转换
        /// </summary>
        public RectTransform RT
        {
            get
            {
                return transform as RectTransform;
            }
        }       
        #endregion
        #region 方法            
        /// <summary>
        /// 窗口风格
        /// </summary>
        /// <param name="styleIndex">风格索引</param>
        public abstract void WindowStyle(int styleIndex);      
        #endregion
    }
}
