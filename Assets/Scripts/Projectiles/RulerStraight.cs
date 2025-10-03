using UnityEngine;

public class RulerStraight : MonoBehaviour
{

    [SerializeField]
    float speed = 1;

    void Update()
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime);
    }
}
