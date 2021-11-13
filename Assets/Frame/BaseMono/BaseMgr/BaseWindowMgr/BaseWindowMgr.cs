using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Farme.Tool;
namespace Farme.UI
{
    /// <summary>
    /// 窗口管理基类
    /// </summary>
    public abstract class BaseWindowMgr : BaseMono
    {
        protected BaseWindowMgr()
        {

        }
        #region 字段
        /// <summary>
        /// 自身指针事件数据
        /// </summary>
        protected PointerEventData m_PED = null;
        /// <summary>
        /// 自身画布
        /// </summary>
        protected Canvas m_Canvas = null;
        /// <summary>
        /// 窗口根节点
        /// </summary>
        protected static WindowRoot m_WindowRoot = null;       
        #endregion
        #region 属性
        /// <summary>
        /// 画布
        /// </summary>
        public Canvas Canvas
        {
            get
            {
                if (m_Canvas == null)
                {
                    m_Canvas = GetComponent<Canvas>();
                }
                return m_Canvas;
            }
        }
        #endregion
        #region 方法       
        protected override void Awake()
        {
            base.Awake();
            RegisterComponentsTypes<Transform>();
            if (m_WindowRoot==null)
            {
                if(GoLoad.Take("FarmeLockFile/WindowRoot",out GameObject go))
                {
                    Debuger.Log("窗口根节点加载成功");
                    m_WindowRoot = go.GetComponent<WindowRoot>();
                    m_PED = new PointerEventData(m_WindowRoot.ES);
                }
            }                             
            m_Canvas = GetComponent<Canvas>();
            gameObject.name = GetType().Name;          
        }     
        /// <summary>
        /// UI事件注册
        /// 注:指针事件触发条件(1:UI透明的>=0.1 2:UI的RaycastTarget为True)
        /// </summary>
        /// <param name="uiB">ui对象</param>
        /// <param name="eTType">添加的事件触发类型</param>
        /// <param name="callBack">添加的回调</param>
        public static void UIEventRegistered(UIBehaviour uiB, EventTriggerType eTType, UnityAction<BaseEventData> callBack)
        {
            if (uiB == null)
            {
                Debuger.Log("申请对象为NULL");
                return;
            }
            if(callBack==null)
            {
                Debuger.Log("回调为NULL");
                return;
            }
            //获取UI对象身下的EventTrigger组件实例
            EventTrigger eT = uiB.gameObject.InspectComponent<EventTrigger>();                     
            //便利已添加的事件
            foreach (var t in eT.triggers)
            {
                //判断需要申请的事件类型是否已添加
                if (t.eventID == eTType)
                {
                    //添加回调监听
                    t.callback.AddListener(callBack);
                    return;
                }
            }
            //实例事件入口用于添加监听的事件
            EventTrigger.Entry e = new EventTrigger.Entry();
            //赋予事件类型
            e.eventID = eTType;
            //添加对该类型事件的监听
            e.callback.AddListener(callBack);
            //实例对象添加对该事件类型的监听
            eT.triggers.Add(e);
        }
        /// <summary>
        /// UI事件移除
        /// </summary>
        /// <param name="uiB">UI对象</param>
        /// <param name="eTType">移除事件触发类型</param>
        /// <param name="callBack">移除的回调</param>
        public static void UIEventRemove(UIBehaviour uiB, EventTriggerType eTType, UnityAction<BaseEventData> callBack)
        {
            if (uiB == null)
            {
                Debuger.Log("申请对象为NULL");
                return;
            }
            if (callBack == null)
            {
                Debuger.Log("回调为NULL");
                return;
            }
            //获取UI对象身下的EventTrigger组件实例
            if(!uiB.gameObject.TryGetComponent(out EventTrigger eT))
            {
                return;
            }
            //遍历已添加的监听事件
            foreach (var t in eT.triggers)
            {
                //找到对应事件类型
                if (t.eventID == eTType)
                {
                    //移除该事件类型下的回调
                    t.callback.RemoveListener(callBack);
                    break;//跳出foreach
                }
            }
        }
        #endregion
    }
}
