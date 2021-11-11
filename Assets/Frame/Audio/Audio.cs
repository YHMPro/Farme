﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Farme.Tool;
using UnityEngine.Events;
namespace Farme.Audio
{
    [RequireComponent(typeof(AudioSource))]
    /// <summary>
    /// 音效
    /// </summary>
    public class Audio : BaseMono
    {
        #region 生命周期函数
        protected override void Awake()
        {
            base.Awake();
            m_As = GetComponent<AudioSource>();           
        }

        protected override void Start()
        {
            base.Start();
            m_As.panStereo = 0;
            m_As.time = 0;
            m_As.clip = null;
            m_As.playOnAwake = false;
            m_As.loop = false;
            m_AbleRecycle = true;
            m_Timer = null;
            m_ListenVolumeExcess = null;
            AudioManager.NotInidleAudioLi.Add(this);
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
        /// 音效混合器组
        /// </summary>
        private AudioMixerGroup m_AudioMixerGroup = null;
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
        /// 是否能够回收(如果为False,则将永远不能回收到音效管理器当中)
        /// </summary>
        private bool m_AbleRecycle = true;
        /// <summary>
        /// 附属计时器
        /// </summary>
        private Coroutine m_Timer = null;
        /// <summary>
        /// 监听音量过度
        /// </summary>
        private Coroutine m_ListenVolumeExcess = null;
        #endregion

        #region 属性 
        /// <summary>
        /// 音效剪辑
        /// </summary>
        public AudioClip Clip
        {
            set
            {
                m_As.clip = value;
                Pause();
            }
            get
            {
                return m_As.clip;
            }
        }
        /// <summary>
        /// 是否循环
        /// </summary>
        public bool Loop
        {
            set
            {               
                m_As.loop = value;
            }
            get
            {
                return m_As.loop;
            }
        }
        /// <summary>
        /// 是否能够回收(如果为False,则将永远不能回收到音效管理器当中)
        /// </summary>
        public bool AbleRecycle
        {
            set
            {
                m_AbleRecycle = value;
            }
            get
            {
                return m_AbleRecycle;
            }
        }         
        /// <summary>
        /// 左声道->右声道的渐变  -1(左)->0(立体)->1(右)
        /// </summary>
        public float StereoPan
        {
            set
            {
                m_As.panStereo = Mathf.Clamp(value, -1, 1);
            }
            get
            {
                return m_As.panStereo;
            }
        }
        /// <summary>
        /// 已播放的时长
        /// </summary>
        public float Time
        {
            set
            {
                m_As.time = value;
                if (m_AbleRecycle)
                {
                    if (!m_As.loop)
                    {
                        RemoveTimer();
                        AppendTimer();
                    }
                }
            }
            get
            {
                return m_As.time;
            }
        }
        /// <summary>
        /// 内置音量(不同于AudioMixerGroup中的音量)  
        /// </summary>
        public float Volume
        {
            set
            {
                m_As.volume = Mathf.Clamp(value, 0, 1);
            }
            get
            {
                return m_As.volume;
            }
        }
        /// <summary>
        /// 音效时长
        /// </summary>
        public float Length
        {
            get
            {
                if (m_As.clip != null)
                {
                    return m_As.clip.length;
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
        /// 添加计时器
        /// </summary>
        private void AppendTimer()
        {
            if(m_Timer == null&& m_As.clip!=null)
            {
                m_Timer = MonoSingletonFactory<ShareMono>.GetSingleton().DelayUAction(m_As.clip.length - m_As.time, () =>
                {
                    RemoveTimer();
                    if (m_As.loop)
                    {
                        return;
                    }
                    m_IsPause = false;
                    m_IsPlay = false;
                    m_As.time = 0;   
                    m_IsStop = true;
                    if (m_AbleRecycle)
                    {
                        InidleWithNotInidleTransform(this);
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
                m_Timer = null;
            }
        }
        #endregion

        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            m_As.Play();
            AppendTimer();
            m_IsPause = false;
            m_IsPlay = true;
            m_IsStop = false;                
        }
        /// <summary>
        /// 播放(先播放，然后音量过度到目标值)
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="time"></param>
        public void Play(float volume,float time)
        {
            if (m_ListenVolumeExcess != null)
            {
                MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(m_ListenVolumeExcess);
            }
            Play();
            VolumeExcess(volume, time);
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            m_As.Pause();
            RemoveTimer();
            m_IsPause = true;
            m_IsPlay = false;
            m_IsStop = false;
        }
        /// <summary>
        /// 暂停(等待音量过度到目标值，然后暂停)
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="time"></param>
        public void Pause(float volume, float time)
        {
            if (m_ListenVolumeExcess != null)
            {
                MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(m_ListenVolumeExcess);
            }
            VolumeExcess(volume, time,()=> 
            {
                Pause();
            });
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            m_As.Stop();
            RemoveTimer();
            m_IsPause = false;
            m_IsPlay = false;
            m_IsStop = true;
            m_As.time = 0;
            if (m_AbleRecycle)
            {
                InidleWithNotInidleTransform(this);
            }
        }
        /// <summary>
        /// 停止(等待音量过度到目标值，然后停止)
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="time"></param>
        public void Stop(float volume, float time)
        {
            if (m_ListenVolumeExcess != null)
            {
                MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(m_ListenVolumeExcess);
            }
            VolumeExcess(volume, time, () =>
            {
                Stop();
            });

        }
        /// <summary>
        /// 音量过度
        /// </summary>
        /// <param name="volume">目标音量</param>
        /// <param name="time">耗时</param>
        /// <param name="finishCallback">完成回调</param>
        private void VolumeExcess(float volume,float time,UnityAction finishCallback=null)
        {
            m_ListenVolumeExcess = MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEVolumeExcess(volume,Mathf.Clamp(time,0,m_As.clip.length-m_As.time), finishCallback));        
        }
        private IEnumerator IEVolumeExcess(float volume,float time, UnityAction finishCallback=null)
        {
            int num = (int)(time / 0.02f);
            float interval = volume / num;
            while (true)
            {            
                if(num<=0)
                {
                    m_ListenVolumeExcess = null;
                    finishCallback?.Invoke();
                    break;
                }
                m_As.volume = Mathf.MoveTowards(m_As.volume, volume,interval);
                num--;
                yield return new WaitForSeconds(0.02f);
            }
        }
        /// <summary>
        /// 闲置与非闲置的置换
        /// </summary>
        /// <param name="audio">音效</param>
        private void InidleWithNotInidleTransform(Audio audio)
        {
            if (AudioManager.InidleAudioLi.Contains(audio))
            {
                AudioManager.InidleAudioLi.Remove(audio);
                AudioManager.NotInidleAudioLi.Add(audio);
                return;
            }
            if (AudioManager.NotInidleAudioLi.Contains(audio))
            {
                AudioManager.NotInidleAudioLi.Remove(audio);
                AudioManager.InidleAudioLi.Add(audio);
                return;
            }
        }
    }
}