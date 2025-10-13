using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelection : MonoBehaviour
{
    public Button forestButton;
    public Button caveButton;
    public Button lastMapButton;

    void Start()
    {
        // Default progress: Forest unlocked lang
        if (!PlayerPrefs.HasKey("UnlockedLevel"))
        {
            PlayerPrefs.SetInt("UnlockedLevel", 1);
        }

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel");

        // Enable or disable buttons based on progress
        forestButton.interactable = true;
        caveButton.interactable = (unlockedLevel >= 2);
        lastMapButton.interactable = (unlockedLevel >= 3);
    }

    public void LoadForest()
    {
        SceneManager.LoadScene("ForestScene");
    }

    public void LoadCave()
    {
        SceneManager.LoadScene("CaveScene");
    }

    public void LoadLastMap()
    {
        SceneManager.LoadScene("LastMapScene");
    }
}
