using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataOverview : MonoBehaviour{
    public Select select;
    public TextMeshProUGUI unemployedText;
    public TextMeshProUGUI workerText;
    public TextMeshProUGUI soldierText;
    public TextMeshProUGUI totalPopulationText;
    public TextMeshProUGUI woodenLogsText;
    public int unemployedAmount;
    public int workerAmount;
    public int soldierAmount;
    public int totalPopulationAmount;
    public int woodenLogsAmount;

    void Start(){
        unemployedText.text = unemployedAmount.ToString();
        workerText.text = workerAmount.ToString();
        soldierText.text = soldierAmount.ToString();
        totalPopulationText.text = totalPopulationAmount.ToString();
        woodenLogsText.text = woodenLogsAmount.ToString();

        StartCoroutine(UpdateResources());
        StartCoroutine(UpdatePopulationOverview());
    }

    private IEnumerator UpdateResources(){
        bool updateResources = true;

        while(updateResources == true){
            yield return new WaitForSeconds(1f);
            woodenLogsText.text = woodenLogsAmount.ToString();
        }
    }

    private IEnumerator UpdatePopulationOverview(){
        bool updatePopulationOverview = true;
        while(updatePopulationOverview == true){       
            unemployedText.text = unemployedAmount.ToString();
            workerText.text = workerAmount.ToString();
            soldierText.text = soldierAmount.ToString();
            totalPopulationAmount = select.selectables.Count;
            totalPopulationText.text = totalPopulationAmount.ToString();

            //When new units has spawned the calculation below will add the data to unemployed
            int currentTotalAmount = workerAmount + soldierAmount + unemployedAmount;
            if(currentTotalAmount != totalPopulationAmount){
                unemployedAmount += totalPopulationAmount - currentTotalAmount;
            }
            yield return new WaitForSeconds(1f);
        }
    }

}
