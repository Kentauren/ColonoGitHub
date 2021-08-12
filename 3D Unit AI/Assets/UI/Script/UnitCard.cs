using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour{

    public Canvas canvas;
    public InventorySystem inventorySystem;
    public InventoryCharacter inventoryCharacter;
    public RectTransform card;
    public float weight;
    public float armor;
    public RectTransform nameInputField;
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
    public Transform characterBodyClothing;
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
            canvas.GetComponent<InventorySystem>().UpdateStatsInfo();
        }
        else{
            inventorySystem.ResetOnClick();
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
        inventoryCharacter.Animator(true);
    }

    public void UpdateName(){
        if(inventorySystem.selectedCard == card){
            inventorySystem.nameInputField.GetComponent<TMP_InputField>().text = nameInputField.GetComponent<TMP_InputField>().text.ToString();
        }
    }

    public void ChangeLoadOut(){
        if(characterHead.childCount > 0){ //Head
            Destroy(characterHead.GetChild(0).gameObject);
        }
        if(characterHeadCard != null){ //Head
            GameObject weaponItemObject = characterHeadCard.GetComponent<Weapon>().itemObject;
            headSlotImage.material = characterHeadCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
            var itemInfo = characterHeadCard.GetComponent<Weapon>().itemInfo;
            GameObject newItemObjectHead = Instantiate(characterHeadCard, characterHead.transform.position, characterHead.transform.rotation);
            newItemObjectHead.transform.Rotate(itemInfo.eulerAnglex, itemInfo.eulerAngley, itemInfo.eulerAnglez);

            newItemObjectHead.transform.SetParent(characterHead);  
            newItemObjectHead.transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);            
        }
        if(characterHeadCard == null){ //Head
            headSlotImage.material = defaultHeadSlotMaterial;
        }
        if(characterBody.childCount > 0){ //Body
            Destroy(characterBody.GetChild(0).gameObject);
        }
        if(characterBodyCard != null){ //Body
            GameObject weaponItemObject = characterBodyCard.GetComponent<Weapon>().itemObject;
            bodySlotImage.material = characterBodyCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
            var itemInfo = characterBodyCard.GetComponent<Weapon>().itemInfo;
            GameObject newItemObjectBody = Instantiate(characterBodyCard, characterBody.transform.position, characterBody.transform.rotation);
            newItemObjectBody.transform.Rotate(itemInfo.eulerAnglex, itemInfo.eulerAngley, itemInfo.eulerAnglez);

            newItemObjectBody.transform.SetParent(characterBody);  
            newItemObjectBody.transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);      
        }
        if(characterBodyCard == null){ //Body
            bodySlotImage.material = defaultBodySlotMaterial;
        }
        if(characterClothingCard != null){ //Clothing
            characterBodyClothing.GetComponent<SkinnedMeshRenderer>().material = characterClothingCard.GetComponent<Weapon>().itemInfo.itemMaterial;  
        }
        if(characterClothingCard == null){ //Clothing
            characterBodyClothing.GetComponent<SkinnedMeshRenderer>().material = defaultClothingMaterial;
            clothingSlotImage.material = defaultClothingSlotMaterial;
        }
        if(characterRightHand.childCount > 0){ //RightHand
            Destroy(characterRightHand.GetChild(0).gameObject);
        }
        if(characterRightHandCard != null){ //RightHand
            GameObject weaponItemObject = characterRightHandCard.GetComponent<Weapon>().itemObject;
            rightHandSlotImage.material = characterRightHandCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
            var itemInfo = characterRightHandCard.GetComponent<Weapon>().itemInfo; 
            GameObject newItemObjectRightHand = Instantiate(characterRightHandCard, characterRightHand.transform.position, characterRightHand.transform.rotation);
            newItemObjectRightHand.transform.Rotate(itemInfo.eulerAnglex, itemInfo.eulerAngley, itemInfo.eulerAnglez);

            newItemObjectRightHand.transform.SetParent(characterRightHand);
            newItemObjectRightHand.transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);
        }
        if(characterRightHandCard == null){ //RightHand
            rightHandSlotImage.material = defaultHandSlotMaterial;
        }        
        if(characterLeftHand.childCount > 0){ //LeftHand
            Destroy(characterLeftHand.GetChild(0).gameObject);
        }
        if(characterLeftHandCard != null){ //LeftHand
            GameObject weaponItemObject = characterLeftHandCard.GetComponent<Weapon>().itemObject;
            leftHandSlotImage.material = characterLeftHandCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
            var itemInfo = characterLeftHandCard.GetComponent<Weapon>().itemInfo;
            GameObject newItemObjectLeftHand = Instantiate(characterLeftHandCard, characterLeftHand.transform.position, characterLeftHand.transform.rotation);
            newItemObjectLeftHand.transform.Rotate(itemInfo.eulerAnglex, itemInfo.eulerAngley, itemInfo.eulerAnglez);

            newItemObjectLeftHand.transform.SetParent(characterLeftHand);
            if(newItemObjectLeftHand.tag == "Shield"){
                newItemObjectLeftHand.transform.GetChild(0).transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);
            }
            else{
                newItemObjectLeftHand.transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);   
            }
                     
        }
        if(characterLeftHandCard == null){ //LeftHand
            leftHandSlotImage.material = defaultHandSlotMaterial;
        }
        if(characterBack.childCount > 0){ //Back
            Destroy(characterBack.GetChild(0).gameObject);
        }
        if(characterBackCard != null){ //Back
            GameObject weaponItemObject = characterBackCard.GetComponent<Weapon>().itemObject;
            backSlotImage.material = characterBackCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
            var itemInfo = characterBackCard.GetComponent<Weapon>().itemInfo;
            GameObject newItemObjectBack = Instantiate(characterBackCard, characterBack.transform.position, characterBack.transform.rotation);
            newItemObjectBack.transform.Rotate(itemInfo.eulerAnglex, itemInfo.eulerAngley, itemInfo.eulerAnglez);

            newItemObjectBack.transform.SetParent(characterBack);
            if(newItemObjectBack.tag == "Shield"){
                newItemObjectBack.transform.GetChild(0).transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);
            }
            else{
                newItemObjectBack.transform.localScale = new Vector3(itemInfo.scalex, itemInfo.scaley, itemInfo.scalez);   
            }          
        }
        if(characterBackCard == null){ //Back
            backSlotImage.material = defaultHandSlotMaterial;
        }
    }
}
