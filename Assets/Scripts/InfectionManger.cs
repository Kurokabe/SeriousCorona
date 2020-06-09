using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfectionManger : MonoBehaviour
{
    
    private TextMeshProUGUI infectionRateText;

    private int infectionRate = 0;
    public int InfectionRate { get => infectionRate; set { infectionRate = value; infectionRateText.text = infectionRate.ToString() + "%"; } }
    
    private double delta = 0.5; 

    void Start(){
       infectionRateText = GameObject.Find("InfectionRateLabel").GetComponent<TextMeshProUGUI>();
	}

    void OnTriggerStay(Collider col){
        if(col.tag == "enemy"){
            delta += Time.deltaTime;
            
            if(delta >= 1){
                InfectionRate++;
                delta = 0;
			}
		}
	}

    void OnTriggerLeave(Collider col){
        delta = 0;
	}
}
