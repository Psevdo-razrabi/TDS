using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UniRx;
using DG.Tweening;
using DG;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;


public class IKSystem_Hands : MonoBehaviour
{
    [SerializeField] GameObject[] weapons = new GameObject[3];
    [SerializeField] Transform[] destinationPos_Weapon = new Transform[3];
    [SerializeField] Transform[] destinationPos_Weapon_Aim = new Transform[3];
    [SerializeField] Animator animator;
    [SerializeField] Transform[] destinationPos_LeftHand = new Transform[6];
    [SerializeField] Transform[] destinationPos_RightHand = new Transform[6];
    [SerializeField] Transform[] destinationPos_LeftHand_Parent = new Transform[3];
    [SerializeField] LayerMask layer;
    [SerializeField] Transform[] weaponEquipPos;
    [SerializeField, Range(0,50)] float[] animationDurations;
    [SerializeField] Transform[] pathForPistolObj;
    [SerializeField] Vector3[] pathForPistol;
    [SerializeField] Transform[] pathForRifleObj;
    [SerializeField] Vector3[] pathForRifle;
    [SerializeField] Transform[] pathForPistolSelectObj;
    [SerializeField] Vector3[] pathForPistolSelect;
    [SerializeField] Transform[] pathForRifleSelectObj;
    [SerializeField] Vector3[] pathForRifleSelect;
    [SerializeField] Transform handEmpty, handSelectRifle;
    [SerializeField] float[] rayDistance;
    [SerializeField] Transform weaponRaycastStart;
    [SerializeField] Transform[] weaponMinPosInAim, weaponMaxPosInAim;
    [SerializeField] Transform removeLeftHandEmpty;
    [SerializeField] Transform leftHandEmpty;
    [SerializeField] Transform leftHandMove;


    public int currentWeapon, innactiveWeapon, additional;
    public bool change, isAim;
    public string weaponStr;
    public int weapon, currentIndex, lastPossibleIndex;

    public bool rotationApplied, changeAimMode, lastAimMode, weaponInteractionInterrupted, weaponRemoveAnim, removeAllWeapon, removeHand;
    private bool isIdle = true;
    private Transform[] currentPathObj;

    enum animationName:int
    {
        EnterAimMode = 0,
        ExitAimMode = 1,
        SelectPistol = 2,
        RemovePistol = 3,
        SelectRifle = 4,
        RemoveRifle = 5
    }

    void Start()
    {


    }


    private void Update()
    {
        // debug only sandbox
        if(change)
        {
            ChangeWeapon(weaponStr, currentWeapon);
            change = false;
        }

        for(int i = 0; i < pathForPistol.Length; i++)
        {
            pathForPistol[i] = pathForPistolObj[i].transform.position;
        }
        for(int i = 0; i < pathForRifle.Length; i++)
        {
            pathForRifle[i] = pathForRifleObj[i].transform.position;
        }
        for(int i = 0; i < pathForPistolSelect.Length; i++)
        {
            pathForPistolSelect[i] = pathForPistolSelectObj[i].transform.position;
        }
        for(int i = 0; i < pathForRifleSelect.Length; i++)
        {
            pathForRifleSelect[i] = pathForRifleSelectObj[i].transform.position;
        }





        //end debug

        //weapons[innactiveWeapon].transform.position = weaponEquipPos[innactiveWeapon].transform.position;
        //weapons[innactiveWeapon].transform.rotation = weaponEquipPos[innactiveWeapon].transform.rotation;
        
        


        if(!weaponInteractionInterrupted && lastAimMode != isAim) 
        {
            changeAimMode = true;
        }
        lastAimMode = isAim;
        

        if(!weaponInteractionInterrupted && !weaponRemoveAnim)
        {
            if(!isAim)
            {

                weapons[currentWeapon].transform.position = destinationPos_Weapon[currentWeapon].transform.position;
                weapons[currentWeapon].transform.rotation = destinationPos_Weapon[currentWeapon].transform.rotation;
            }
            else
            {

                //weapons[currentWeapon].transform.position = destinationPos_Weapon_Aim[currentWeapon].transform.position;

            }
        }

    }

