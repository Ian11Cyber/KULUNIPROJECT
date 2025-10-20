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

    [Tooltip("Obstacle layers (your TilemapCollider2D walls/ground)")]
    public LayerMask obstacleMask;
    [Tooltip("How far the feeler rays check ahead")]
    public float lookAhead = 0.7f;
    [Tooltip("How strongly to avoid obstacles")]
    public float avoidWeight = 1.4f;
    [Tooltip("How strongly to slide along walls when facing them")]
    public float slideWeight = 0.6f;

    [Header("Carry")]
    public int carried = 0;
    public int capacity = 20;

    [Header("Unstuck")]
    public float stuckTime = 0.6f;
    public float nudgeStrength = 0.8f;

    Rigidbody2D rb;
    Collider2D  col;

    Vector2 lastPos;
    float stuckTimer;

    void Awake()
    {
        rb  = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // physics so it can always move
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // X/Y free
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        // Ensure movers actually collide (not trigger) so slide works
        col.isTrigger = false;

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
            if (node == null) { rb.linearVelocity = Vector2.zero; yield return new WaitForSeconds(0.5f); continue; }

            // 2) go to node
            yield return SteerTo(node.transform.position);

            // 3) harvest once (limited by capacity)
            if (carried < capacity && node != null)
            {
                yield return new WaitForSeconds(node.harvestTime);
                if (node.TryHarvest(out int gained))
                    carried = Mathf.Min(capacity, carried + gained);
            }

            // 4) go back to HQ and deposit
            if (depositTarget == null && myBank != null) depositTarget = myBank.transform;
            if (depositTarget != null)
            {
                yield return SteerTo(depositTarget.position);
                if (myBank != null && carried > 0) { myBank.AddFood(carried); carried = 0; }
            }

            yield return null;
        }
    }

    IEnumerator SteerTo(Vector3 target)
    {
        target.z = 0;
        while ((target - transform.position).sqrMagnitude > (stopDist * stopDist))
        {
            // desired direction
            Vector2 toTgt = ((Vector2)target - rb.position);
            Vector2 fwd   = rb.linearVelocity.sqrMagnitude > 0.001f ? rb.linearVelocity.normalized : toTgt.normalized;
            Vector2 left  = new Vector2(-fwd.y, fwd.x);
            Vector2 right = -left;

            // feeler rays
            float d = lookAhead;
            RaycastHit2D hitF = Physics2D.Raycast(rb.position, fwd, d, obstacleMask);
            RaycastHit2D hitL = Physics2D.Raycast(rb.position, (fwd + 0.6f*left).normalized, d*0.8f, obstacleMask);
            RaycastHit2D hitR = Physics2D.Raycast(rb.position, (fwd + 0.6f*right).normalized, d*0.8f, obstacleMask);

            // base desired velocity toward target
            Vector2 desired = toTgt.sqrMagnitude > 0.0001f ? toTgt.normalized * moveSpeed : Vector2.zero;

            // avoidance pushes away from obstacle surface normals
            Vector2 avoid = Vector2.zero;
            if (hitF) avoid += (Vector2)hitF.normal * avoidWeight;
            if (hitL) avoid += (Vector2)hitL.normal * (avoidWeight * 0.7f);
            if (hitR) avoid += (Vector2)hitR.normal * (avoidWeight * 0.7f);

            // slide along the wall if we're facing one
            Vector2 slide = Vector2.zero;
            if (hitF)
            {
                Vector2 tangent = new Vector2(-hitF.normal.y, hitF.normal.x); // perpendicular
                slide = tangent * slideWeight * moveSpeed;
            }

            // combine
            Vector2 vel = desired + avoid + slide;

            // clamp speed
            if (vel.magnitude > moveSpeed) vel = vel.normalized * moveSpeed;
            rb.linearVelocity = vel;

            // rotate to face velocity (like player)
            if (vel.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg - 90f;
                rb.rotation = angle;
            }

            // unstuck check
            float moved = (rb.position - lastPos).sqrMagnitude;
            lastPos = rb.position;
            if (moved < 0.0001f)
            {
                stuckTimer += Time.fixedDeltaTime;
                if (stuckTimer > stuckTime)
                {
                    Vector2 perp = new Vector2(-fwd.y, fwd.x).normalized;
                    rb.linearVelocity += perp * nudgeStrength;
                    stuckTimer = 0f;
                }
            }
            else stuckTimer = 0f;

            yield return new WaitForFixedUpdate();
        }

        // arrived
        rb.linearVelocity = Vector2.zero;
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

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        Vector2 pos = rb ? rb.position : (Vector2)transform.position;
        Vector2 fwd = rb && rb.linearVelocity.sqrMagnitude > 0.001f ? rb.linearVelocity.normalized : Vector2.up;

        float d = lookAhead;
        Vector2 left = new Vector2(-fwd.y, fwd.x);
        Vector2 right = -left;

        Gizmos.color = Color.yellow; Gizmos.DrawLine(pos, pos + fwd * d);
        Gizmos.color = Color.cyan;   Gizmos.DrawLine(pos, pos + (fwd + 0.6f*left).normalized * d*0.8f);
        Gizmos.color = Color.cyan;   Gizmos.DrawLine(pos, pos + (fwd + 0.6f*right).normalized * d*0.8f);
    }
#endif
}
