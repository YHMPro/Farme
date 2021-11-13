using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.Tool;
using UnityEngine.UI;

namespace Farme.UI
{
    /// <summary>
    /// 窗口层级
    /// </summary>
    public enum PanelLayer
    {
        /// <summary>
        /// 底层
        /// </summary>
        BOTTOM,
        /// <summary>
        /// 中层
        /// </summary>
        MIDDLE,
        /// <summary>
        /// 顶层
        /// </summary>
        TOP,
        /// <summary>
        /// 系统层
        /// </summary>
        SYSTEM
    }
    /// <summary>
    /// 窗口模块
    /// </summary>
    public class WindowModel : MonoBehaviour
    {
        private Dictionary<string, BasePanel> m_PanelDic;
        /// <summary>
        /// 底层
        /// </summary>
        private Transform m_TranBottom = null;
        /// <summary>
        /// 中层
        /// </summary>
        private Transform m_TranMiddle = null;
        /// <summary>
        /// 顶层
        /// </summary>
        private Transform m_TranTop = null;
        /// <summary>
        /// 系统层(最高层)
        /// </summary>
        private Transform m_TranSystem = null;
        /// <summary>
        /// 画布
        /// </summary>
        private Canvas m_Canvas = null;
        /// <summary>
        /// 画布布局
        /// </summary>
        private CanvasScaler m_CS = null;
        /// <summary>
        /// 画布布局
        /// </summary>
        public CanvasScaler CS
        {
            get
            {
                if(m_CS==null)
                {
                    m_CS = GetComponent<CanvasScaler>();
                }
                return m_CS;
            }
        }
        /// <summary>
        /// 画布
        /// </summary>
        public Canvas Canvas
        {
            get
            {
                if(m_Canvas==null)
                {
                    m_Canvas = GetComponent<Canvas>();
                }
                return m_Canvas;
            }
        }
        private void Awake()
        {
            m_PanelDic = new Dictionary<string, BasePanel>();
            m_Canvas = GetComponent<Canvas>();
            m_CS = GetComponent<CanvasScaler>();
            m_TranBottom = transform.Find("Bottom");
            m_TranMiddle = transform.Find("Middel");
            m_TranTop = transform.Find("Top");
            m_TranSystem = transform.Find("System");
        }
        public T GetPanelInstance<T>(string panelName) where T:BasePanel
        {
            if(m_PanelDic.TryGetValue(panelName,out BasePanel panel))
            {
                return panel as T;
            }
            return null;
        }
        public bool GetPanelInstance<T>(string panelName,out T result) where T : BasePanel
        {         
            if (m_PanelDic.TryGetValue(panelName,out BasePanel panel))
            {
                result = panel as T;
                return true;
            }
            result = null;
            return false;
        }
     
        public bool AddPanel(BasePanel panel, PanelLayer layer=PanelLayer.BOTTOM)
        {
            if(panel == null)
            {
                Debuger.LogError("面板为NULL");
                return false;
            }
            if (m_PanelDic.ContainsKey(panel.name))
            {
                Debuger.LogWarning("面板已添加过:" + panel.name);
                return true;
            }
            m_PanelDic.Add(panel.name, panel);
            PanelLayerChange(panel, layer);
            return true;         
        }
        public bool RemovePanel(string panelName,string style)
        {
            if(m_PanelDic.TryGetValue(panelName,out BasePanel panel))
            {
                m_PanelDic.Remove(panelName);
                if (panel==null)
                {                    
                    Debuger.LogWarning("面板:" + panelName + "在这之前已被销毁，但键仍存在。");
                    return true;
                }
                panel.PanelStyle(style);
                return true;
            }
            Debuger.LogWarning("面板:" + panelName + "不存在。");
            return true;
        }

        public void PanelLayerChange(BasePanel panel, PanelLayer layer = PanelLayer.BOTTOM)
        {
            if (panel == null)
            {
                Debuger.LogError("面板为NULL");
                return ;
            }
            panel.transform.SetParent(GetPanelLayer(layer));
        }
        /// <summary>
        /// 获取面板层级
        /// </summary>
        /// <param name="layer">窗口层级</param>
        /// <returns></returns>   
        private Transform GetPanelLayer(PanelLayer layer)
        {
            Transform tran = null;
            //设置父级
            switch (layer)
            {
                case PanelLayer.BOTTOM: tran = m_TranBottom; break;

                case PanelLayer.MIDDLE: tran = m_TranMiddle; break;

                case PanelLayer.TOP: tran = m_TranTop; break;

                case PanelLayer.SYSTEM: tran = m_TranSystem; break;
                
            }
            if(tran==null)
            {
               tran = transform;
            }
            return tran;
        }
    }
}
