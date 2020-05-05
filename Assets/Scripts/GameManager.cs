using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Role role;

    [SerializeField]
    private Camera handlerCamera;
    [SerializeField]
    private ManagerHandler managerHandler;

    [SerializeField]
    private PlayerHandler playerHandler;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private GameObject playerObject;

    public enum GameState
    {
        PLANNING,
        PLAYING
    }
    public GameState gameState;

    void Start()
    {
        role = (Role) PhotonNetwork.LocalPlayer.CustomProperties["Role"];
        if(role == Role.MANAGER)
        {
            managerHandler = FindObjectOfType<ManagerHandler>();
        }
        else if(role == Role.PLAYER)
        {
            playerHandler = FindObjectOfType<PlayerHandler>();
        }
        gameState = GameState.PLANNING;

    }

    private void HandleGamestate()
    {
        if(gameState == GameState.PLANNING)
        {
            if(role == Role.MANAGER)
            {
                managerHandler.gameObject.SetActive(true);
            }
            else
            {
                playerHandler.gameObject.SetActive(false);
            }            
        }
        else if(gameState == GameState.PLAYING)
        {
            if (role == Role.PLAYER)
            {
                playerHandler.gameObject.SetActive(true);
                playerObject.SetActive(true);
            }
            else
            {
                managerHandler.gameObject.SetActive(false);
            }            
        }
    }

    public void FinishPlanning()
    {
        print("next phase");
        gameState = GameState.PLAYING;
        HandleGamestate();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
