using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject QuitButton;
    [SerializeField] private GameManagerScript gameManagerScript;
    private int selectedButton;

    //Input
    private bool upDpadPress = false;
    private bool downDpadPress = false;
    private bool selectButton_state_toggled = false;

    public void OnUpButtonPress(InputAction.CallbackContext ctx) => upDpadPress = ctx.action.WasPressedThisFrame();
    public void OnDownButtonPress(InputAction.CallbackContext ctx) => downDpadPress = ctx.action.WasPressedThisFrame();
    public void OnSelect(InputAction.CallbackContext ctx) => selectButton_state_toggled = ctx.action.WasPressedThisFrame();

    // Start is called before the first frame update
    void Start()
    {
        StartButton = GameObject.FindGameObjectWithTag("StartButton");
        QuitButton = GameObject.FindGameObjectWithTag("QuitButton");
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        selectedButton = 0;
    }

    // Update is called once per frame
    void Update()
    {
        print(selectedButton);
        if (upDpadPress)
        {
            Debug.Log("start pressed");
            selectedButton = 0;
            StartButton.GetComponent<Button>().Select();
        }
        else if (downDpadPress)
        {
            Debug.Log("quit pressed");
            selectedButton = 1;
            QuitButton.GetComponent<Button>().Select();
        }

        if (selectedButton == 0)
        {
            StartButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            QuitButton.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            QuitButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            StartButton.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        if (selectButton_state_toggled)
        {
            if (selectedButton == 0 && gameManagerScript.enoughPlayers)
            {
                Debug.Log("start");
                gameManagerScript.isPlaying = true;
                gameManagerScript.isMenu = false;
            }
            else if (selectedButton == 1)
            {
                Debug.Log("quit");
                
            }
        }
    }
}
