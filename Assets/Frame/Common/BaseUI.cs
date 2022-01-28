using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Farme.UI
{
    public abstract class BaseUI : BaseMono,IRefreshUI
    {
        protected RectTransform m_RectTransform;
        public RectTransform RectTransform
        {
            get { return m_RectTransform; }
        }

        protected override void Awake()
        {
            base.Awake();
            m_RectTransform = transform as RectTransform;
        }

        public abstract void RefreshUI();     
    }
}
