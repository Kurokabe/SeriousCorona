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
                Vector3 oldP = transform.position;

                float H = Input.GetAxis("Horizontal") * scale;
                float V = Input.GetAxis("Vertical") * scale;

                float y = Input.mouseScrollDelta.y;
                float zoomTo = 0;
                if (y > 0.1f)
                {
                    zoomTo = 1f * scaleZoom;
                }

                else if (y <= -0.1f)
                {
                    zoomTo = -1f * scaleZoom;
                }

                transform.position = new Vector3(
                                        Mathf.Clamp(oldP.x + H, 2f, 23f),
                                        Mathf.Clamp(oldP.y - zoomTo, 4f, 25f),
                                        Mathf.Clamp(oldP.z + V, -24f, 28f)
                                        );
            }
        }
    }
}
