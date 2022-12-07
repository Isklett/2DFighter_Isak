using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinScript : MonoBehaviour
{
    [SerializeField] private GameObject textRed;
    [SerializeField] private GameObject textBlue;
    [SerializeField] private GameObject lifeTextRed;
    [SerializeField] private GameObject lifeTextBlue;
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject[] spawnLocation;
    [SerializeField] private GameManagerScript gameManagerScript;
    [SerializeField] private GameObject WinText;
    private PlayerInput[] playerInputs;
    private InputActionMap uiMap1;
    private InputActionMap uiMap2;
    private InputActionMap playerMap1;
    private InputActionMap playerMap2;

    private void Start()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    //Variabler får värden av den nya spelaren som går med och spelarens position sätts antingen till höger eller vänster beroende på om den gick med först eller sist
    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.playerIndex == 0)
        {
            player1 = playerInput.gameObject;
            player1.GetComponent<MenuScript>().whatPlayer = playerInput.playerIndex;
            uiMap1 = playerInput.actions.FindActionMap("UI");
            playerMap1 = playerInput.actions.FindActionMap("Player");
            playerInput.gameObject.transform.position = spawnLocation[0].transform.position;
        }
        else if (playerInput.playerIndex == 1)
        {
            player2 = playerInput.gameObject;
            player2.GetComponent<MenuScript>().whatPlayer = playerInput.playerIndex;
            uiMap2 = playerInput.actions.FindActionMap("UI");
            playerMap2 = playerInput.actions.FindActionMap("Player");
            playerInput.gameObject.transform.SetPositionAndRotation(spawnLocation[1].transform.position, Quaternion.Euler(0, 270, 0));
        }
    }

    private void Update()
    {
        if (gameObject.GetComponent<PlayerInputManager>().playerCount >= 2)
        {
            gameObject.GetComponent<PlayerInputManager>().DisableJoining();
        }
        else
        {
            gameObject.GetComponent<PlayerInputManager>().EnableJoining();
        }

        //Uppdaterar UI text för liv
        if (gameObject.GetComponent<PlayerInputManager>().playerCount == 1)
        {
            textRed.GetComponent<TMPro.TextMeshProUGUI>().text = System.Math.Round(player1.GetComponentInChildren<AttackScript>().health, 1).ToString() + "%";
            lifeTextRed.GetComponent<TMPro.TextMeshProUGUI>().text = player1.GetComponent<CharacterScript>().health.ToString();
        }
        else if (gameObject.GetComponent<PlayerInputManager>().playerCount == 2)
        {
            textRed.GetComponent<TMPro.TextMeshProUGUI>().text = System.Math.Round(player1.GetComponentInChildren<AttackScript>().health, 1).ToString() + "%";
            textBlue.GetComponent<TMPro.TextMeshProUGUI>().text = System.Math.Round(player2.GetComponentInChildren<AttackScript>().health, 1).ToString() + "%";
            lifeTextRed.GetComponent<TMPro.TextMeshProUGUI>().text = player1.GetComponent<CharacterScript>().health.ToString();
            lifeTextBlue.GetComponent<TMPro.TextMeshProUGUI>().text = player2.GetComponent<CharacterScript>().health.ToString();
        }

        //Byter mellan att styre spelare och UI
        if (gameManagerScript.isPlaying)
        {
            if (gameObject.GetComponent<PlayerInputManager>().playerCount >= 1)
            {
                playerMap1.Enable();
                uiMap1.Disable();
            }
            if (gameObject.GetComponent<PlayerInputManager>().playerCount == 2)
            {
                playerMap2.Enable();
                uiMap2.Disable();
            }
        }
        else if (gameManagerScript.isMenu)
        {
            if (gameObject.GetComponent<PlayerInputManager>().playerCount >= 1)
            {
                playerMap1.Disable();
                uiMap1.Enable();
            }
            if (gameObject.GetComponent<PlayerInputManager>().playerCount == 2)
            {
                playerMap2.Disable();
                uiMap2.Enable();
            }
        }
    }
}
