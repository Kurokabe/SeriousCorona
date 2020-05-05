using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousCorona
{
    public class ManagerCameraHandler : MonoBehaviour
    {
        private Camera managerCamera;
        private GameManager gameManager;
        // Start is called before the first frame update
        void Start()
        {
            managerCamera = GetComponent<Camera>();
            gameManager = FindObjectOfType<GameManager>();
        }
        public float scale;
        public float scaleZoom;

        void Update()
        {
            if (gameManager.role == Role.MANAGER)
            {
                float H = Input.GetAxis("Horizontal") * scale;
                float V = Input.GetAxis("Vertical") * scale;

                float y = Input.mouseScrollDelta.y;
                float zoomTo = 0;
                if (y > 0.1f)
                {
                    zoomTo = 1f * scaleZoom;
                    Debug.Log("Zoomed In");
                }

                else if (y <= -0.1f)
                {
                    zoomTo = -1f * scaleZoom;
                    Debug.Log("Zoomed Out");
                }

                //float newY = Mathf.Clamp(transform.position.y + zoomTo, 8f, 50f);

                Vector3 move = new Vector3(H, V, zoomTo);
                print(move);
                transform.Translate(move);
            }
        }
    }
}
