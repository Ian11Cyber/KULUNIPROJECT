using UnityEngine;
using System.Collections;

public class DamageFeedback : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color flashColor = Color.white;
    public float flashTime = 0.08f;

    void Awake() {
        if (!sr) sr = GetComponentInChildren<SpriteRenderer>();
        var hp = GetComponent<Health>();
        if (hp) hp.onDamaged.AddListener(_ => StartCoroutine(Flash()));
    }

    IEnumerator Flash() {
        if (!sr) yield break;
        var orig = sr.color;
        sr.color = flashColor;
        yield return new WaitForSeconds(flashTime);
        sr.color = orig;
    }
}
