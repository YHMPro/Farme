using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
namespace Farme.UI
{
    [Serializable]
    /// <summary>
    /// 富有弹性的按钮
    /// </summary>
    public class ElasticBtn : Button
    {
        /// <summary>
        /// 文本
        /// </summary>
        private Text m_Text = null;
        /// <summary>
        /// 文本
        /// </summary>
        public Text Text => m_Text;
        /// <summary>
        /// 矩形转换
        /// </summary>
        private RectTransform m_RectTransform;
        /// <summary>
        /// 按钮背景
        /// </summary>
        private Image m_Bg = null;
        /// <summary>
        /// 按钮背景
        /// </summary>
        public Image Bg => m_Bg;
        /// <summary>
        /// 原生缩放
        /// </summary>
        private Vector3 m_NativeScale = Vector3.one;
        /// <summary>
        /// 外接圆半径
        /// </summary>
        private float m_CircumcircleRadius = 0;
        /// <summary>
        /// 
        /// </summary>
        private float m_ScaleValue = 0.2f;
        /// <summary>
        /// 所依赖画布的渲染模式
        /// </summary>
        private RenderMode m_RelyCanvasRenderMode = RenderMode.ScreenSpaceOverlay;

        protected override void Awake()
        {      
            base.Awake();    
            m_Text=GetComponentInChildren<Text>(true);
            m_Bg = GetComponent<Image>();
            m_NativeScale = transform.localScale;
            m_RectTransform = transform as RectTransform;
        }

        protected override void Start()
        {
            base.Start();
            m_RelyCanvasRenderMode = GetComponentInParent<Canvas>().renderMode;
            m_CircumcircleRadius = Mathf.Sqrt(Mathf.Pow(m_RectTransform.rect.width / 2.0f * transform.localScale.x, 2) + Mathf.Pow(m_RectTransform.rect.height / 2.0f * transform.localScale.y, 2));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (ShareMono.Exists)
            {
                ShareMono.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, this.ScaleUpdate);
            }
            transform.localScale = m_NativeScale;
            
        }
        private void ScaleUpdate()
        {
            Vector2 toPoint;//自身指向指针的方向向量
            Vector2 pos;//toPoint的第一次被赋值后为:向量与UI矩形框外接圆的交点坐标 toPoint的第二次被赋值后为:向量与UI矩形框的交点坐标
            float angle;//toPoint(单位化后)向量与单位向量Vector2.right的夹角
            float k;//toPoint向量所构成的直线的斜率
            toPoint = (m_RelyCanvasRenderMode == RenderMode.ScreenSpaceOverlay) ?
                           (Vector2)(Input.mousePosition - transform.position) : (RectTransformUtility.WorldToScreenPoint(WindowRoot.Exists ?
                                                                                                                    WindowRoot.GetSingleton().Camera : Camera.main, transform.position) - (Vector2)Input.mousePosition);
            angle = Vector2.SignedAngle(Vector2.right, toPoint.normalized);
            pos.x = Mathf.Cos(angle * Mathf.Deg2Rad) * m_CircumcircleRadius;
            pos.y = Mathf.Sin(angle * Mathf.Deg2Rad) * m_CircumcircleRadius;
            k = pos.y / pos.x;
            if (Mathf.Abs(pos.x) > m_RectTransform.rect.width / 2f)
            {
                pos.x = pos.x / Mathf.Abs(pos.x) * m_RectTransform.rect.width / 2f * m_NativeScale.x;
                pos.y = k * pos.x;
            }
            if (Mathf.Abs(pos.y) > Bg.rectTransform.rect.height / 2f)
            {
                pos.y = pos.y / Mathf.Abs(pos.y) * m_RectTransform.rect.height / 2f * m_NativeScale.y;
                pos.x = pos.y / k;
            }
            transform.localScale = m_NativeScale - (1f - toPoint.magnitude / pos.magnitude) * Vector3.one * m_ScaleValue;
        }
        #region OnPointerEvent
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            ShareMono.GetSingleton().ApplyUpdateAction(EnumUpdateAction.Standard, this.ScaleUpdate);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            ShareMono.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, this.ScaleUpdate);
            transform.localScale = m_NativeScale;
        }
        #endregion   
    }
}
