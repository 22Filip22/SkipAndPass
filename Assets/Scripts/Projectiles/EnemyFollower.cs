using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private string playerTag = "Player";

    private Transform target;

    void Start()
    {
        var playerGO = GameObject.FindGameObjectWithTag(playerTag);
        if (playerGO != null)
        {
            target = playerGO.transform;
        }
    }

    void Update()
    {
        if (target == null || speed <= 0f) return;

        Vector3 current = transform.position;
        Vector3 destination = target.position;


        transform.position = Vector3.MoveTowards(current, destination, speed * Time.deltaTime);
    }
}
