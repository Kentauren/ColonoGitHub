using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterZoomInButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject character;
    public Transform pointA;
    public Transform pointB;
    public bool zoomIn = false;

    void Update(){
        if(zoomIn == true){
            character.transform.position = Vector3.MoveTowards(character.transform.position, pointA.position, 10 * Time.deltaTime);
        }
    }
    public void OnPointerDown(PointerEventData eventData){
        zoomIn = true;
    }

    public void OnPointerUp(PointerEventData eventData){
        zoomIn = false;
    }
}
