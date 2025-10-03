using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;
using System.Collections;

public class ExplosiveBullet : MonoBehaviour
{
    [SerializeField] VisualEffect effect;

    bool hasTriggered;

    public void Explode()
    {
        if (hasTriggered)
            return;

        effect.Play();

        hasTriggered = true;

        gameObject.transform.localScale *= 5f;


        StartCoroutine(DisableAfterTime());
    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
