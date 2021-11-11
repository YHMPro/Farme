using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Farme.Audio
{
    /// <summary>
    /// 音效管理器
    /// </summary>
    public class AudioManager 
    {
        #region 字段
        /// <summary>
        /// 闲置的音效
        /// </summary>
        private static List<Audio> m_InidleAudioLi = null;
        /// <summary>
        /// 非闲置的音效
        /// </summary>
        private static List<Audio> m_NotInidleAudioLi = null;
        #endregion

        #region 属性
        /// <summary>
        /// 非闲置的音效列表
        /// </summary>
        public static List<Audio> NotInidleAudioLi
        {
            get
            {
                if(m_NotInidleAudioLi==null)
                {
                    m_NotInidleAudioLi = new List<Audio>();
                }
                return m_NotInidleAudioLi;
            }
        }
        /// <summary>
        /// 闲置的音效列表
        /// </summary>
        public static List<Audio> InidleAudioLi
        {
            get
            {
                if(m_InidleAudioLi==null)
                {
                    m_InidleAudioLi = new List<Audio>();
                }
                return m_InidleAudioLi;
            }
        }
        #endregion


        

    }
}
