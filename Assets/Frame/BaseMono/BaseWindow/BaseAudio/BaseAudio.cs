using UnityEngine;
using UnityEngine.Audio;
using System;
namespace Farme
{
    [Obsolete("已更替为Audio统一使用,未来将会删除")]
    [RequireComponent(typeof(AudioSource))]
    /// <summary>
    /// Audio基类
    /// </summary>
    public abstract class BaseAudio : BaseMono, IAudioControl
    {
        
        protected BaseAudio() { }

        #region 生命周期函数
        
        protected override void Awake()
        {
            base.Awake();
            m_As = GetComponent<AudioSource>();
            m_As.panStereo = 0;
            m_As.time = 0;
            m_As.clip = null;
            m_As.playOnAwake = false;
            m_As.loop = false;
            m_Ac = null;
            m_Loop = false;
            m_IsPause = false;
            m_IsStop = true;
            m_IsPlay = false;
            m_Volume = 1;
            m_PanStereo = 0;
            m_AbleRecycle = true;
            m_Timer = null;
            GetRelyOnAudioMgr().NotInidleAudioControlLi.Add(Control);
        }
        #endregion

        #region 字段         
        [SerializeField]
        /// <summary>
        /// 音效源
        /// </summary>
        protected AudioSource m_As = null;
        [SerializeField]
        /// <summary>
        /// 音效
        /// </summary>
        private AudioClip m_Ac = null;
        [SerializeField]
        /// <summary>
        /// 绑定的音效混合器组
        /// </summary>
        private AudioMixerGroup m_BindAudioMixerGroup = null;
        /// <summary>
        /// 是否播放
        /// </summary>
        private bool m_IsPlay = false;
        /// <summary>
        /// 是否停止
        /// </summary>
        private bool m_IsStop = true;
        /// <summary>
        /// 是否暂停
        /// </summary>
        private bool m_IsPause = true;
        /// <summary>
        /// 是否循环
        /// </summary>
        private bool m_Loop = false;
        /// <summary>
        /// 是否可以回收利用(默认为可回收) 为True时{非循环模式下:停止播放或完成播放将会回收该音效播放器}
        /// </summary>
        private bool m_AbleRecycle = true;
        /// <summary>
        /// 音量
        /// </summary>
        private float m_Volume = 1;
        /// <summary>
        /// 左声道->右声道的渐变  -1(左)->0(立体)->1(右)
        /// </summary>
        private float m_PanStereo = 0;
        /// <summary>
        /// 已播放的时长
        /// </summary>
        private float m_PlayedTime = 0;
        /// <summary>
        /// 附属计时器
        /// </summary>
        private Coroutine m_Timer;
        #endregion

