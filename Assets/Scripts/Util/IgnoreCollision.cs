using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField]
    private List<Collider2D> colliders;

    private void Start()
    {
        var playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        
        foreach (Collider2D col in colliders)
        {
             Physics2D.IgnoreCollision(col, playerCollider);
        }
    }
}
