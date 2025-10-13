using UnityEngine;

[System.Serializable]
public class ColonySetup
{
    public string colonyName;         // Ant / Beetle / Moth / Spider
    public ColonyBank bank;           // HQ (with ColonyBank)
    public Transform gathererSpawn;   // spawn Transform
    public GameObject gathererPrefab; // worker prefab (has GathererAI)
}

public class ColonyMatchDirector : MonoBehaviour
{
    public ColonySetup[] colonies;

    void Start()
    {
        string playerColony = PlayerPrefs.GetString("SelectedColony", "Ant");
        Debug.Log($"[Director] PlayerColony = {playerColony}");

        foreach (var c in colonies)
        {
            if (c == null || c.bank == null || c.gathererSpawn == null || c.gathererPrefab == null)
            { Debug.LogWarning("[Director] Missing refs"); continue; }

            if (c.colonyName == playerColony) { Debug.Log($"[Director] Skip {c.colonyName}"); continue; }

            var g = Instantiate(c.gathererPrefab, c.gathererSpawn.position, c.gathererSpawn.rotation);

            // keep on top (z=0)
            var pos = g.transform.position; pos.z = 0; g.transform.position = pos;

            var ai = g.GetComponent<GathererAI>();
            if (ai != null)
            {
                ai.myBank = c.bank;
                ai.depositTarget = c.bank.transform;
                Debug.Log($"[Director] Spawned for {c.colonyName} → bank {c.bank.name}");
            }
            else Debug.LogError("[Director] Gatherer prefab missing GathererAI!");
        }
    }
}
