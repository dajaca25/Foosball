//using UnityEngine.XR.ARFoundation;
using UnityEngine;
//using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using System;
using Enablegames;
using UnityEngine.XR.ARFoundation.Samples;

namespace EAG
{

public class AvatarBoneControllerInterface : BoneControllerInterface
{
       
    public Transform SkeletonRoot;
    AvatarBoneController avatarBoneController;
    public override void Awake()
    {
        print("AvatarBoneControllerInterface");
        avatarBoneController = this.GetComponent<AvatarBoneController>();
        print("AvatarBoneControllerInterface1:"+avatarBoneController);
        if (!avatarBoneController)
            avatarBoneController = gameObject.AddComponent<AvatarBoneController>() as AvatarBoneController;
        print("AvatarBoneControllerInterface2:"+avatarBoneController);
        ((BoneController) avatarBoneController).skeletonRoot = SkeletonRoot;
        //base.Awake();
    }

    public override void MapBones(ref Transform [] bones, ref string [] boneNames )
    {
        avatarBoneController.MapBones(ref bones,ref boneNames);
        Debug.Log("BoneControllerInterface:MapBones for "+ this.name);
    }


    public override void InitializeSkeletonJoints()
    {
        Awake();
        avatarBoneController.InitializeSkeletonJoints();
    }

}
}
