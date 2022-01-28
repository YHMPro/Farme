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
        protected override void Awake()
        {
            base.Awake();
            m_Img = GetComponent<Image>();
        }

        private void ScaleUpdate()
        {
            //计算指针的位置与自身的距离
            m_ToPointerDistance = Mathf.Clamp((transform.position - Input.mousePosition).magnitude, 0, (m_Img.rectTransform.rect.width * m_Img.transform.lossyScale.x) / 2f - 3f);
            //调控放缩量
            transform.localScale = Vector3.one - (1f - m_ToPointerDistance / ((m_Img.rectTransform.rect.width * m_Img.transform.lossyScale.x) / 2f - 3f)) * Vector3.one * m_ScaleValue;
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
            base.OnDestroy();
            if (!Application.isEditor)
            {
                MonoSingletonFactory<ShareMono>.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, this.ScaleUpdate);
            }

        }        
    }
}
