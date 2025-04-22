using DG.Tweening;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWorkDG : MonoBehaviour
{
    public VRIK vrik;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void TweenIK()
    {
        if(vrik==null)
            vrik = GetComponentInChildren<VRIK>();

        if (animator.enabled == true)
        {
            StartCoroutine(enableIK());

        }

    }
    IEnumerator enableIK()
    {
        
        // DOTween.To(() => vrik.solver.IKPositionWeight, x => vrik.solver.IKPositionWeight = x, i, 2);
        vrik.solver.IKPositionWeight = 1;
         yield return new WaitForSeconds(1);
        vrik.enabled = true;
        animator.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
