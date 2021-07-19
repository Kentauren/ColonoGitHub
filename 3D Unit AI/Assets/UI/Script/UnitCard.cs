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
    public Material defaultClothingMaterial;
    public Material defaultHeadSlotMaterial;
    public Material defaultBodySlotMaterial;
    public Material defaultClothingSlotMaterial;
    public Material defaultHandSlotMaterial;
    public GameObject characterHeadCard;
    public GameObject characterBodyCard;
    public GameObject characterClothingCard;
    public GameObject characterRightHandCard;
    public GameObject characterLeftHandCard;
    public GameObject characterBackCard;
    public Transform characterHead;
    public Transform characterBody;
    public Transform characterRightHand;
    public Transform characterLeftHand;
    public Transform characterBack;
    public Image headSlotImage;
    public Image bodySlotImage;
    public Image clothingSlotImage;
    public Image rightHandSlotImage;
    public Image leftHandSlotImage;
    public Image backSlotImage;

    public void DeleteButtonOnClick(){
        int i = canvas.GetComponent<InventorySystem>().boardList.IndexOf(card);
        canvas.GetComponent<InventorySystem>().boardList.RemoveAt(i);
        Destroy(canvas.GetComponent<UnitSpawnSystem>().spawnBoardList[i].gameObject);
        canvas.GetComponent<UnitSpawnSystem>().spawnBoardList.RemoveAt(i);

        canvas.GetComponent<InventorySystem>().boardView.sizeDelta = new Vector2(canvas.GetComponent<InventorySystem>().boardView.sizeDelta.x, canvas.GetComponent<InventorySystem>().boardView.sizeDelta.y - 60);

        if(canvas.GetComponent<InventorySystem>().boardList.Count > 0){
            canvas.GetComponent<InventorySystem>().selectedCard = canvas.GetComponent<InventorySystem>().boardList[0];
            canvas.GetComponent<UnitSpawnSystem>().selectedSpawnCard = canvas.GetComponent<UnitSpawnSystem>().spawnBoardList[0];
        }
        else{
            canvas.GetComponent<InventorySystem>().selectedCard = null;
            canvas.GetComponent<UnitSpawnSystem>().selectedSpawnCard = null;
        }
        Destroy(gameObject);
    }

    public void EditButtonOnClick(){
        canvas.GetComponent<InventorySystem>().selectedCard = card; 
        int i = canvas.GetComponent<InventorySystem>().boardList.IndexOf(card);
        canvas.GetComponent<UnitSpawnSystem>().selectedSpawnCard = canvas.GetComponent<UnitSpawnSystem>().spawnBoardList[i];
        canvas.GetComponent<InventorySystem>().UpdateStatsInfo();
        canvas.GetComponent<InventorySystem>().UpdateName();
        ChangeLoadOut();
    }

    public void ChangeLoadOut(){
        if(characterHead.childCount > 0){ //Head
            Destroy(characterHead.GetChild(0).gameObject);
        }
        if(characterHeadCard != null){ //Head
            headSlotImage.material = characterHeadCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
            var itemAngleHead = characterHeadCard.GetComponent<Weapon>().itemInfo;
            GameObject newItemObjectHead = Instantiate(characterHeadCard, characterHead.transform.position, characterHead.transform.rotation);
            newItemObjectHead.transform.Rotate(itemAngleHead.eulerAnglex, itemAngleHead.eulerAngley, itemAngleHead.eulerAnglez);
            newItemObjectHead.transform.SetParent(characterHead);              
        }
        if(characterHeadCard == null){ //Head
            headSlotImage.material = defaultHeadSlotMaterial;
        }
        if(characterBody.childCount > 0){ //Body
            Destroy(characterBody.GetChild(0).gameObject);
        }
        if(characterBodyCard != null){ //Body
            bodySlotImage.material = characterBodyCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
            var itemAngleBody = characterBodyCard.GetComponent<Weapon>().itemInfo;
            GameObject newItemObjectBody = Instantiate(characterBodyCard, characterBody.transform.position, characterBody.transform.rotation);
            newItemObjectBody.transform.Rotate(itemAngleBody.eulerAnglex, itemAngleBody.eulerAngley, itemAngleBody.eulerAnglez);
            newItemObjectBody.transform.SetParent(characterBody);        
        }
        if(characterBodyCard == null){ //Body
            bodySlotImage.material = defaultBodySlotMaterial;
        }
        if(characterClothingCard != null){ //Clothing
            characterBody.GetComponent<MeshRenderer>().material = characterClothingCard.GetComponent<Weapon>().itemInfo.itemMaterial;  
        }
        if(characterClothingCard == null){ //Clothing
            characterBody.GetComponent<MeshRenderer>().material = defaultClothingMaterial;
            clothingSlotImage.material = defaultClothingSlotMaterial;
        }
        if(characterRightHand.childCount > 0){ //RightHand
            Destroy(characterRightHand.GetChild(0).gameObject);
        }
        if(characterRightHandCard != null){ //RightHand
            rightHandSlotImage.material = characterRightHandCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
            var itemAngleRightHand = characterRightHandCard.GetComponent<Weapon>().itemInfo; 
            GameObject newItemObjectRightHand = Instantiate(characterRightHandCard, characterRightHand.transform.position, characterRightHand.transform.rotation);
            newItemObjectRightHand.transform.Rotate(itemAngleRightHand.eulerAnglex, itemAngleRightHand.eulerAngley, itemAngleRightHand.eulerAnglez);
            newItemObjectRightHand.transform.SetParent(characterRightHand);
        }
        if(characterRightHandCard == null){ //RightHand
            rightHandSlotImage.material = defaultHandSlotMaterial;
        }        
        if(characterLeftHand.childCount > 0){ //LeftHand
            Destroy(characterLeftHand.GetChild(0).gameObject);
        }
        if(characterLeftHandCard != null){ //LeftHand
            leftHandSlotImage.material = characterLeftHandCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
            var itemAngleLeftHand = characterLeftHandCard.GetComponent<Weapon>().itemInfo;
            GameObject newItemObjectLeftHand = Instantiate(characterLeftHandCard, characterLeftHand.transform.position, characterLeftHand.transform.rotation);
            newItemObjectLeftHand.transform.Rotate(itemAngleLeftHand.eulerAnglex, itemAngleLeftHand.eulerAngley, itemAngleLeftHand.eulerAnglez);
            newItemObjectLeftHand.transform.SetParent(characterLeftHand);            
        }
        if(characterLeftHandCard == null){ //LeftHand
            leftHandSlotImage.material = defaultHandSlotMaterial;
        }
        if(characterBack.childCount > 0){ //Back
            Destroy(characterBack.GetChild(0).gameObject);
        }
        if(characterBackCard != null){ //Back
            backSlotImage.material = characterBackCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
            var itemAngleBack = characterBackCard.GetComponent<Weapon>().itemInfo;
            GameObject newItemObjectBack = Instantiate(characterBackCard, characterBack.transform.position, characterBack.transform.rotation);
            newItemObjectBack.transform.Rotate(itemAngleBack.eulerAnglex, itemAngleBack.eulerAngley, itemAngleBack.eulerAnglez);
            newItemObjectBack.transform.SetParent(characterBack);            
        }
        if(characterBackCard == null){ //Back
            backSlotImage.material = defaultHandSlotMaterial;
        }
    }
}
