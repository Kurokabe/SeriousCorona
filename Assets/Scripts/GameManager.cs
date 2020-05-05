using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
        private GameObject finishButton;

        [SerializeField]
        private PlayerHandler playerHandler;
        [SerializeField]
        private GameObject playerObject;
        public GameObject spawnPlayer;

        private const string PREFAB_NAME = "ThirdPersonController";

        public enum GameState
        {
            PLANNING,
            PLAYING
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
            if(PhotonNetwork.LocalPlayer.CustomProperties["Role"] != null)
                role = (Role)PhotonNetwork.LocalPlayer.CustomProperties["Role"];

            if (role == Role.MANAGER)
            {
                managerHandler = FindObjectOfType<ManagerHandler>();
            }
            else if (role == Role.PLAYER)
            {
                playerHandler = FindObjectOfType<PlayerHandler>();
            }
            GameStateP = GameState.PLANNING;

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
                managerCamera.SetActive(false);
                finishButton.SetActive(false);
                //playerObject.SetActive(true);
                if (role == Role.PLAYER)
                {
                    print("init");
                    PhotonNetwork.Instantiate(PREFAB_NAME, spawnPlayer.transform.position, spawnPlayer.transform.rotation);
                    //playerHandler.gameObject.SetActive(true);                    
                }
                else
                {
                    //managerHandler.gameObject.SetActive(false);
                }
            }
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
    }
}