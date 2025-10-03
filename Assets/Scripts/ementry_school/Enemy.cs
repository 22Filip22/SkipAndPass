using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    int xpValue = 1;


    [SerializeField]
    int hp = 4;


    [SerializeField]
    UnityEvent onDeath;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {

            hp -= collision.gameObject.GetComponent<StraightProjectile>().Damage;

            if (hp <= 0)
            {
                XPCounter.Instance.AddXP(xpValue);
                EnemySolverCounter.Instance.counter++;
                Destroy(gameObject);
            }
        }
    }
}
