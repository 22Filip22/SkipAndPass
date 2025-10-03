using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class EnemyGroup
{
    public string name;
    public GameObject[] prefabs;
    public Vector2Int countRange = new Vector2Int(1, 3);
    [Range(0f, 1f)] public float probability = 1f;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public EnemyGroup[] groups;

    [Header("Spawn Area")]
    public Vector2 squareSize = new Vector2(1f, 1f); // Inner square size
    public float padding = 0.5f; // Outer padding

    [Header("Spawn Timing")]
    public float spawnInterval = 2f; // Time between spawns in seconds

    [SerializeField]
    GameObject enemyCollector;


    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemies();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemies()
    {
        Vector3 center = transform.position;

        float innerHalfWidth = squareSize.x / 2f;
        float innerHalfHeight = squareSize.y / 2f;

        float outerHalfWidth = innerHalfWidth + padding;
        float outerHalfHeight = innerHalfHeight + padding;

        foreach (var group in groups)
        {
            if (UnityEngine.Random.value <= group.probability)
            {
                int count = UnityEngine.Random.Range(group.countRange.x, group.countRange.y + 1);

                for (int i = 0; i < count; i++)
                {
                    if (group.prefabs.Length == 0) continue;

                    GameObject prefab = group.prefabs[UnityEngine.Random.Range(0, group.prefabs.Length)];

                    Vector3 spawnPos;

                    // Randomly decide which "side" of the inner square to spawn in
                    bool spawnOnXEdge = UnityEngine.Random.value < 0.5f;

                    if (spawnOnXEdge)
                    {
                        // Spawn on left or right "strip"
                        float x = UnityEngine.Random.value < 0.5f
                            ? UnityEngine.Random.Range(center.x - outerHalfWidth, center.x - innerHalfWidth)
                            : UnityEngine.Random.Range(center.x + innerHalfWidth, center.x + outerHalfWidth);
                        float y = UnityEngine.Random.Range(center.y - outerHalfHeight, center.y + outerHalfHeight);
                        spawnPos = new Vector3(x, y, center.z);
                    }
                    else
                    {
                        // Spawn on top or bottom "strip"
                        float y = UnityEngine.Random.value < 0.5f
                            ? UnityEngine.Random.Range(center.y - outerHalfHeight, center.y - innerHalfHeight)
                            : UnityEngine.Random.Range(center.y + innerHalfHeight, center.y + outerHalfHeight);
                        float x = UnityEngine.Random.Range(center.x - outerHalfWidth, center.x + outerHalfWidth);
                        spawnPos = new Vector3(x, y, center.z);
                    }

                    Instantiate(prefab, spawnPos, Quaternion.identity, enemyCollector.transform);
                }
            }
        }
    }



    private void OnDrawGizmos()
    {
        Vector3 center = transform.position;

        // Inner square
        Gizmos.color = Color.green;
        float halfWidthInner = squareSize.x / 2f;
        float halfHeightInner = squareSize.y / 2f;

        Vector3 tl = center + new Vector3(-halfWidthInner, halfHeightInner, 0);
        Vector3 tr = center + new Vector3(halfWidthInner, halfHeightInner, 0);
        Vector3 br = center + new Vector3(halfWidthInner, -halfHeightInner, 0);
        Vector3 bl = center + new Vector3(-halfWidthInner, -halfHeightInner, 0);

        Gizmos.DrawLine(tl, tr);
        Gizmos.DrawLine(tr, br);
        Gizmos.DrawLine(br, bl);
        Gizmos.DrawLine(bl, tl);

        // Outer square
        Gizmos.color = Color.red;
        float halfWidthOuter = halfWidthInner + padding;
        float halfHeightOuter = halfHeightInner + padding;

        Vector3 otl = center + new Vector3(-halfWidthOuter, halfHeightOuter, 0);
        Vector3 otr = center + new Vector3(halfWidthOuter, halfHeightOuter, 0);
        Vector3 obr = center + new Vector3(halfWidthOuter, -halfHeightOuter, 0);
        Vector3 obl = center + new Vector3(-halfWidthOuter, -halfHeightOuter, 0);

        Gizmos.DrawLine(otl, otr);
        Gizmos.DrawLine(otr, obr);
        Gizmos.DrawLine(obr, obl);
        Gizmos.DrawLine(obl, otl);
    }
}
