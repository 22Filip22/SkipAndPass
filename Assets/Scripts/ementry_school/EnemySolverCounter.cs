using UnityEngine;

public class EnemySolverCounter : MonoBehaviour
{
    public static EnemySolverCounter Instance { get; private set; }

    public int counter;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

}
