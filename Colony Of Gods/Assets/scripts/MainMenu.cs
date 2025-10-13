using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // palitan ito sa mismong pangalan ng Colony Selection scene mo
        SceneManager.LoadScene("Colony Selection");
    }

    public void OpenSettings()
    {
        // dito mo pwede i-load yung settings scene mo kung meron
        SceneManager.LoadScene("SettingsScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
