using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionManger : MonoBehaviour
{
    
    private int infectionRate = 0;
    public int InfectionRate { get => infectionRate; set { infectionRate = value; infectionRateText.text = infectionRate.ToString() + "%"; } }
    
    private float delta = 0.5; 

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
        delta = 0f;
	}
}