    private async void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, isIdle ? 0 : 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, isIdle ? 0 : 1);

        leftHandEmpty.transform.position = animator.GetIKPosition(AvatarIKGoal.LeftHand);
        leftHandEmpty.transform.rotation = animator.GetIKRotation(AvatarIKGoal.LeftHand);


        if(changeAimMode && !weaponInteractionInterrupted && !removeHand)
            if(isAim) 
            {
                isIdle = false;
                additional = 3;
                await AimAnimation(destinationPos_Weapon_Aim[currentWeapon].transform.position, destinationPos_Weapon_Aim[currentWeapon].transform.rotation, animationDurations[(int)animationName.EnterAimMode]);
                rotationApplied = false;
            }
            else 
            {
                await AimAnimation(destinationPos_Weapon[currentWeapon].transform.position, destinationPos_Weapon[currentWeapon].transform.rotation, animationDurations[(int)animationName.ExitAimMode]);
                isIdle = true;
                additional = 0;


            }
        else 
            if(!weaponRemoveAnim) SetHands();
            else SetHand(AvatarIKGoal.RightHand, handEmpty);


        if(isAim && !weaponInteractionInterrupted)
        {
            RaycastHit hit;
            var ray = new Ray(weaponRaycastStart.transform.position, transform.forward);
            weapons[currentWeapon].transform.localPosition = Vector3.Lerp(weaponMinPosInAim[currentWeapon].localPosition, weaponMaxPosInAim[currentWeapon].localPosition, 1);
            weapons[currentWeapon].transform.localRotation = Quaternion.Lerp(weaponMinPosInAim[currentWeapon].localRotation, weaponMaxPosInAim[currentWeapon].localRotation, 1);

            if (Physics.Raycast(ray, out hit, rayDistance[currentWeapon]))
            {
                float lerp = Vector3.Distance(hit.point, weaponRaycastStart.transform.position) / rayDistance[currentWeapon];
                if(lerp < 0) lerp = 0;
                weapons[currentWeapon].transform.localPosition = Vector3.Lerp(weaponMinPosInAim[currentWeapon].localPosition, weaponMaxPosInAim[currentWeapon].localPosition, lerp);
                weapons[currentWeapon].transform.localRotation = Quaternion.Lerp(weaponMinPosInAim[currentWeapon].localRotation, weaponMaxPosInAim[currentWeapon].localRotation, lerp);
            }

        }

        if(removeHand)
        {
            Debug.Log(leftHandMove.transform.position);
            Debug.Log(animator.GetIKPosition(AvatarIKGoal.LeftHand));

            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMove.transform.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMove.transform.rotation);
        }
    }

    private void SetHands()
    {
        if(isIdle)
        {
            destinationPos_LeftHand_Parent[currentWeapon].transform.position = animator.GetIKPosition(AvatarIKGoal.RightHand);
            destinationPos_LeftHand_Parent[currentWeapon].transform.rotation = animator.GetIKRotation(AvatarIKGoal.RightHand);
        }

        animator.SetIKPosition(AvatarIKGoal.LeftHand, destinationPos_LeftHand[currentWeapon + additional].transform.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, destinationPos_LeftHand[currentWeapon + additional].transform.rotation);
        
        animator.SetIKPosition(AvatarIKGoal.RightHand, destinationPos_RightHand[currentWeapon + additional].transform.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, destinationPos_RightHand[currentWeapon + additional].transform.rotation);
    }

    private void SetHand(AvatarIKGoal avatarIKGoal, Transform holder)
    {
        animator.SetIKPositionWeight(avatarIKGoal, 1);
        animator.SetIKRotationWeight(avatarIKGoal, 1);

        animator.SetIKPosition(avatarIKGoal, holder.transform.position);
        animator.SetIKRotation(avatarIKGoal, holder.transform.rotation);
    }


    private async UniTask AimAnimation(Vector3 destPos, Quaternion destRot, float speed)
    {
        weaponInteractionInterrupted = true;
        weapons[currentWeapon].transform.DOMove(destPos, speed);
        await weapons[currentWeapon].transform.DORotate(destRot.eulerAngles, speed);
        changeAimMode = false;
        weaponInteractionInterrupted = false;
    }


    private async UniTask ChangeWeaponAnimation(Vector3[] path, Transform[] pathObj, float speed, bool select)
    {
        weaponRemoveAnim = true;
        weaponInteractionInterrupted = true;
        currentPathObj = pathObj;
        int lastIndex = pathObj.Length - 1;
        await weapons[currentWeapon].transform.DOPath(path, speed, PathType.CatmullRom, PathMode.Full3D, 15, Color.cyan).OnWaypointChange(index =>
        {
            if (index < path.Length)
            {
                currentIndex = index;
                if (currentIndex + 1 < path.Length)
                    weapons[currentWeapon].transform.DORotate(currentPathObj[currentIndex + 1].transform.localRotation.eulerAngles, 1f, RotateMode.Fast);

                lastPossibleIndex = lastIndex;
            }
        }).OnUpdate(() => 
        {
            if(currentIndex <= lastPossibleIndex)
            {

                handEmpty.transform.position = destinationPos_RightHand[currentWeapon + 3].transform.position;
                handEmpty.transform.rotation = destinationPos_RightHand[currentWeapon + 3].transform.rotation;
            }
        }).SetEase(Ease.InOutCubic);

        // await weapons[currentWeapon].transform.DORotate(currentPathObj[currentIndex].transform.localRotation.eulerAngles, 1f, RotateMode.Fast);


        if(select)
        {
            weaponRemoveAnim = false;
            weaponInteractionInterrupted = false;
        }


    }

    async UniTask RemoveHand(Transform from, Transform to, bool remove)
    {
        removeHand = true;

        leftHandMove.transform.position = from.transform.position;
        leftHandMove.transform.rotation = from.transform.rotation;

        leftHandMove.transform.rotation = to.transform.rotation;

        await leftHandMove.DOMove(to.transform.position, .25f);

        if(remove)
            removeHand = false;

        
    }
    


    [SerializeField] Transform temp;

    public async void ChangeWeapon(string weaponName, int weapon)
    {
        isIdle = false;
        weapons[currentWeapon].transform.position = destinationPos_Weapon[currentWeapon].transform.position;
        weapons[currentWeapon].transform.rotation = destinationPos_Weapon[currentWeapon].transform.rotation;
        innactiveWeapon = currentWeapon;

        RemoveHand(leftHandEmpty, removeLeftHandEmpty, false);

        await ChangeWeaponAnimation
        (
            weapon == 0 ? pathForPistol : pathForRifle, 
            weapon == 0 ? pathForPistolObj : pathForRifleObj,
            weapon == 0 ? animationDurations[(int)animationName.RemovePistol] : animationDurations[(int)animationName.RemoveRifle],
            false
        );
        Debug.Log(weapon);
        
        currentWeapon = weapon == 0 ? 1 : 0;

        await ChangeWeaponAnimation
        (
            weapon == 0 ? pathForRifleSelect : pathForPistolSelect,
            weapon == 0 ? pathForRifleSelectObj : pathForPistolSelectObj,
            weapon == 0 ? animationDurations[(int)animationName.SelectRifle] : animationDurations[(int)animationName.SelectPistol],
            true
        );

        RemoveHand(removeLeftHandEmpty, destinationPos_LeftHand[currentWeapon], true);



        isIdle = true;


        // await ChangeWeaponAnimation(innactiveWeapon == 1 ? pathForPistol : pathForRifle, innactiveWeapon == 1 ? animationDurations[(int)animationName.RemovePistol] : animationDurations[(int)animationName.RemoveRifle]);
        switch (weaponName)
        {
            case "Pistol":
                currentWeapon = 0;
                break;
            case "Rifle":
                currentWeapon = 1;
                break;
            case "Shotgun":
                currentWeapon = 2;
                break;
            default:
                Debug.LogWarning("Error incorrect weapon name!");
                break;
        }

    }

}
