using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class GathererAI : MonoBehaviour
{
    [Header("Setup (director fills these)")]
    public ColonyBank myBank;          // where to deposit
    public Transform  depositTarget;   // usually myBank.transform

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float stopDist  = 0.1f;

    [Header("Carry")]
    public int carried = 0;
    public int capacity = 20;

    Rigidbody2D rb;
    Collider2D  col;

    void Awake()
    {
        rb  = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // physics so it can always move
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // X/Y free!

        // (optional) let workers pass through walls easily:
        // col.isTrigger = true;

        // render above ground + z=0
        var sr = GetComponentInChildren<SpriteRenderer>();
        if (sr) { sr.sortingLayerName = "Units"; sr.sortingOrder = 10; }
        var p = transform.position; p.z = 0; transform.position = p;
    }

    void Start() { StartCoroutine(Loop()); }

    IEnumerator Loop()
    {
        while (true)
        {
            // 1) get a node
            var node = FindNearestNode();
            if (node == null) { yield return new WaitForSeconds(0.5f); continue; }

            // 2) go to node
            yield return MoveTo(node.transform.position);

            // 3) harvest once (limited by capacity)
            if (carried < capacity)
            {
                yield return new WaitForSeconds(node.harvestTime);
                if (node.TryHarvest(out int gained))
                    carried = Mathf.Min(capacity, carried + gained);
            }

            // 4) go back to HQ and deposit
            if (depositTarget == null && myBank != null) depositTarget = myBank.transform;
            if (depositTarget != null)
            {
                yield return MoveTo(depositTarget.position);
                if (myBank != null && carried > 0) { myBank.AddFood(carried); carried = 0; }
            }

            yield return null;
        }
    }

    IEnumerator MoveTo(Vector3 target)
    {
        target.z = 0;
        while ((target - transform.position).sqrMagnitude > (stopDist * stopDist))
        {
            Vector2 dir = (target - transform.position).normalized;
            rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    ResourceNode FindNearestNode()
    {
        var nodes = Object.FindObjectsByType<ResourceNode>(FindObjectsSortMode.None);
        if (nodes == null || nodes.Length == 0) return null;

        ResourceNode best = null;
        float bestDist = float.MaxValue;
        Vector3 p = transform.position;
        foreach (var n in nodes)
        {
            float d = (n.transform.position - p).sqrMagnitude;
            if (d < bestDist) { bestDist = d; best = n; }
        }
        return best;
    }
}
