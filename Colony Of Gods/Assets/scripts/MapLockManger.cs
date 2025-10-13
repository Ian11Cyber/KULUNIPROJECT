using UnityEngine;
using UnityEngine.UI;

public class MapLockManager : MonoBehaviour
{
    public Button forestButton;
    public Button caveButton;
    public Button desertButton;

    public GameObject caveOverlay;
    public GameObject desertOverlay;

    void Start()
    {
        // Forest = laging bukas
        forestButton.interactable = true;

        // Cave
        bool caveUnlocked = PlayerPrefs.GetInt("CaveUnlocked", 0) == 1;
        caveButton.interactable = caveUnlocked;
        caveOverlay.SetActive(!caveUnlocked);

        // Desert
        bool desertUnlocked = PlayerPrefs.GetInt("DesertUnlocked", 0) == 1;
        desertButton.interactable = desertUnlocked;
        desertOverlay.SetActive(!desertUnlocked);
    }

    public void UnlockMap(string mapName)
    {
        PlayerPrefs.SetInt(mapName + "Unlocked", 1);
        PlayerPrefs.Save();
        Start(); // i-refresh ang status ng buttons at overlays
    }
}
