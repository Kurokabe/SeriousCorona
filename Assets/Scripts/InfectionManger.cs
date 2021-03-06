﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace SeriousCorona
{
    public class InfectionManger : MonoBehaviour
    {

        private TextMeshProUGUI infectionRateText;

        private int infectionRate = 0;
        public int InfectionRate { get => infectionRate; set { 
                infectionRate = Mathf.Clamp(value, 0, 100); 
                infectionRateText.text = infectionRate.ToString() + " %";
                GameManager.instance.InfectionRate = infectionRate;
            } }

        private double delta = 0.5;
        private bool hasExitEndZone = false;

        void Start()
        {
            infectionRateText = GameObject.Find("InfectionRateLabel").GetComponent<TextMeshProUGUI>();
            InfectionRate = 20;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.CompareTag("EndZone") && hasExitEndZone)
            {
                GameManager.instance.EndGame(Random.value * 100);
            }
        }

        void OnTriggerStay(Collider col)
        {
            if (col.gameObject.CompareTag("enemy"))
            {
                delta += Time.deltaTime;

                if (delta >= 0.5f)
                {
                    InfectionRate++;
                    delta = 0;
                }
            }
        }

        void OnTriggerExit(Collider col)
        {
            delta = 0;
            hasExitEndZone = true;
        }

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.CompareTag("Doctor"))
            {
                GameManager.instance.EndGame(Random.value * 100, hasBeenCatch:true);
            }
        }
    }
}
