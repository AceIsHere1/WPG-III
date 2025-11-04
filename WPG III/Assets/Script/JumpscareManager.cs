using UnityEngine;
using UnityEngine.Video;
using System;

public class JumpscareManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Canvas jumpscareCanvas;

    private Action onFinishCallback;

    void Start()
    {
        if (jumpscareCanvas != null)
            jumpscareCanvas.enabled = false;

        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished;
    }

    public void PlayJumpscare(Action callback)
    {
        onFinishCallback = callback;

        if (jumpscareCanvas != null)
            jumpscareCanvas.enabled = true;

        if (videoPlayer != null)
            videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (jumpscareCanvas != null)
            jumpscareCanvas.enabled = false;

        onFinishCallback?.Invoke();
    }
}
