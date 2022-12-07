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
    private PlayerInput[] playerInputs;

    private void Start()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }
    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.playerIndex == 0)
        {
            player1 = playerInput.gameObject;
            playerInput.gameObject.transform.position = spawnLocation[0].transform.position;
        }
        else if (playerInput.playerIndex == 1)
        {
            player2 = playerInput.gameObject;
            playerInput.gameObject.transform.SetPositionAndRotation(spawnLocation[1].transform.position, Quaternion.Euler(0, 270, 0));
        }

        playerInputs[playerInput.playerIndex] = playerInput;

    }

    private void Update()
    {
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

        //if (gameManagerScript.isPlaying)
        //{
        //    print("tjo");
        //    if (gameObject.GetComponent<PlayerInputManager>().playerCount >= 1)
        //        playerInputs[0].SwitchCurrentActionMap("Player");
        //    if (gameObject.GetComponent<PlayerInputManager>().playerCount == 2)
        //        playerInputs[1].SwitchCurrentActionMap("Player");
        //}
        //else if(gameManagerScript.isMenu)
        //{
        //    print("hej2");
        //    if (gameObject.GetComponent<PlayerInputManager>().playerCount >= 1)
        //        playerInputs[0].SwitchCurrentActionMap("UI");
        //    if (gameObject.GetComponent<PlayerInputManager>().playerCount == 2)
        //        playerInputs[1].SwitchCurrentActionMap("UI");
        //}
    }
}
