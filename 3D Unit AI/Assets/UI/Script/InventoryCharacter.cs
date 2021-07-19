using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCharacter : MonoBehaviour
{
    public Transform Head;
    public Transform Body;
    public Transform rightHand;
    public Transform leftHand;

    public Animation animator;

    void Start(){
        animator = gameObject.GetComponent<Animation>();
    }

    public void Animator(bool isActive){
        if(isActive == true){
            animator.Play("Idle");
            animator["IdleLeftHand"].layer = 1; 
            animator.Play("IdleLeftHand");
            animator["IdleLeftHand"].weight = .3f;
            animator["IdleRightHand"].layer = 2;
            animator.Play("IdleRightHand");
            animator["IdleRightHand"].weight = .3f;
        }
        else{
            animator.Stop();
        }
    }
}
