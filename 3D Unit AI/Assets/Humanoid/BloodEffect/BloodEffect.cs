using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour {
    public Material bloodPool0Percent;
    public Material bloodPool25Percent;
    public Material bloodPool50Percent;
    public Material bloodPool75Percent;

    public bool fade = false;
    
    // Start is called before the first frame update
    void Start(){
        StartCoroutine(FadeBlood());
    }
    public IEnumerator FadeBlood(){
        gameObject.GetComponent<MeshRenderer>().material = bloodPool0Percent;

        fade = true;      
        while(fade == true){
            yield return new WaitForSeconds(30);
            gameObject.GetComponent<MeshRenderer>().material = bloodPool25Percent;
            yield return new WaitForSeconds(15);
            gameObject.GetComponent<MeshRenderer>().material = bloodPool50Percent;
            yield return new WaitForSeconds(10);
            gameObject.GetComponent<MeshRenderer>().material = bloodPool75Percent;
            yield return new WaitForSeconds(10);
            Destroy(gameObject);
        }
    }
}
