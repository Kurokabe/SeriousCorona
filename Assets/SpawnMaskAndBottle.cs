using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnMaskAndBottle : MonoBehaviour
{
    public float proba;
    public GameObject spawnPoints;

    public GameObject prefabMask;
    public GameObject prefabBottle;
    

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform t in spawnPoints.transform)
        {
            float r = Random.value;
            if(r < proba)
            {
               PhotonNetwork.Instantiate((r < proba / 2 ? prefabMask.name : prefabBottle.name), t.position, t.rotation);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
