using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverInfo : MonoBehaviour
{
    public GameObject hoveringItemObject;
    public bool hoveringHoverInfo;

    public void ShowHoverInfoOnEnter(){
        hoveringHoverInfo = true;
    }

    public void ShowHoverInfoOnExit(){
        hoveringHoverInfo = false;
        if(hoveringItemObject.GetComponent<InventoryItem>().checkingHover == false){
            StartCoroutine(CheckHoverInfo());
        }
    }
    public IEnumerator CheckHoverInfo(){
        hoveringItemObject.GetComponent<InventoryItem>().checkingHover = true;
        yield return new WaitForSeconds(.1f);
        hoveringItemObject.GetComponent<InventoryItem>().checkingHover = false;
        if(hoveringHoverInfo == false && hoveringItemObject.GetComponent<InventoryItem>().hoveringItemButton == false){
            StopCoroutine(hoveringItemObject.GetComponent<InventoryItem>().ShowHoverInfo());
            gameObject.SetActive(false); 
        } 
    }
}
