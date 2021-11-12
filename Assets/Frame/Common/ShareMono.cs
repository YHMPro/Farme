using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace Farme
{ 
    /// <summary>
    /// 共享Mono 用于非MonoBehaviour派生类提供MonoBehaviour功能
    /// </summary>
    public class ShareMono : MonoBehaviour
    {
        protected ShareMono() { }
        #region 事件
        /// <summary>
        /// 回调
        /// </summary>
        private event UnityAction m_Callback;
        /// <summary>
        /// 回调
        /// </summary>
        private event UnityAction m_LateCallback;
        /// <summary>
        /// 回调
        /// </summary>
        private event UnityAction m_FixCallback;
        #endregion
        #region 方法      
        /// <summary>
        /// 持续更新
        /// </summary>
        private void Update()
        {
            m_Callback?.Invoke();
        }
        /// <summary>
        /// 迟与Update更新
        /// </summary>
        private void LateUpdate()
        {
            m_LateCallback?.Invoke();
        }
        /// <summary>
        /// 固定更新
        /// </summary>
        private void FixedUpdate()
        {
            m_FixCallback?.Invoke();
        }
        /// <summary>
        /// 申请Update行为
        /// </summary>
        /// <param name="callback"></param>
        public void ApplyUpdateAction(UnityAction callback)
        {
            m_Callback += callback;
        }
        /// <summary>
        /// 申请LateUpdate行为
        /// </summary>
        /// <param name="callback">回调</param>
        public void AppleLateUpdateAction(UnityAction callback)
        {
            m_LateCallback += callback;
        }
        /// <summary>
        /// 申请FixUpdate行为
        /// </summary>
        /// <param name="callback">回调</param>
        public void ApplyFixUpdateAction(UnityAction callback)
        {
            m_LateCallback += callback;
        }       
        /// <summary>
        /// 移除Update行为
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveUpdateAction(UnityAction callback)
        {
            m_Callback -= callback;
        }
        /// <summary>
        /// 移除LateUpdate行为
        /// </summary>
        /// <param name="callBack">回调</param>
        public void RemoveLateUpdateAction(UnityAction callback)
        {
            m_LateCallback -= callback;
        }
        
        /// <summary>
        /// 移除FixUpdate行为
        /// </summary>
        /// <param name="callBack">回调</param>
        public void RemoveFixUpdateAction(UnityAction callback)
        {
            m_FixCallback -= callback;
        }
        
        /// <summary>
        /// 延迟执行(无参数传递) 
        /// </summary>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        public Coroutine DelayAction(float delayTime, UnityAction callback)
        {
            return StartCoroutine(IEDelayAction(Mathf.Clamp(delayTime, 0, delayTime), callback));
        }
        /// <summary>
        /// 延迟执行(含参数传递) 
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="tInfo">信息</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        public Coroutine DelayAction<T>(float delayTime, T tInfo, UnityAction<T> callback)
        {
            return StartCoroutine(IEDelayAction(Mathf.Clamp(delayTime, 0, delayTime), tInfo, callback));
        }
        /// <summary>
        /// 延迟执行(含参数传递) 
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <typeparam name="K">参数类型</typeparam>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="tInfo">信息</param>
        /// <param name="kInfo">信息</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        public Coroutine DelayAction<T, K>(float delayTime, T tInfo, K kInfo, UnityAction<T, K> callback)
        {
            return StartCoroutine(IEDelayAction(Mathf.Clamp(delayTime, 0, delayTime), tInfo, kInfo, callback));
        }
        /// <summary>
        /// 协程延迟(无参数传递)
        /// </summary>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        private IEnumerator IEDelayAction(float delayTime, UnityAction callback)
        {
            yield return new WaitForSeconds(delayTime);
            callback?.Invoke();
        }
        /// <summary>
        /// 协程延迟(含参数传递)
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="tInfo">信息</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        private IEnumerator IEDelayAction<T>(float delayTime, T tInfo, UnityAction<T> callback)
        {
            yield return new WaitForSeconds(delayTime);
            callback?.Invoke(tInfo);
        }
        /// <summary>
        /// 协程延迟(含参数传递)
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <typeparam name="K">参数类型</typeparam>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="tInfo">信息</param>
        /// <param name="kInfo">信息</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        private IEnumerator IEDelayAction<T, K>(float delayTime, T tInfo, K kInfo, UnityAction<T, K> callback)
        {
            yield return new WaitForSeconds(delayTime);
            callback?.Invoke(tInfo, kInfo);
        }
        /// <summary>
        /// 移除所有Update委托
        /// </summary>
        public void ClearUpdate()
        {
            m_Callback = null;
        }
        /// <summary>
        /// 移除所有FixUpdate委托
        /// </summary>
        public void ClearFixedUpdate()
        {
            m_FixCallback = null;
        }
        /// <summary>
        /// 移除所有LateUpdate委托
        /// </summary>
        public void ClearLateUpdate()
        {
            m_LateCallback = null;
        }
        #endregion
    }
}
