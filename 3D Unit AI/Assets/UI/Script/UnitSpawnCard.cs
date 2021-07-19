using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawnCard : MonoBehaviour{

    public UnitSpawnSystem unitSpawnSystem;
    public RectTransform unitCard;
    public Text cardName;
    public Text cardNameStatsInfo;
    public Text totalWeightText;
    public Text totalArmorText;    
    public Button unitCardButton;
    Color32 selectedButtonColor = new Color32(140, 140, 140, 255);
    Color32 defaultButtonColor = new Color32(255, 255, 255, 255);
    public GameObject characterHeadCard;
    public GameObject characterBodyCard;
    public GameObject characterClothingCard;
    public GameObject characterRightHandCard;
    public GameObject characterLeftHandCard;
    public GameObject characterBackCard;
    public Image headSlotImage;
    public Image bodySlotImage;
    public Image clothingSlotImage;
    public Image rightHandSlotImage;
    public Image leftHandSlotImage;
    public Image backSlotImage;
    public Material defaultHeadSlotMaterial;
    public Material defaultBodySlotMaterial;
    public Material defaultHandSlotMaterial;
    public Material defaultClothingSlotMaterial;
    public float totalWeight;
    public float totalArmor;

    public void SelectCardOnClick(){
        if(unitSpawnSystem.selectedButton != null){
            unitSpawnSystem.selectedButton.GetComponent<UnitSpawnCard>().unitCardButton.GetComponent<Image>().color = defaultButtonColor;
        }

        UpdateText();
        unitCardButton.GetComponent<Image>().color = selectedButtonColor;
        unitSpawnSystem.selectedButton = unitCardButton;    
        unitSpawnSystem.selectedSpawnCard = unitCard;

        headSlotImage.material = defaultHeadSlotMaterial;
        bodySlotImage.material = defaultBodySlotMaterial;
        clothingSlotImage.material = defaultClothingSlotMaterial;
        rightHandSlotImage.material = defaultHandSlotMaterial;
        leftHandSlotImage.material = defaultHandSlotMaterial;
        backSlotImage.material = defaultHandSlotMaterial;
        
        if(characterHeadCard != null){
            headSlotImage.material = characterHeadCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
        }
        if(characterBodyCard != null){
            bodySlotImage.material = characterBodyCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
        }
        if(characterClothingCard != null){
            clothingSlotImage.material = characterClothingCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
        }
        if(characterRightHandCard != null){
            rightHandSlotImage.material = characterRightHandCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
        }
        if(characterLeftHandCard != null){
            leftHandSlotImage.material = characterLeftHandCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
        }
        if(characterBackCard != null){
            backSlotImage.material = characterBackCard.GetComponent<Weapon>().itemInfo.itemIconMaterial;
        }
    }

    public void UpdateText(){
        totalWeightText.text = totalWeight.ToString();
        totalArmorText.text = totalArmor.ToString();
        cardNameStatsInfo.text = cardName.text;    
    }
}
