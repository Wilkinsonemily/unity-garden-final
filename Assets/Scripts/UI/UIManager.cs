using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Welcome UI")]
    public GameObject welcomePanel;

    [Header("Top Left UI (Tutorial + Collectible)")]
    public GameObject topLeftUI;

    [Header("Buttons")]
    public Button startButton;
    public Button tutorialButton;

    bool paused = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ShowWelcome();

        if (startButton) startButton.onClick.AddListener(StartGame);
        if (tutorialButton) tutorialButton.onClick.AddListener(ToggleTutorial);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleTutorial();
    }

    public void ShowWelcome()
    {
        if (welcomePanel) welcomePanel.SetActive(true);

        if (topLeftUI) topLeftUI.SetActive(false);

        Time.timeScale = 0f;
        paused = true;
    }

    public void StartGame()
    {
        if (welcomePanel) welcomePanel.SetActive(false);

        if (topLeftUI) topLeftUI.SetActive(true);

        Time.timeScale = 1f;
        paused = false;
    }

    public void ToggleTutorial()
    {
        if (paused) StartGame();
        else ShowWelcome();
    }
}
