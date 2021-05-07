using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterRotationRightButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject character;
    public bool characterRotatingRight = false;

    void Update(){
        if(characterRotatingRight == true){
            character.transform.Rotate(Vector3.down * Time.deltaTime * 100);
        }
    }
    public void OnPointerDown(PointerEventData eventData){
        characterRotatingRight = true;
    }

    public void OnPointerUp(PointerEventData eventData){
        characterRotatingRight = false;
    }
}
