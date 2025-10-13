
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject antPrefab;
    public GameObject beetlePrefab;
    public GameObject mothPrefab;
    public GameObject spiderPrefab;

    [Header("Spawn Points")]
    public Transform antSpawn;
    public Transform beetleSpawn;
    public Transform mothSpawn;
    public Transform spiderSpawn;

    private GameObject currentPlayer;

    void Start()
    {
        // REMOVED: Auto-spawn Ant. The GameInitializer will now handle this.
    }

    // These public methods are called by the GameInitializer script.
    public void SpawnAnt() => Spawn(antPrefab, antSpawn);
    public void SpawnBeetle() => Spawn(beetlePrefab, beetleSpawn);
    public void SpawnMoth() => Spawn(mothPrefab, mothSpawn);
    public void SpawnSpider() => Spawn(spiderPrefab, spiderSpawn);

    private void Spawn(GameObject prefab, Transform spawnPoint)
    {
        // 1. If a player exists, destroy it first (in case you swap mid-game)
        if (currentPlayer != null)
            Destroy(currentPlayer);

        // 2. Create the new player (Beetle, Moth, etc.)
        currentPlayer = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        // 3. Set the camera to follow the new player
        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
        {
            cam.target = currentPlayer.transform;
            // Instantly move the camera to the new player's position
            cam.JumpToTarget(); 
        }
    }
}