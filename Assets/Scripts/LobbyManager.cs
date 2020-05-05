using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using ExitGames.Client.Photon;

namespace Com.MyCompany.MyGame
{
    public enum Role
    {
        NONE,
        PLAYER,
        MANAGER
    }

    public class LobbyManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
    {

        #region Private Serialize Fields
        [SerializeField]
        private  Button buttonPlayer;
        [SerializeField]
        private Button buttonManager;
        [SerializeField]
        private Button buttonPlay;

        [SerializeField]
        private TextMeshProUGUI textPlayer;
        [SerializeField]
        private TextMeshProUGUI textManager;
        #endregion

        #region Private Fields
        private const string propRole = "Role";
        private Role role;
        #endregion

        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                buttonPlay.gameObject.SetActive(true);
            }
            else
            {
                buttonPlay.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            if (buttonPlay.gameObject.activeSelf)
            {
                buttonPlay.interactable = textPlayer.text != "" && textManager.text != "";
            }
        }
        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void StartGame()
        {
            PhotonNetwork.LoadLevel("SampleScene");
        }
        
        public void SelectPlayer()
        {
            SetRole(Role.PLAYER);
            buttonManager.interactable = false;
        }

        public void SelectManager()
        {
            SetRole(Role.MANAGER);
            buttonPlayer.interactable = false;
        }
        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            SetRole(role);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);

            SelectRole(Role.NONE, otherPlayer.NickName);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
            SelectRole((Role)changedProps[propRole], targetPlayer.NickName);
        }

        

        #endregion

        #region Private Methods
        private void SetRole(Role role)
        {
            this.role = role;
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties.Add(propRole, role);

            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            //PhotonNetwork.LocalPlayer.CustomProperties = playerProperties;
        }
        private void SelectRole(Role role, string nickname)
        {
            if (role == Role.PLAYER)
            {
                buttonPlayer.interactable = false;
                textPlayer.text = nickname;
            }
            else if (role == Role.MANAGER)
            {
                buttonManager.interactable = false;
                textManager.text = nickname;
            }
            else
            {
                if (textManager.text == nickname)
                {
                    buttonManager.interactable = buttonPlayer.interactable;
                    textManager.text = "";
                }
                else if (textPlayer.text == nickname)
                {
                    buttonPlayer.interactable = buttonManager.interactable;
                    textPlayer.text = "";
                }
            }
        }
        #endregion

        #region IInRoomCallbacks
        void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
        {
            if (newMasterClient.Equals(PhotonNetwork.LocalPlayer))
            {
                buttonPlay.gameObject.SetActive(true);
            }
        }
        #endregion
    }
}