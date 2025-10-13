using UnityEngine;

public class ColonyBank : MonoBehaviour
{
    public string colonyName;   // Ant / Beetle / Moth / Spider
    public int food = 0;        // start at 0

    public void AddFood(int amount) { food += Mathf.Max(0, amount); }
    public bool SpendFood(int amount)
    {
        if (food < amount) return false;
        food -= amount;
        return true;
    }
}
