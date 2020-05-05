using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SeriousCorona
{
    public class Pickable : MonoBehaviour
    {
        private PhotonView view;

        private void Start()
        {
            view = GetComponent<PhotonView>();   
        }

        public void Pick()
        {
            GameManager.instance.Pickup(name);
            view.RPC("DestroyObject", RpcTarget.All);
        }

        [PunRPC]
        private void DestroyObject()
        {
            Destroy(gameObject);

        }
    }
}
