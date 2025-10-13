using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractor : MonoBehaviour
{
    public PlayerInventory inventory;   // drag your PlayerInventory here in Inspector

    ResourceNode nearNode;              // set when you enter a node trigger
    QueenDepot   nearDepot;             // set when you enter the queen's deposit trigger

    void Reset()
    {
        // auto link if on the same object
        inventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        // keyboard fallback (mobile button can call PressInteract() too)
        if (Input.GetKeyDown(KeyCode.E)) PressInteract();
    }

    // Call this from your mobile Interact button
    public void PressInteract()
    {
        if (inventory == null) return;

        if (nearNode != null)
        {
            nearNode.TryPickup(inventory);      // pick up from resource
            return;
        }
        if (nearDepot != null)
        {
            nearDepot.Deposit(inventory);       // deposit to queen/bank
            return;
        }
    }

    // detect triggers (requires ResourceNode/QueenDepot colliders to be "Is Trigger")
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.TryGetComponent(out ResourceNode node)) nearNode = node;
        if (c.TryGetComponent(out QueenDepot depot))  nearDepot = depot;
    }
    void OnTriggerExit2D(Collider2D c)
    {
        if (c.TryGetComponent(out ResourceNode node) && node == nearNode) nearNode = null;
        if (c.TryGetComponent(out QueenDepot depot)  && depot == nearDepot) nearDepot = null;
    }
}
