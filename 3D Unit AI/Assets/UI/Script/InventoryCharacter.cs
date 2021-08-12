using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCharacter : MonoBehaviour
{
    public InventorySystem inventorySystem;
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

            if(inventorySystem.selectedCard != null && inventorySystem.selectedCard.GetComponent<UnitCard>().characterLeftHandCard != null){
                if(inventorySystem.selectedCard.GetComponent<UnitCard>().characterLeftHandCard.tag == "Shield"){
                    animator["IdleShieldLeftHand"].layer = 1; 
                    animator.Play("IdleShieldLeftHand");
                    animator["IdleShieldLeftHand"].weight = .3f;                   
                }
                else{
                    animator["IdleLeftHand"].layer = 1; 
                    animator.Play("IdleLeftHand");
                    animator["IdleLeftHand"].weight = .3f;      
                }
            }
            else{
                animator["IdleLeftHand"].layer = 1; 
                animator.Play("IdleLeftHand");
                animator["IdleLeftHand"].weight = .3f;
            }

            if(inventorySystem.selectedCard != null && inventorySystem.selectedCard.GetComponent<UnitCard>().characterRightHandCard != null){
                if(inventorySystem.selectedCard.GetComponent<UnitCard>().characterRightHandCard.tag == "Spear"){
                    if(inventorySystem.selectedCard.GetComponent<UnitCard>().characterLeftHandCard == null){
                        animator["ReadySpearBothHands"].layer = 2;
                        animator.Play("ReadySpearBothHands");
                        animator["ReadySpearBothHands"].weight = .3f; 
                    }
                    else{
                        animator["IdleSpearRightHand"].layer = 2;
                        animator.Play("IdleSpearRightHand");
                        animator["IdleSpearRightHand"].weight = .3f;                        
                    }

                }
                else{
                    animator["IdleRightHand"].layer = 2;
                    animator.Play("IdleRightHand");
                    animator["IdleRightHand"].weight = .3f;
                }
            }
            else{
                animator["IdleRightHand"].layer = 2;
                animator.Play("IdleRightHand");
                animator["IdleRightHand"].weight = .3f;                
            }
            

        }
        else{
            animator.Stop();
        }
    }
}
