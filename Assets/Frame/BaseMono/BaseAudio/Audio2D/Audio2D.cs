using System;
namespace Farme
{
    [Obsolete("已更替为Audio统一使用,未来将会删除")]
    /// <summary>
    /// Audio2D
    /// </summary>
    public class Audio2D : BaseAudio
    {
        protected Audio2D() { }
        #region 生命周期函数
       
        #endregion
        #region 方法       
        protected override BaseAudioMgr GetRelyOnAudioMgr()
        {
            return MonoSingletonFactory<Audio2DMgr>.GetSingleton();
        }

        #endregion
    }
}
