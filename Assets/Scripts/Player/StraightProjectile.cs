using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StraightProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;

    [SerializeField] UnityEvent onHit;
    [SerializeField] int passCount = 1;
    [SerializeField] int liveTime = 15;

    [Header("Return Settings")]
    [SerializeField] private bool returnToSender = false;
    [SerializeField] private float maxDistance = 10f; // distance before returning
    private GameObject sender;
    private Vector3 startPosition;
    private bool isReturning = false;


    public int Damage { get; private set; }

    public void SetDamage(int damage)
    {
        Damage = damage;
    }

    public void SetTarget(GameObject target)
    {
        if (target == null) return;

        sender = GameObject.FindGameObjectWithTag("Player");
        startPosition = transform.position;

        // Calculate initial direction to the target
        direction = (target.transform.position - transform.position).normalized;
    }

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(liveTime);
        Destroy(gameObject);
    }

    void Update()
    {
        if (isReturning && sender != null)
        {
            // Update direction towards sender (no rotation applied)
            direction = (sender.transform.position - transform.position).normalized;
        }

        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (!isReturning && returnToSender)
        {
            float traveled = Vector3.Distance(startPosition, transform.position);
            if (traveled >= maxDistance)
            {
                ReturnToSender();
            }
        }
    }

    private void ReturnToSender()
    {
        if (sender == null) return;
        isReturning = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isReturning)
        {
            if (--passCount != 0)
                return;

            if (!returnToSender)
            {
                StartCoroutine(BulletHit());
            }
        }
        else if (sender != null && collision.gameObject == sender && isReturning)
        {
            // Projectile reached the sender, destroy it
            StartCoroutine(BulletHit());
        }
    }

    IEnumerator BulletHit()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        speed = 0;
        onHit?.Invoke();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
