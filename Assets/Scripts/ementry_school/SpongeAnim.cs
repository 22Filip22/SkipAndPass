using UnityEngine;

public class SpongeAnim : MonoBehaviour
{
    [Header("Scale Settings")]
    public float minY = 0.5f;       // Minimum Y scale
    public float maxY = 1.5f;       // Maximum Y scale
    public float cycleTime = 1f;    // Time (in seconds) for one full up-and-down cycle

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Calculate a value from 0 to 1 that oscillates over time
        float t = Mathf.PingPong(Time.time / cycleTime, 1f);

        // Lerp Y scale between minY and maxY
        float newY = Mathf.Lerp(minY, maxY, t);

        // Apply new scale while keeping X and Z the same
        transform.localScale = new Vector3(initialScale.x, newY, initialScale.z);
    }
}
