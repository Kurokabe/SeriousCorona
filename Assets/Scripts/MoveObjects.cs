using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    private GameObject selectedObject;
    private GameObject hoverObject;
    private Outline hoverObjectOutline;
    private Rigidbody selectedObjectRb;
    private Outline selectedObjectOutline;
    private Photon.Pun.PhotonView selectedObjectPV;

    private int RAYCAST_DIST = 1000;

    public float scale;

    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, RAYCAST_DIST))
        {
            if (hit.collider.gameObject != null && hit.collider.gameObject.CompareTag("MoveableObj") && hit.collider.gameObject != selectedObject)
            {
                if (hit.collider.gameObject != hoverObject)
                {
                    RemoveOutlineHover();
                    hoverObject = hit.collider.gameObject;
                    if (!hoverObject.TryGetComponent(out Outline outline))
                    {
                        hoverObjectOutline = hoverObject.AddComponent<Outline>();
                        hoverObjectOutline.OutlineColor = Color.blue;
                    }
                }            
            }
            else
            {
                RemoveOutlineHover();
            }
        }
        else
        {
            RemoveOutlineHover();
        }

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

    private void RemoveOutlineHover()
    {
        if ((selectedObject != null && hoverObject != selectedObject) || selectedObject == null)
        {
            hoverObject = null;
            DestroyImmediate(hoverObjectOutline);
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

                if(selectedObject == hoverObject)
                    selectedObjectOutline = selectedObject.GetComponent<Outline>();
                else
                    selectedObjectOutline = selectedObject.AddComponent<Outline>();

                selectedObjectPV = selectedObject.GetComponent<Photon.Pun.PhotonView>();
                selectedObjectPV.RequestOwnership();
                selectedObjectRb = selectedObject.GetComponent<Rigidbody>();                
                selectedObjectOutline.OutlineColor = Color.yellow;
            }
        }
    }
}
