using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Carry settings")]
    public int capacity = 20;   // max you can hold
    public int carried  = 0;    // current amount

    public int FreeSpace => Mathf.Max(0, capacity - carried);

    // try to add to backpack
    public bool Add(int amount)
    {
        int canTake = Mathf.Min(amount, FreeSpace);
        if (canTake <= 0) return false;
        carried += canTake;
        return true;
    }

    // remove everything you carry (for depositing)
    public int TakeAll()
    {
        int n = carried;
        carried = 0;
        return n;
    }
}
