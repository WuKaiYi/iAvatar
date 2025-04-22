using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    // 引用视频播放组件
    public VideoPlayer videoPlayer;

    // 设置播放进度，输入参数为百分比
    public void SetPlayBackPosition(float percent)
    {
        if (percent < 0 || percent > 100)
        {
            Debug.LogError("Percentage must be between 0 and 100");
            return;
        }
        long targetFrame = (long)(videoPlayer.frameCount * percent / 100);
        videoPlayer.frame = targetFrame;

        //暫停播放
        videoPlayer.Pause();
    }
    // 播放视频
    public void PlayVideo()
    {
        videoPlayer.Stop();
        videoPlayer.Play();
    }
}
