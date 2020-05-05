using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Role role;


    private Camera handlerCamera;
    private ManagerHandler managerHandler;

    private PlayerHandler playerHandler;
    private Camera playerCamera;

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
            }
            else
            {
                managerHandler.gameObject.SetActive(false);
            }            
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
