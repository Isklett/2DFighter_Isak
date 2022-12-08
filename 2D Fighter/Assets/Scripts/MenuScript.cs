using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject QuitButton;
    //[SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject startMenuUIStart;
    [SerializeField] private GameObject startMenuUIQuit;
    [SerializeField] private GameManagerScript gameManagerScript;
    public bool hasStarted;
    public static bool GameIsPaused;
    public int whatPlayer;
    private int selectedButton;
    private Vector3 tempQuitSize;
    private Vector3 tempStartSize;
    private bool femaleChar;
    [SerializeField] private List<GameObject> femaleParts;
    [SerializeField] private List<GameObject> maleParts;
    private int changeTimer;
    private bool canChange;

    //Input
    private bool upDpad_state_toggled = false;
    private bool downDpad_state_toggled = false;
    private bool selectButton_state_toggled = false;
    private bool pauseButton_state_toggled = false;
    private bool changeCharButton_state_toggled = false;

    public void OnUpButtonPress(InputAction.CallbackContext ctx) => upDpad_state_toggled = ctx.action.WasPressedThisFrame();
    public void OnDownButtonPress(InputAction.CallbackContext ctx) => downDpad_state_toggled = ctx.action.WasPressedThisFrame();
    public void OnSelect(InputAction.CallbackContext ctx) => selectButton_state_toggled = ctx.action.WasPressedThisFrame();
    public void OnPause(InputAction.CallbackContext ctx) => pauseButton_state_toggled = ctx.action.WasPressedThisFrame();
    public void OnCharChange(InputAction.CallbackContext ctx) => changeCharButton_state_toggled = ctx.action.WasPressedThisFrame();

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        StartButton = GameObject.FindGameObjectWithTag("StartButton");
        QuitButton = GameObject.FindGameObjectWithTag("QuitButton");
        selectedButton = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (whatPlayer == 0)
        {
            //Första spelaren kan styra menyn. Ändrar storleken på den valda knappen och startar/fortsätter eller stänger av spelet
            if (GameIsPaused || !hasStarted)
            {
                if (selectedButton == 0)
                {
                    tempStartSize = new Vector3(1.2f, 1.2f, 1.2f);
                    tempQuitSize = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else
                {
                    tempStartSize = new Vector3(1.0f, 1.0f, 1.0f);
                    tempQuitSize = new Vector3(1.2f, 1.2f, 1.2f);
                }
                StartButton.transform.localScale = tempStartSize;
                QuitButton.transform.localScale = tempQuitSize;
            }
            if (upDpad_state_toggled)
            {
                selectedButton = 0;
                print(selectedButton);
            }
            else if (downDpad_state_toggled)
            {
                selectedButton = 1;
                print(selectedButton);
            }

            if (selectButton_state_toggled && !hasStarted)
            {
                if (selectedButton == 0 && gameManagerScript.enoughPlayers)
                {
                    StartPlaying();
                }
                else if (selectedButton == 1)
                {
                    Application.Quit();
                }
            }
            else if (selectButton_state_toggled && GameIsPaused)
            {
                if (selectedButton == 0)
                {
                    Resume();
                }
                else
                {
                    Application.Quit();
                }
            }

            if (pauseButton_state_toggled && hasStarted)
            {
                if (!GameIsPaused)
                {
                    Pause();
                }
            }
            if (hasStarted)
            {
                StartButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "RESUME";
            }
            else
            {
                StartButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "START";
            }
        }
        if (!hasStarted && canChange)
        {
            ChangeChar();
        }
    }

    private void FixedUpdate()
    {
        print(canChange);
        if (changeTimer > 0)
        {
            changeTimer--;
        }
        else
        {
            canChange = true;
        }
    }

    public IEnumerator ChangeCharCR()
    {
        if (femaleChar)
        {
            femaleParts[0].gameObject.SetActive(true);
            femaleParts[1].gameObject.SetActive(true);
            femaleParts[2].gameObject.SetActive(true);
            femaleParts[3].gameObject.SetActive(true);
            femaleParts[4].gameObject.SetActive(true);
            femaleParts[5].gameObject.SetActive(true);
            maleParts[0].gameObject.SetActive(false);
            maleParts[1].gameObject.SetActive(false);
            maleParts[2].gameObject.SetActive(false);
            maleParts[3].gameObject.SetActive(false);
            maleParts[4].gameObject.SetActive(false);
            maleParts[5].gameObject.SetActive(false);
            femaleChar = false;
        }
        else
        {
            femaleParts[0].gameObject.SetActive(false);
            femaleParts[1].gameObject.SetActive(false);
            femaleParts[2].gameObject.SetActive(false);
            femaleParts[3].gameObject.SetActive(false);
            femaleParts[4].gameObject.SetActive(false);
            femaleParts[5].gameObject.SetActive(false);
            maleParts[0].gameObject.SetActive(true);
            maleParts[1].gameObject.SetActive(true);
            maleParts[2].gameObject.SetActive(true);
            maleParts[3].gameObject.SetActive(true);
            maleParts[4].gameObject.SetActive(true);
            maleParts[5].gameObject.SetActive(true);
            femaleChar = true;
        }
        canChange = false;
        changeTimer = 10;
        yield return new WaitForSeconds(0.1f);
    }

    void ChangeChar()
    {
        if (changeCharButton_state_toggled)
        {
            StartCoroutine(ChangeCharCR());
        }
    }

    void StartPlaying()
    {
        StartButton.GetComponentInChildren<Image>().enabled = false;
        StartButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = false;
        QuitButton.GetComponentInChildren<Image>().enabled = false;
        QuitButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = false;
        GameIsPaused = false;
        hasStarted = true;
        gameManagerScript.StartGame();
    }

    void Resume()
    {
        StartButton.GetComponentInChildren<Image>().enabled = false;
        StartButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = false;
        QuitButton.GetComponentInChildren<Image>().enabled = false;
        QuitButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = false;
        Time.timeScale = 1f;
        GameIsPaused = false;
        gameManagerScript.StartGame();
    }

    void Pause()
    {
        StartButton.GetComponentInChildren<Image>().enabled = true;
        StartButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = true;
        QuitButton.GetComponentInChildren<Image>().enabled = true;
        QuitButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = true;
        Time.timeScale = 0f;
        GameIsPaused = true;
        gameManagerScript.PauseGame();
    }
}
