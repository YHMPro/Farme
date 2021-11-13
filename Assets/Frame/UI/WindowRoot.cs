using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Farme.Tool;
namespace Farme.UI
{
    /// <summary>
    /// 窗口根节点
    /// </summary>
    public class WindowRoot : MonoBehaviour
    {
        protected WindowRoot() { }
        #region 字段
        private readonly string m_WindowModelPath = "FarmeLockFile\\WindowModel";
        private EventSystem m_ES = null;
        private StandaloneInputModule m_InputModule = null;
        private Camera m_Camera = null;
        private Dictionary<string, WindowModel> m_WindowModelDic = null;
        #endregion
        #region 属性
        /// <summary>
        /// 事件系统
        /// </summary>
        public EventSystem ES
        {
            get
            {
                if(m_ES==null)
                {
                    m_ES = GetComponent<EventSystem>();
                }
                return m_ES;
            }
        }
        /// <summary>
        /// 相机
        /// </summary>
        public Camera Camera
        {
            get
            {
                if(m_Camera==null)
                {
                    m_Camera = GetComponent<Camera>();
                }
                return m_Camera;
            }
        }
        #endregion
        #region 生命周期函数
        private void Awake()
        {
            m_WindowModelDic = new Dictionary<string, WindowModel>();
            m_ES = GetComponent<EventSystem>();
            m_InputModule = GetComponent<StandaloneInputModule>();
            m_Camera = GetComponent<Camera>();
        }
        #endregion
        
        public void CreateWindow(string windowName, RenderMode renderMode=RenderMode.ScreenSpaceOverlay)
        {
            if(m_WindowModelDic.ContainsKey(windowName))
            {
                Debuger.LogWarning("窗口:"+ windowName+"反复创建。");
                return;
            }
            if (GoLoad.Take(m_WindowModelPath,out GameObject go))
            {
                WindowModel windowModel = go.GetComponent<WindowModel>();
                windowModel.gameObject.name = windowName;          
                windowModel.Canvas.renderMode = renderMode;
                windowModel.Canvas.worldCamera = m_Camera;             
                switch (renderMode)
                {
                    case RenderMode.ScreenSpaceOverlay:
                        {
                            
                            break;
                        }
                    case RenderMode.ScreenSpaceCamera:
                        {
                            
                            break;
                        }
                    case RenderMode.WorldSpace:
                        {
                            windowModel.transform.localScale = Vector3.one * 0.001f;
                            break;
                        }
                }
                m_WindowModelDic.Add(windowName, windowModel);
            }                                      
        }

        public WindowModel GetWindowInstance(string windowName)
        {
            if(m_WindowModelDic.TryGetValue(windowName,out WindowModel windowModel))
            {
                return windowModel;
            }
            return null;
        }

        public void RemoveWindow(string windowName)
        {
            if(m_WindowModelDic.TryGetValue(windowName,out WindowModel windowModel))
            {
                m_WindowModelDic.Remove(windowName);
                Destroy(windowModel.gameObject);
            }          
        }
    }
}
