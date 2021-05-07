using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour{

    public Canvas canvas;
    public RectTransform card;
    public float weight;
    public float armor;
    public Text nameField;
    public void DeleteButtonOnClick(){
        int i = canvas.GetComponent<InventorySystem>().boardList.IndexOf(card);
        canvas.GetComponent<InventorySystem>().boardList.RemoveAt(i);
        if(canvas.GetComponent<InventorySystem>().boardList.Count > 0){
            canvas.GetComponent<InventorySystem>().selectedCard = canvas.GetComponent<InventorySystem>().boardList[0];
        }
        else{
            canvas.GetComponent<InventorySystem>().selectedCard = null;
        }
        Destroy(gameObject);
    }
    public void EditButtonOnClick(){
        canvas.GetComponent<InventorySystem>().selectedCard = card;
        canvas.GetComponent<InventorySystem>().UpdateStatsInfo();
    }
}
