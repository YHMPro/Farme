using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Farme.UI
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class FoldContentRect : BaseUI
    {
        private VerticalLayoutGroup m_VGroup;
        protected override void Awake()
        {
            base.Awake();
            m_VGroup = GetComponent<VerticalLayoutGroup>();
        }

        protected override void Start()
        {
            base.Start();

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
        /// <summary>
        /// 刷新UI
        /// </summary>
        public override void RefreshUI()
        {
            float childHeightTotal = 0;
            float maxWidth = 0;
            RectTransform childRectTransform;
            for(int index=0;index<m_RectTransform.childCount;index++)
            {
                childRectTransform = m_RectTransform.GetChild(index) as RectTransform;
                if(!childRectTransform.gameObject.activeInHierarchy)
                {
                    continue;
                }
                maxWidth = (maxWidth < childRectTransform.rect.width * childRectTransform.localScale.x) ? (childRectTransform.rect.width * childRectTransform.localScale.x) : maxWidth;
                childHeightTotal += (childRectTransform.rect.height+m_VGroup.spacing)* childRectTransform.localScale.y;            
            }
            m_RectTransform.sizeDelta=new Vector2 (maxWidth, childHeightTotal+ m_VGroup.padding.top+m_VGroup.padding.bottom);
            #region 刷新父级UI  (注:只对含FoldFarmeRect组件的父级有效) 逆向刷新UI         
            FoldFarmeRect foldFarmeRect =GetComponentInParent<FoldFarmeRect>();
            if (foldFarmeRect == null)
            {
                return;
            }
            foldFarmeRect.RefreshUI();
            #endregion
        }
    }
}
