using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Farme.UI
{
    /// <summary>
    /// 面板基类
    /// </summary>
    public abstract class BasePanel : BaseMono
    {
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
        /// 面板风格
        /// </summary>
        /// <param name="style">风格</param>
        public virtual void PanelStyle(string style)
        {
            switch(style)
            {
                case "destroy":
                    {
                        Destroy(gameObject);
                        break;
                    }
            }
        }
        #endregion
    }
}
