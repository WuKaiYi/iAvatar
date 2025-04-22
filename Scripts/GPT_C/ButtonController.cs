using System.Collections;
using UnityEngine;
using DG.Tweening;

using SickscoreGames.HUDNavigationSystem;

public class ButtonController : MonoBehaviour
{

    public Animator Animator;
    public HUDNavigationSystem HUDNavigationSystem;

    public GameObject[] UIs;

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    foreach (var item in UIs)
        //    {
        //        item.SetActive(!item.activeSelf);
        //    }
        //}
    }
    /// <summary>
    /// 小地圖放大
    /// </summary>
    public void ZoomInMiniMap()
    {
        HUDNavigationSystem.minimapScale = Mathf.Clamp(HUDNavigationSystem.minimapScale + 0.05f, 0.1f, 1f);
    }

    public void ZoomOutMiniMap()
    {
        HUDNavigationSystem.minimapScale = Mathf.Clamp(HUDNavigationSystem.minimapScale - 0.05f, 0.1f, 1f);
    }
    public void SetFloat(string name)
    {
        StartCoroutine(_setFloat(name));


    }

    IEnumerator _setFloat(string name)
    {
        /*    var forwardTween = */
        DOTween.To(
            () => Animator.GetFloat(name),
            (val) => Animator.SetFloat(name, val),
            0.5f,
            0.5f)
            ;
        //DOTween.To(() => Animator.GetFloat(name), (val) => Animator.SetFloat(name, val), 1, 0.5);

        // 在这里编写你的代码
        yield return new WaitForSeconds(0.8f);
        DOTween.To(
            () => Animator.GetFloat(name),
            (val) => Animator.SetFloat(name, val),
            0,
            0.5f)
            ;
    }
    public void SetBool(string name)
    {
        StartCoroutine(_settBool(name));


    }

    IEnumerator _settBool(string name)
    {
        Animator.SetBool(name, true);

        //DOTween.To(() => Animator.GetFloat(name), (val) => Animator.SetFloat(name, val), 1, 0.5);

        // 在这里编写你的代码
        yield return new WaitForSeconds(0.8f);
        Animator.SetBool(name, false);
    }
}