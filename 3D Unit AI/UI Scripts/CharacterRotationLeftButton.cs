using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterRotationLeftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject character;
    public bool characterRotatingLeft = false;

    void Update(){
        if(characterRotatingLeft == true){
            character.transform.Rotate(Vector3.up * Time.deltaTime * 150);
        }
    }
    public void OnPointerDown(PointerEventData eventData){
        characterRotatingLeft = true;
    }

    public void OnPointerUp(PointerEventData eventData){
        characterRotatingLeft = false;
    }
}