        #region 属性
        public bool AbleRecycle
        {
            set
            {
                if(m_AbleRecycle!=value)
                {
                    m_AbleRecycle = value;
                }
            }
            get
            {
                return m_AbleRecycle;
            }
        }      
        /// <summary>
        /// 是否绑定音效混合器
        /// </summary>
        public bool IsBindAudioMixer
        {
            get
            {
                return m_BindAudioMixerGroup == null ? false : true;
            }
        }
        /// <summary>
        /// 音效控制接口
        /// </summary>
        public IAudioControl Control
        {
            get
            {
                return this;
            }
        }
        /// <summary>
        /// 左声道->右声道的渐变  -1(左)->0(立体)->1(右)
        /// </summary>
        public float StereoPan
        {
            set
            {
                m_PanStereo = Mathf.Clamp(value, -1, 1);
                if (m_As != null)
                {
                    m_As.panStereo = m_PanStereo;
                }

            }
            get
            {
                return m_PanStereo;
            }
        }
        /// <summary>
        /// 音量  
        /// </summary>
        public float Volume
        {
            set
            {
                m_Volume = Mathf.Clamp(value, 0, 1);
                if (m_As != null)
                {
                    m_As.volume = m_Volume;
                }
            }
            get
            {
                return m_Volume;
            }
        }
        /// <summary>
        /// 音效时长
        /// </summary>
        public float Length
        {
            get
            {
                if (m_Ac != null)
                {
                    return m_Ac.length;
                }
                return 0;
            }
        }
        /// <summary>
        /// 音效ID
        /// </summary>
        public int ID
        {
            get
            {
                if (m_As != null)
                {
                    return m_As.GetInstanceID();
                }
                return -1;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 附加计时器
        /// </summary>
        private void AppendTimer()
        {
            if (m_Timer == null && m_As != null && m_Ac != null)
            {
                m_Timer = MonoSingletonFactory<ShareMono>.GetSingleton().DelayAction(m_Ac.length - m_As.time, () =>//延迟回收时长为调用时音效剪辑总时长-已播放时长
                {
                    m_As.Stop();//停止播放
                    m_IsPause = false;//非暂停
                    m_IsPlay = false;//非播放
                    m_As.time = 0;//重置播放时长        
                    m_IsStop = true;//停止                                                              
                    MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(m_Timer);//停止协程
                    m_Timer = null;//重置为NULL
                    if (m_AbleRecycle)
                    {
                        GetRelyOnAudioMgr().InidleWithNotInidleTransform(this);//置换     
                    }
                });
            }
        }

        /// <summary>
        /// 移除计时器
        /// </summary>
        private void RemoveTimer()
        {
            if (m_Timer != null)
            {
                MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(m_Timer);//停止协程
            }
        }

        /// <summary>
        /// 获取自身依赖的音效管理器
        /// </summary>
        /// <returns></returns>
        protected abstract BaseAudioMgr GetRelyOnAudioMgr();

        /// <summary>
        /// 音效播放
        /// </summary>
        /// <returns>是否播放成功</returns>
        protected virtual bool AudioPlay()
        {
            if (m_As != null && !m_IsPlay)
            {
                m_As.Play();
                if (!m_Loop)//非循环
                {
                    AppendTimer();//添加回收器
                }
                m_IsPlay = true;//播放
                m_IsPause = false;//非暂停
                m_IsStop = false;//非停止
                return true;
            }
            return false;
        }

        /// <summary>
        /// 音效暂停
        /// </summary>
        /// <returns>是否暂停成功</returns>
        protected virtual bool AudioPause()
        {
            if (m_As != null && !m_IsPause)
            {
                m_As.Pause();
                if (!m_Loop)//非循环
                {
                    RemoveTimer();//移除回收器
                }
                m_IsPlay = false;//非播放
                m_IsStop = false;//非停止
                m_IsPause = true;//暂停
                return true;
            }
            return false;
        }

        /// <summary>
        /// 音效停止
        /// </summary>
        /// <returns>是否停止成功</returns>
        protected virtual bool AudioStop()
        {
            if (m_As != null && !m_IsStop)
            {
                m_As.Stop();//停止播放
                if (!m_Loop)//非循环
                {
                    RemoveTimer();
                }
                m_As.time = 0;//重置播放时长
                m_IsPause = false;//非暂停
                m_IsPlay = false;//非播放           
                m_IsStop = true;//停止
                if (m_AbleRecycle)
                {
                    GetRelyOnAudioMgr().InidleWithNotInidleTransform(this);//置换
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 音效重播
        /// </summary>
        /// <returns>是否重播成功</returns>
        protected virtual bool AudioReplay()
        {
            if (m_As != null && !m_IsStop)
            {
                m_As.time = 0;//起始播放时长归零
                m_As.Play();//播放
                if (!m_As.loop)//非循环
                {
                    RemoveTimer();
                    AppendTimer();
                }
                m_IsPlay = true;//播放
                m_IsPause = false;//非暂停
                m_IsStop = false;//非停止
                return true;
            }
            return false;
        }
        public float GetPlayedTime()
        {
            return m_PlayedTime;
        }

        public bool GetAudioMixerGroup(out AudioMixerGroup group)
        {
            if (IsBindAudioMixer)
            {
                group = m_BindAudioMixerGroup;
                return true;
            }
            group = null;
            return false;
        }

        public AudioClip GetAudioClip()
        {
            return m_Ac;
        }

        public bool IsPause()
        {
            return m_IsPause;
        }

        public bool IsStop()
        {
            return m_IsStop;
        }

        public bool IsPlay()
        {
            return m_IsPlay;
        }

        public bool IsLoop()
        {
            return m_Loop;
        }

        public bool Play()
        {
            return AudioPlay();
        }

        public bool Pause()
        {
            return AudioPause();
        }

        public bool RePlay()
        {
            return AudioReplay();
        }

        public bool Stop()
        {
            return AudioStop();
        }

        public bool SetLoop(bool loop)
        {
            if (m_As != null)
            {
                if (m_Loop != loop)
                {
                    if (m_AbleRecycle)
                    {
                        if (loop)
                        {
                            RemoveTimer();    
                        }
                        else
                        {

                            AppendTimer();
                        }
                    }
                    m_As.loop = loop;
                    m_Loop = loop;
                }
                return true;
            }
            return false;
        }

        public bool SetAudioMixerGroup(AudioMixerGroup group)
        {
            if (m_As != null)
            {
                if (m_BindAudioMixerGroup != group)
                {
                    m_As.outputAudioMixerGroup = group;
                    m_BindAudioMixerGroup = group;
                }
                return true;
            }
            return false;
        }

        public bool SetPlayedTime(float playedTime)
        {
            if (m_As != null)
            {
                m_As.time = playedTime;
                m_PlayedTime = playedTime;
                if (m_AbleRecycle)
                {
                    if (!m_Loop)
                    {
                        RemoveTimer();
                        AppendTimer();
                    }
                }
                return true;
            }
            return false;
        }

        public bool SetAudioClip(AudioClip clip, bool inheritTime = false)
        {
            if (m_As != null)
            {
                if (AudioPause())
                {
                    if (m_Ac != clip)
                    {
                        m_Ac = clip;
                        m_As.clip = clip;
                        if (clip != null)
                        {
                            if (inheritTime)
                            {
                                m_As.time = Mathf.Clamp(m_PlayedTime, 0, m_Ac.length);
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
