using UnityEngine;
using TMPro;

public class FoodUI : MonoBehaviour
{
    public ColonyBank playerBank;   // leave empty to auto-detect
    public TMP_Text label;          // assign your TMP Text in Inspector

    void Start()
    {
        // Auto-find the selected colony's bank if not assigned
        if (playerBank == null)
        {
            string selected = PlayerPrefs.GetString("SelectedColony", "Ant");
            var banks = Object.FindObjectsByType<ColonyBank>(FindObjectsSortMode.None);
            foreach (var b in banks)
            {
                if (string.Equals(b.colonyName, selected, System.StringComparison.OrdinalIgnoreCase))
                {
                    playerBank = b;
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (playerBank != null && label != null)
            label.text = $"Food: {playerBank.food}";
    }
}
