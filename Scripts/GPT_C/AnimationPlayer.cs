using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public Animator animator;
    public float transitionDuration = 0.25f; // 平滑过渡的时间
    public string defaultAnimation = "Idle"; // 默认播放的动画名字

    private bool isPlayingCustomAnimation = false;
    private Queue<string> animationQueue = new Queue<string>();

    // 播放指定名字的动画
    public void PlayAnimations(string animationNames)
    {
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned.");
            return;
        }

        // 将动画名称字符串分割成数组并加入队列
        string[] animations = animationNames.Split(',');
        foreach (string animation in animations)
        {
            animationQueue.Enqueue(animation.Trim());
        }

        // 播放队列中的第一个动画
        PlayNextAnimation();
    }

    private void PlayNextAnimation()
    {
        if (animationQueue.Count > 0)
        {
            string nextAnimation = animationQueue.Dequeue();
            if (animator.HasState(0, Animator.StringToHash(nextAnimation)))
            {
                animator.CrossFade(nextAnimation, transitionDuration);
                isPlayingCustomAnimation = true;
            }
            else
            {
                Debug.LogWarning($"Animation '{nextAnimation}' does not exist.");
                PlayNextAnimation(); // 尝试播放下一个动画
            }
        }
        else
        {
            isPlayingCustomAnimation = false;
            PlayDefaultAnimation();
        }
    }

    private void Update()
    {
        if (isPlayingCustomAnimation)
        {
            // 检查当前动画是否播放完毕
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !animator.IsInTransition(0))
            {
                // 播放下一个动画
                PlayNextAnimation();
            }
        }
    }

    // 播放默认动画
    private void PlayDefaultAnimation()
    {
        if (animator != null && animator.HasState(0, Animator.StringToHash(defaultAnimation)))
        {
            animator.CrossFade(defaultAnimation, transitionDuration);
        }
        else
        {
            Debug.LogWarning($"Default animation '{defaultAnimation}' does not exist.");
        }
    }
}