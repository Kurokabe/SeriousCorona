using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    private GameObject selectedObject;
    private Rigidbody selectedObjectRb;
    private Outline selectedObjectOutline;
    private Photon.Pun.PhotonView selectedObjectPV;

    private int RAYCAST_DIST = 1000;

    public float scale;

    void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            selectOnClick();            
        }
        
        if(selectedObject != null)
        {
            if (Input.GetMouseButtonDown(1) && selectedObject != null)
            {
                Destroy(selectedObjectOutline);
                selectedObjectOutline = null;
                selectedObject = null;
            }
            else
            {
                print(selectedObjectPV.Owner);
                if(selectedObjectPV.Owner != null && selectedObjectPV.Owner.UserId != Photon.Pun.PhotonNetwork.LocalPlayer.UserId)
                {
                    selectedObject.GetComponent<Photon.Pun.PhotonView>().RequestOwnership();
                }
                
                Vector3 oldPos = selectedObject.transform.position;
                float H = Input.GetAxis("HorizontalArrow") * scale;
                float V = Input.GetAxis("VerticalArrow") * scale;
                selectedObjectRb.MovePosition(new Vector3(oldPos.x + H, oldPos.y, oldPos.z + V));
                // selectedObjectRb.AddForce(new Vector3(H, 0, V));
            }
        }

        
    }
    private void selectOnClick()
    {
        Destroy(selectedObjectOutline);
        selectedObject = null;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, RAYCAST_DIST))
        {

            if (hit.collider.gameObject != null && hit.collider.gameObject.CompareTag("MoveableObj"))
            {
                selectedObject = hit.collider.gameObject;
                selectedObjectPV = selectedObject.GetComponent<Photon.Pun.PhotonView>();
                selectedObjectPV.RequestOwnership();
                selectedObjectRb = selectedObject.GetComponent<Rigidbody>();
                selectedObjectOutline = selectedObject.AddComponent<Outline>();
                selectedObjectOutline.OutlineColor = Color.yellow;
            }
        }
    }
}
