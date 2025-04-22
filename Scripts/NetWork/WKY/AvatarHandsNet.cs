using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarHandsNet : MonoBehaviour
{
    public Transform[] Hands;
    public Transform[] avatarHands;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        

       
    }

    public void Init()
    {
        animator = this.GetComponent<Animator>();
        avatarHands = new Transform[]{

        animator.GetBoneTransform(HumanBodyBones.LeftIndexProximal),
        animator.GetBoneTransform(HumanBodyBones.LeftIndexIntermediate),
        animator.GetBoneTransform(HumanBodyBones.LeftIndexDistal),

         animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal),
        animator.GetBoneTransform(HumanBodyBones.LeftMiddleIntermediate),
        animator.GetBoneTransform(HumanBodyBones.LeftMiddleDistal),

         animator.GetBoneTransform(HumanBodyBones.LeftRingProximal),
        animator.GetBoneTransform(HumanBodyBones.LeftRingIntermediate),
        animator.GetBoneTransform(HumanBodyBones.LeftRingDistal),

                 animator.GetBoneTransform(HumanBodyBones.LeftLittleProximal),
        animator.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate),
        animator.GetBoneTransform(HumanBodyBones.LeftLittleDistal),

                 animator.GetBoneTransform(HumanBodyBones.LeftThumbProximal),
        animator.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate),
        animator.GetBoneTransform(HumanBodyBones.LeftThumbDistal),


             animator.GetBoneTransform(HumanBodyBones.RightIndexProximal),
        animator.GetBoneTransform(HumanBodyBones.RightIndexIntermediate),
        animator.GetBoneTransform(HumanBodyBones.RightIndexDistal),

         animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal),
        animator.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate),
        animator.GetBoneTransform(HumanBodyBones.RightMiddleDistal),

         animator.GetBoneTransform(HumanBodyBones.RightRingProximal),
        animator.GetBoneTransform(HumanBodyBones.RightRingIntermediate),
        animator.GetBoneTransform(HumanBodyBones.RightRingDistal),

                 animator.GetBoneTransform(HumanBodyBones.RightLittleProximal),
        animator.GetBoneTransform(HumanBodyBones.RightLittleIntermediate),
        animator.GetBoneTransform(HumanBodyBones.RightLittleDistal),

                 animator.GetBoneTransform(HumanBodyBones.RightThumbProximal),
        animator.GetBoneTransform(HumanBodyBones.RightThumbIntermediate),
        animator.GetBoneTransform(HumanBodyBones.RightThumbDistal),

    };
        isinit = true;
    }
    bool isinit = false;
    private void FixedUpdate()
    {
        if (!isinit)
            return;


        for (int i = 0; i < Hands.Length; i++)
        {

            avatarHands[i].position = Hands[i].position;
            avatarHands[i].rotation  = Hands[i].rotation;

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
