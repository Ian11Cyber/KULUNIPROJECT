using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ResourceNode : MonoBehaviour
{
    [Header("Harvest settings")]
    public float harvestTime = 1.0f; // seconds AI waits at node
    public int   yield       = 5;    // amount per harvest
    public int   remaining   = -1;   // -1 = infinite

    // AI harvest
    public bool TryHarvest(out int gained)
    {
        gained = 0;
        if (remaining == 0) return false;

        int give = remaining > 0 ? Mathf.Min(yield, remaining) : yield;
        gained = give;
        if (remaining > 0) remaining -= give;
        return true;
    }

    // Player pickup (E key)
    public bool TryPickup(PlayerInventory inv)
    {
        if (inv == null) return false;
        if (remaining == 0) return false;

        int give = remaining > 0 ? Mathf.Min(yield, remaining) : yield;
        int canTake = Mathf.Min(give, inv.FreeSpace);
        if (canTake <= 0) return false;

        inv.Add(canTake);
        if (remaining > 0) remaining -= canTake;
        return true;
    }
}
