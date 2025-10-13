using UnityEngine;
using UnityEngine.SceneManagement;

public class ColonySelectionMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject colonyPanel;   // SELECT panel (visible initially)
    public GameObject mapSelection;  // map selection panel (initially inactive)

    // Called by UI Buttons (Inspector): use SelectAnt/SelectBeetle/...
    public void SelectColony(string colonyName)
    {
        PlayerPrefs.SetString("SelectedColony", colonyName);
        Debug.Log("[ColonySelection] Selected: " + colonyName);

        if (colonyPanel != null) colonyPanel.SetActive(false);
        if (mapSelection != null) mapSelection.SetActive(true);
    }

    public void SelectAnt()    { SelectColony("Ant"); }
    public void SelectBeetle() { SelectColony("Beetle"); }
    public void SelectMoth()   { SelectColony("Moth"); }
    public void SelectSpider() { SelectColony("Spider"); }

    // Called by map buttons: provide exact scene name string in inspector
    public void SelectMap(string mapName)
    {
        string colony = PlayerPrefs.GetString("SelectedColony", "Ant");
        Debug.Log("[ColonySelection] Loading Map: " + mapName + " | Colony: " + colony);
        SceneManager.LoadScene(mapName);
    }
}
