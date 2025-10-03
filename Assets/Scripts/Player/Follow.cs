using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    Transform TargetToFollow;

    void Update()
    {
        transform.position = TargetToFollow.position;
    }
}
