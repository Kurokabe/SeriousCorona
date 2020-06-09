using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace SeriousCorona
{
    public class GameManager : MonoBehaviour, IPunObservable
    {
        public Role role;

        public static GameManager instance;

        [SerializeField]
        private ManagerHandler managerHandler;
        [SerializeField]
        private GameObject managerCamera;
        [SerializeField]
        private GameObject managerCameraPrefab;
        [SerializeField]
        private GameObject finishButton;
        [SerializeField]
        private TextMeshProUGUI remainingTimeText;
        [SerializeField]
        private TextMeshProUGUI maskNumberText;
        [SerializeField]
        private TextMeshProUGUI bottleNumberText;

        [SerializeField]
        private PlayerHandler playerHandler;
        [SerializeField]
        private GameObject playerPrefab;
        [SerializeField]
        private GameObject spawnPlayer;

        public float PlayerTime = 180f;
        private InfectionManger infectionManger;

        private int maskNumber = 0;
        private int bottleNumber = 0;

        public int MaskNumber { get => maskNumber; set { maskNumber = value; maskNumberText.text = maskNumber.ToString(); } }
        public int BottleNumber { get => bottleNumber; set { bottleNumber = value; bottleNumberText.text = bottleNumber.ToString(); } }

        private PhotonView view;

        public enum GameState
        {
            PLANNING,
            PLAYING,
            END
        }
        private GameState gameState;

        public GameState GameStateP
        {
            get => gameState;
            set
            {
                if(gameState != value)
                {
                    gameState = value;
                    HandleGamestate();
                }
            }
        }


        void Start()
        {
            instance = this;

            view = GetComponent<PhotonView>();

            if (PhotonNetwork.LocalPlayer.CustomProperties["Role"] != null)
                role = (Role)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            StartCoroutine(PlayerTimer());
            if (role == Role.MANAGER)
            {
                managerHandler = FindObjectOfType<ManagerHandler>();
                GameObject.Instantiate(managerCameraPrefab, new Vector3(0,10,0), managerCameraPrefab.transform.rotation);
            }
            else if (role == Role.PLAYER)
            {
                if (PhotonNetwork.LocalPlayer.CustomProperties["Role"] == null)
                    GameObject.Instantiate(playerPrefab, spawnPlayer.transform.position, spawnPlayer.transform.rotation);
                playerHandler = FindObjectOfType<PlayerHandler>();
                PhotonNetwork.Instantiate(playerPrefab.name, spawnPlayer.transform.position, spawnPlayer.transform.rotation);
            }
            infectionManger = FindObjectOfType<InfectionManger>();
            //GameStateP = GameState.PLANNING;

        }

        private void HandleGamestate()
        {
            if (GameStateP == GameState.PLANNING)
            {
                if (role == Role.MANAGER)
                {
                    //managerHandler.gameObject.SetActive(true);
                }
                else
                {
                    //finishButton.SetActive(false);
                    //playerHandler.gameObject.SetActive(false);
                }
            }
            else if (GameStateP == GameState.PLAYING)
            {
                StartCoroutine(PlayerTimer());
                managerCamera.SetActive(false);
                finishButton.SetActive(false);
                //playerObject.SetActive(true);
                if (role == Role.PLAYER)
                {
                    print("init");
                    PhotonNetwork.Instantiate(playerPrefab.name, spawnPlayer.transform.position, spawnPlayer.transform.rotation);
                    //playerHandler.gameObject.SetActive(true);                    
                }
                else
                {
                    PhotonNetwork.Instantiate(managerCameraPrefab.name, spawnPlayer.transform.position, spawnPlayer.transform.rotation);
                    //managerHandler.gameObject.SetActive(false);
                }
            }
            else if (GameStateP == GameState.END)
            {
                print($"You've recolted {MaskNumber} Masks and {BottleNumber} bottles of disinfectant");
            }
        }

        IEnumerator PlayerTimer()
        {
            float currentTime = PlayerTime;
            while (currentTime > 0)
            {
                var tmpTime = Time.time;
                yield return new WaitForSeconds(0.1f);
                currentTime -= (Time.time - tmpTime);
                remainingTimeText.text = ((int)currentTime).ToString();
            }
            GameStateP = GameState.END;
        }

        public void FinishPlanning()
        {
            print("next phase");
            GameStateP = GameState.PLAYING;
        }

        void Update()
        {

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
           if(stream.IsWriting)
            {
                stream.SendNext(GameStateP);
            }
           else
            {
                print("State changed " + role); 
                GameStateP = (GameState)stream.ReceiveNext();
            }
        }

        public void Pickup(string item)
        {
            view.RPC("PickupItem", RpcTarget.All, item);
        }

        [PunRPC]
        void PickupItem(string item)
        {
            if (item == "Bottle")
            {
                print("ITEM PICKED : " + item);
                BottleNumber++;
            }
            else if (item == "Maskbox")
            {
                MaskNumber++;
            }
        }

        public void EndGame()
        {
            float infectLevel = 100 - (Mathf.Max(infectionManger.InfectionRate - (maskNumber + bottleNumber), 0));
            int score = maskNumber + bottleNumber;
            float r = Random.value * 100;
            if(r <= infectLevel)
            {
                print("You win with a score of " + score);
            }
            else
            {
                print("You got Coroned, you dumbass");
            }
        }
    }
}