using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using Farme.Extend;
using System;
namespace Farme.UI
{
    /// <summary>
    /// 富有弹性的按钮
    /// </summary>
    public class ElasticBtn : Button
    {       
        /// <summary>
        /// 
        /// </summary>
        private Image m_Img;
        /// <summary>
        /// 与指针的距离
        /// </summary>
        private float m_ToPointerDistance = 0;
        [Range(-0.5f, 0.5f)]
        [SerializeField]
        private float m_ScaleValue = 0.2f;
        /// <summary>
        /// 所依赖画布的渲染模式
        /// </summary>
        private RenderMode m_RelyCanvasRenderMode = RenderMode.ScreenSpaceOverlay;
        protected override void Awake()
        {
            base.Awake();
            m_Img = GetComponent<Image>();           
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            m_RelyCanvasRenderMode=GetComponentInParent<Canvas>().renderMode;
        }
        private void ScaleUpdate()
        {
            if(m_RelyCanvasRenderMode== RenderMode.ScreenSpaceOverlay)
            {
                //计算指针的位置与自身的距离
                m_ToPointerDistance = Mathf.Clamp((transform.position - Input.mousePosition).magnitude, 0, (m_Img.rectTransform.rect.width * m_Img.transform.lossyScale.x) / 2f - 3f);
                //调控放缩量
                transform.localScale = Vector3.one - (1f - m_ToPointerDistance / ((m_Img.rectTransform.rect.width * m_Img.transform.lossyScale.x) / 2f - 3f)) * Vector3.one * m_ScaleValue;
                return;
            }
            if (!MonoSingletonFactory<WindowRoot>.SingletonExist)
            {
                //计算指针的位置与自身的距离
                m_ToPointerDistance = Mathf.Clamp(((Vector3)RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position) - Input.mousePosition).magnitude, 0, (m_Img.rectTransform.rect.width * m_Img.transform.localScale.x) / 2f - 3f);
                //调控放缩量
                transform.localScale = Vector3.one - (1f - m_ToPointerDistance / ((m_Img.rectTransform.rect.width * m_Img.transform.localScale.x) / 2f - 3f)) * Vector3.one * m_ScaleValue;
                return;
            }
            //计算指针的位置与自身的距离
            m_ToPointerDistance = Mathf.Clamp(((Vector3)RectTransformUtility.WorldToScreenPoint(MonoSingletonFactory<WindowRoot>.GetSingleton().Camera, transform.position) - Input.mousePosition).magnitude, 0, (m_Img.rectTransform.rect.width * m_Img.transform.localScale.x) / 2f - 3f);
            //调控放缩量
            transform.localScale = Vector3.one - (1f - m_ToPointerDistance / ((m_Img.rectTransform.rect.width * m_Img.transform.localScale.x) / 2f - 3f)) * Vector3.one * m_ScaleValue;          
        }
        #region OnPointerEvent
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            MonoSingletonFactory<ShareMono>.GetSingleton().ApplyUpdateAction(EnumUpdateAction.Standard, this.ScaleUpdate);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            MonoSingletonFactory<ShareMono>.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, this.ScaleUpdate);
            transform.localScale = Vector3.one;
        }
        #endregion   
        protected override void OnDestroy()
        {
            if (MonoSingletonFactory<ShareMono>.SingletonExist)
            {
                MonoSingletonFactory<ShareMono>.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, this.ScaleUpdate);
            }
            base.OnDestroy();
        }        
    }
}
