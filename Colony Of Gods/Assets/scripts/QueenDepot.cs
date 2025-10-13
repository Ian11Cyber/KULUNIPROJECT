using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class QueenDepot : MonoBehaviour
{
    public ColonyBank bank;   // drag the matching HQ (with ColonyBank) here

    public void Deposit(PlayerInventory inv)
    {
        if (inv == null || bank == null) return;
        int delivered = inv.TakeAll();
        if (delivered > 0) bank.AddFood(delivered);
    }
}
