using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
        /// <summary>
        /// 缓存的音效播放器最大数量
        /// </summary>
        private static int m_cacheAudioMax = 5;      
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
        /// <summary>
        /// 缓存的最大音效数量
        /// </summary>
        public static int CacheAudioMax
        {
            set
            {
                m_cacheAudioMax = value;
            }
            get
            {
                return m_cacheAudioMax;
            }
        }
        /// <summary>
        /// 主音量
        /// </summary>
        public static float MainVolume
        {
            set
            {
                AudioMixerMgr.MainAudioMixer.SetFloat("MasterVolume", Mathf.Clamp(value, 0, 1));
                SetVolume("Master", "MasterVolume", Mathf.Clamp(value,0,1));       
            }
            get
            {
                AudioMixerMgr.MainAudioMixer.GetFloat("MasterVolume", out float volume);
                return volume;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void ClearCache()
        {
            while(m_InidleAudioLi.Count > m_cacheAudioMax)
            {              
                Audio audio = m_InidleAudioLi[m_InidleAudioLi.Count - 1];
                if (audio != null)
                {
                    m_InidleAudioLi.Remove(audio);
                    Object.Destroy(audio.gameObject);
                }
            }
        }
        /// <summary>
        /// 申请音效
        /// </summary>
        /// <returns></returns>
        public static Audio ApplyForAudio()
        {
            Audio audio;
            if (m_InidleAudioLi.Count > 0)
            {
                audio = m_InidleAudioLi[0];
                m_InidleAudioLi.Remove(audio);
                if(!m_NotInidleAudioLi.Contains(audio))
                {
                    m_NotInidleAudioLi.Add(audio);
                }                
            }
            else
            {
                audio = MonoFactory<Audio>.GetInstance(new GameObject("Audio"));             
            }
            return audio;
        }
        /// <summary>
        /// 全局暂停
        /// </summary>
        public static void GlobalPause()
        {
            for (int index = m_NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = m_NotInidleAudioLi[index];
                if (audio != null)
                {
                    audio.Pause();
                }
            }
        }
        /// <summary>
        /// 全局停止
        /// </summary>
        public static void GlobalStop()
        {
            for (int index = m_NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = m_NotInidleAudioLi[index];
                if (audio != null)
                {
                    audio.Pause();
                }
            }
        }
        /// <summary>
        /// 全局播放
        /// </summary>
        public static void GlobalPlay()
        {
            for (int index = m_NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = m_NotInidleAudioLi[index];
                if (audio != null)
                {
                    audio.Pause();
                }
            }
        }
        /// <summary>
        /// 暂停音效
        /// </summary>
        /// <param name="audioMixerGroup">音效混合组</param>
        public static void Pause(AudioMixerGroup audioMixerGroup)
        {
            for (int index = m_NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = m_NotInidleAudioLi[index];
                if (audio != null && audio.Group == audioMixerGroup)
                {
                    audio.Pause();
                }
            }
        }
        /// <summary>
        /// 停止音效
        /// </summary>
        /// <param name="audioMixerGroup">音效混合组</param>
        public static void Stop(AudioMixerGroup audioMixerGroup)
        {
            for (int index = m_NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = m_NotInidleAudioLi[index];
                if (audio != null&& audio.Group== audioMixerGroup)
                {
                    audio.Stop();
                }
            }
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioMixerGroup">音效混合组</param>
        public void Play(AudioMixerGroup audioMixerGroup)
        {
            for (int index = m_NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = m_NotInidleAudioLi[index];
                if (audio != null && audio.Group == audioMixerGroup)
                {
                    audio.Play();
                }
            }
        }
        /// <summary>
        /// 重新播放音效
        /// </summary>
        /// <param name="audioMixerGroup">音效混合组</param>
        public static void RePlay(AudioMixerGroup audioMixerGroup)
        {
            for (int index = m_NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = m_NotInidleAudioLi[index];
                if (audio != null&& audio.Group == audioMixerGroup)
                {
                    audio.RePlay();
                }
            }
        }
        /// <summary>
        /// 过度播放  一个音频过度到另一个音频的播放形式
        /// </summary>
        /// <param name="form"></param>
        /// <param name="to"></param>
        /// <param name="volume">音量</param>
        /// <param name="time">耗时</param>
        public static void ExcessPlay(Audio form,Audio to,float volume=1,float time=0)
        {
            form.Stop(0, time);
            to.Play(volume, time);
        }
        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="valueName">值名称</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public static bool SetVolume(string groupName, string valueName, float value)
        {
            value = Mathf.Clamp(value, 0, 1);
            value = value * 80.0f - 80.0f;
            return SetFloat(groupName, valueName, value);
        }
        /// <summary>
        /// 获取音量
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="valueName">值名称</param>
        /// <param name="value">值</param>
        /// <returns>是否获取成功</returns>
        public static bool GetVolume(string groupName, string valueName, out float value)
        {
            if (GetFloat(groupName, valueName, out value))
            {
                value = (value + 80.0f) / 80.0f;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="valueName">值名称</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        private static bool SetFloat(string groupName, string valueName, float value)
        {
            return AudioMixerMgr.SetFloat(groupName, valueName, value);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="valueName">值名称</param>
        /// <param name="value">值</param>
        /// <returns>是否获取成功</returns>
        private static bool GetFloat(string groupName, string valueName, out float value)
        {
            return AudioMixerMgr.GetFloat(groupName, valueName, out value);
        }
        #endregion
    }
}
