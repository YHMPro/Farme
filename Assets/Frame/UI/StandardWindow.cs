using System.Collections.Generic;
using UnityEngine;
using Farme.Tool;
using UnityEngine.UI;
using UnityEngine.Events;
namespace Farme.UI
{
    /// <summary>
    /// 面板状态
    /// </summary>
    public enum EnumWindowState
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 显示
        /// </summary>
        Show,
        /// <summary>
        /// 隐藏
        /// </summary>
        Hide,
        /// <summary>
        /// 销毁
        /// </summary>
        Destroy
    }
    /// <summary>
    /// 窗口层级
    /// </summary>
    public enum EnumPanelLayer
    {
        /// <summary>
        /// 窗口
        /// </summary>
        Self,
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
    /// 标准窗口
    /// </summary>
    public class StandardWindow : MonoBehaviour
    {
        #region 生命周期
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
        #endregion
        #region 字段
        /// <summary>
        /// 面板容器
        /// </summary>
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
        /// 
        /// </summary>
        private GraphicRaycaster m_GR = null;
        /// <summary>
        /// 画布布局
        /// </summary>
        private CanvasScaler m_CS = null;
        #endregion
        #region 属性
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
        #endregion
        #region 方法
        /// <summary>
        /// 创建面板
        /// </summary>
        /// <typeparam name="T">面板类型</typeparam>
        /// <param name="path">路径</param>
        /// <param name="panelName">面板名称</param>
        /// <param name="layer">层级</param>
        /// <param name="callback">回调</param>
        public void CreatePanel<T>(string path,string panelName, EnumPanelLayer layer = EnumPanelLayer.BOTTOM, UnityAction<T> callback=null) where T:BasePanel
        {
            if (m_PanelDic.ContainsKey(panelName))
            {
                return;
            }
            if (GoLoad.Take(path, out GameObject panel, GetPanelLayer(layer)))
            {
                T t = panel.InspectComponent<T>();
                t.relyWindow = this;
                m_PanelDic.Add(panelName, t);
                callback?.Invoke(t);
                return;
            }
            Debuger.LogWarning("面板加载失败。");
        }
        /// <summary>
        /// 移除面板引用
        /// </summary>
        /// <param name="panelName">面板名称</param>
        public void RemovePanel(string panelName)
        {
            if(m_PanelDic.ContainsKey(panelName))
            {
                m_PanelDic.Remove(panelName);
            }
        }
        /// <summary>
        /// 获取面板
        /// </summary>
        /// <typeparam name="T">面板类型</typeparam>
        /// <param name="panelName">面板名称</param>
        /// <returns></returns>
        public T GetPanel<T>(string panelName) where T : BasePanel
        {
            if (m_PanelDic.TryGetValue(panelName, out BasePanel panel))
            {
                return panel as T;
            }
            Debuger.LogWarning(panelName + "面板不存在。");
            return null;
        }
        /// <summary>
        /// 获取面板
        /// </summary>
        /// <typeparam name="T">面板类型</typeparam>
        /// <param name="panelName">面板名称</param>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public bool GetPanel<T>(string panelName,out T result)where T:BasePanel
        {
            if(m_PanelDic.TryGetValue(panelName,out BasePanel panel ))
            {
                result = panel as T;
                return true;
            }
            result = null;
            Debuger.LogWarning(panelName + "面板不存在。");
            return false;
        }
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="callback">回调</param>
        public void SetState(EnumWindowState state, UnityAction callback = null)
        {
            switch (state)
            {
                case EnumWindowState.Show:
                    {
                        gameObject.SetActive(true);
                        callback?.Invoke();
                        break;
                    }
                case EnumWindowState.Hide:
                    {
                        callback?.Invoke();
                        gameObject.SetActive(false);
                        break;
                    }
                case EnumWindowState.Destroy:
                    {
                        
                        if(MonoSingletonFactory<WindowRoot>.SingletonExist)
                        {
                            MonoSingletonFactory<WindowRoot>.GetSingleton().RemoveWindow(gameObject.name);
                        }
                        else
                        {
                            Debuger.LogWarning("窗口根节点不存在");
                        }
                        callback?.Invoke();
                        Destroy(gameObject);
                        break;
                    }
                case EnumWindowState.None:
                    {
                        callback?.Invoke();
                        break;
                    }
            }
        }
        /// <summary>
        /// 面板层级转换
        /// </summary>
        /// <param name="panel">面板</param>
        /// <param name="layer">层级</param>
        public void PanelLayerTransform(BasePanel panel, EnumPanelLayer layer = EnumPanelLayer.BOTTOM)
        {
            if (panel == null)
            {
                Debuger.LogError("面板为NULL");
                return;
            }
            panel.transform.SetParent(GetPanelLayer(layer));
        }
        /// <summary>
        /// 获取面板层级
        /// </summary>
        /// <param name="layer">窗口层级</param>
        /// <returns></returns>   
        private Transform GetPanelLayer(EnumPanelLayer layer)
        {
            Transform tran = null;
            //设置父级
            switch (layer)
            {
                case EnumPanelLayer.BOTTOM: tran = m_TranBottom; break;

                case EnumPanelLayer.MIDDLE: tran = m_TranMiddle; break;

                case EnumPanelLayer.TOP: tran = m_TranTop; break;

                case EnumPanelLayer.SYSTEM: tran = m_TranSystem; break;

                case EnumPanelLayer.Self: tran = transform; break;

            }
            return tran;
        }        
        #endregion
    }
}
