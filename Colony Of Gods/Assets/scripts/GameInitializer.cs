using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    // Drag your PlayerSpawner script's GameObject here in the Inspector.
    public PlayerSpawner playerSpawner; 

    void Start()
    {
        // 1. Get the colony name saved by the ColonySelectionMenu
        // We default to "Ant" in case the game is started without the menu.
        string selectedColony = PlayerPrefs.GetString("SelectedColony", "Ant"); 
        
        Debug.Log("[GameInitializer] Spawning Player: " + selectedColony);

        // Make sure the spawner is assigned
        if (playerSpawner == null)
        {
            Debug.LogError("PlayerSpawner reference is missing! Assign it in the Inspector.");
            return;
        }

        // 2. Call the appropriate spawn method on the PlayerSpawner
        switch (selectedColony)
        {
            case "Ant":
                playerSpawner.SpawnAnt();
                break;
            case "Beetle":
                playerSpawner.SpawnBeetle();
                break;
            case "Moth":
                playerSpawner.SpawnMoth();
                break;
            case "Spider":
                playerSpawner.SpawnSpider();
                break;
            default:
                Debug.LogWarning("Unknown colony selected: " + selectedColony + ". Defaulting to Ant.");
                playerSpawner.SpawnAnt();
                break;
        }

        // OPTIONAL: Clean up the selection after use
        // PlayerPrefs.DeleteKey("SelectedColony");
    }
}