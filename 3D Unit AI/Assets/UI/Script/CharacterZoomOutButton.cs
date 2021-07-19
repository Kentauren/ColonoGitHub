using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterZoomOutButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject character;
    public Transform pointA;
    public Transform pointB;
    public bool zoomOut = false;

    void Update(){
        if(zoomOut == true){
            character.transform.position = Vector3.MoveTowards(character.transform.position, pointB.position, 10 * Time.deltaTime);
        }
    }
    public void OnPointerDown(PointerEventData eventData){
        zoomOut = true;
    }

    public void OnPointerUp(PointerEventData eventData){
        zoomOut = false;
    }
}
