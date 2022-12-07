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

    private void Update()
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

    public void StartGame()
    {
        isPlaying = true;
        isMenu = false;
    }

    public void PauseGame()
    {
        isPlaying = false;
        isMenu = true;
    }

}
