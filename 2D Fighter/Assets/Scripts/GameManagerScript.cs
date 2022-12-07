using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerScript : MonoBehaviour
{

    [SerializeField] private PlayerInputManager pInputMan;
    public bool isPlaying;
    public bool isMenu;
    public bool enoughPlayers;

    private void Start()
    {
        isPlaying = false;
        isMenu = true;
    }

    public void StartGame()
    {
        if (pInputMan.playerCount >= 2)
        {
            enoughPlayers = true;
        }
        else
        {
            enoughPlayers = false;
        }
    }

}
