using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
namespace Farme
{
    [Obsolete("已更替为:AudioManager统一管理,未来将会删除")]
    /// <summary>
    /// Audio3D托管
    /// </summary>
    public class Audio3DMgr : BaseAudioMgr
    {
        protected Audio3DMgr() { }

        protected override BaseAudio GetAudioInstance()
        {
            return MonoFactory<Audio3D>.GetInstance();
        }
    }
}
