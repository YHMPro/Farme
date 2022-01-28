using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
namespace Farme.UI
{
    /// <summary>
    /// 折叠矩形框
    /// </summary>
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class FoldFarmeRect : BaseUI
    {           
        private VerticalLayoutGroup m_VGroup;
        /// <summary>
        /// 是否折叠
        /// </summary>
        [SerializeField]
        private bool m_IsFold;
        /// <summary>
        /// 是否折叠
        /// </summary>
        public bool IsFold
        {
            get { return m_IsFold; }
            set 
            { 
                m_IsFold = value;
                RefreshUI();
            }
        }
        /// <summary>
        /// 折叠按钮
        /// </summary>
        [SerializeField]
        private ElasticBtn m_FoldBtn;
        /// <summary>
        /// 折叠内容矩形
        /// </summary>
        [SerializeField]
        private FoldContentRect m_FoldContentRect;
        /// <summary>
        /// 折叠事件
        /// </summary>
        [SerializeField]
        private FoldEvent m_FE = new FoldEvent();
        /// <summary>
        /// 折叠事件
        /// </summary>
        public FoldEvent FE
        {
            get 
            {                  
                return m_FE;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            m_VGroup=GetComponent<VerticalLayoutGroup>();
        }

        protected override void Start()
        {
            base.Start();
            m_FoldBtn.onClick.AddListener(OnFoldClick);
            #region 提高刷新的效率临时方案
            //判定自身子集(一级)是否含有继承IRefreshUI的类(暂时用这种方案来优化首次刷新的效率,后续会进行进一步优化)
            if (transform.childCount == 0)
            {
                return;
            }
            for (int index = 0; index < transform.childCount; index++)
            {
                if (transform.GetChild(index).GetComponent<IRefreshUI>() != null)
                {
                    return;
                }
            }
            RefreshUI();
            #endregion   
        }

        #region Button
        /// <summary>
        /// 监听折叠按钮点击事件
        /// </summary>
        private void OnFoldClick()
        {
            m_IsFold = !m_IsFold;       
            RefreshUI();
            //刷新折叠矩形框的内容
            m_FoldContentRect.RefreshUI();
        }
        #endregion
        /// <summary>
        /// 折叠矩形框刷新
        /// </summary>
        public override void RefreshUI()
        {
            m_FE?.Invoke(m_IsFold);        
            float childHeightTotal = 0;
            float maxWidth = 0;
            m_FoldContentRect.gameObject.SetActive(!m_IsFold);
            RectTransform childRectTransform;
            for (int index = 0; index < m_RectTransform.childCount; index++)
            {
                childRectTransform = m_RectTransform.GetChild(index) as RectTransform;
                if (!childRectTransform.gameObject.activeInHierarchy)
                {
                    continue;
                }
                maxWidth = (maxWidth < childRectTransform.rect.width * childRectTransform.localScale.x) ? (childRectTransform.rect.width * childRectTransform.localScale.x) : maxWidth;
                childHeightTotal += (childRectTransform.rect.height+ m_VGroup.spacing) * childRectTransform.localScale.y;
            }          
            m_RectTransform.sizeDelta = new Vector2(maxWidth, childHeightTotal+ m_VGroup.padding.top+ m_VGroup.padding.bottom);

            #region 刷新父级UI  (注:只对含FoldContentRect组件的父级有效)  逆向刷新UI           
            FoldContentRect foldContentRect = GetComponentInParent<FoldContentRect>();
            if(foldContentRect==null)
            {
                return;
            }
            foldContentRect.RefreshUI();
            #endregion
        }
        [Serializable]
        public class FoldEvent : UnityEvent<bool>
        {
            
        }
    }
}
