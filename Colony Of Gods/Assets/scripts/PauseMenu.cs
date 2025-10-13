using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   // Keep this for UI components like Slider and Image
using UnityEngine.InputSystem;  // Keep this for the new Input System

public class PauseMenu : MonoBehaviour
{
    [Header("UI (assign in inspector)")]
    public GameObject pausePanel;     
    public GameObject settingsPanel;   
    public GameObject darkOverlay;     

    [Header("Optional")]
    public GameObject hudPauseButton;  
    public GameObject player;          

    [Header("Scene names")]
    public string mainMenuSceneName = "MainMenu"; 

    [Header("Brightness Settings")] // Keep the brightness settings
    public Slider brightnessSlider;       
    public Image brightnessOverlay;       

    bool isPaused = false;

    void Start()
    {
        // make sure pause-related UI is hidden at start
        if (pausePanel) pausePanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);
        if (darkOverlay) darkOverlay.SetActive(false);

        // HUWAG gagalawin ang brightnessOverlay dito
        // brightnessOverlay must stay active para gumana lagi

        Time.timeScale = 1f;
        AudioListener.pause = false;

        // setup brightness slider
        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.AddListener(SetBrightness);

            // load saved brightness (default 0.7 kung wala pa)
            float savedValue = PlayerPrefs.GetFloat("Brightness", 0.7f);
            brightnessSlider.value = savedValue;
            SetBrightness(savedValue);
        }
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        // Escape key to toggle pause (using new Input System)
        if (kb.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;

        if (pausePanel) pausePanel.SetActive(true);
        if (darkOverlay) darkOverlay.SetActive(true);
        if (settingsPanel) settingsPanel.SetActive(false);

        Time.timeScale = 0f;
        AudioListener.pause = true;

        if (player) player.SendMessage("OnPause", SendMessageOptions.DontRequireReceiver);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        isPaused = false;

        if (pausePanel) pausePanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);
        if (darkOverlay) darkOverlay.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (player) player.SendMessage("OnResume", SendMessageOptions.DontRequireReceiver);
    }

    public void ToggleSettings()
    {
        if (settingsPanel == null || pausePanel == null) return;
        bool opening = !settingsPanel.activeSelf;
        settingsPanel.SetActive(opening);
        pausePanel.SetActive(!opening);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // =====================
    // Brightness Functions
    // =====================
    public void SetBrightness(float value)
    {
        if (brightnessOverlay == null) return;

        // slider value 0–1 (1 = maliwanag, 0 = madilim)
        Color c = brightnessOverlay.color;
        c.a = 1f - value;
        brightnessOverlay.color = c;

        // save value
        PlayerPrefs.SetFloat("Brightness", value);
    }
}
