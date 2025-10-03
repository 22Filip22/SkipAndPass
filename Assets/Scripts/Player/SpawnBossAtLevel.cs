using UnityEngine;

public class SpawnBossAtLevel : MonoBehaviour
{
    [SerializeField]
    int levelToSpawnBoss;

    [SerializeField]
    GameObject enemySpawner;

    [SerializeField]
    GameObject enemyCollector;


    [SerializeField]
    Transform spawnPointPlayer;

    [SerializeField]
    Transform spawnPointBoss;

    [SerializeField]
    GameObject boss;



    void Update()
    {
       if(XPCounter.Instance.CurrentLevel == 15)
        {
            enemySpawner.SetActive(false);
            enemyCollector.SetActive(false);

            Destroy(enemySpawner);
            Destroy(enemyCollector);




            GameObject.FindGameObjectWithTag("Player").transform.position = spawnPointPlayer.transform.position;

            Instantiate(boss, spawnPointBoss);
        }
    }
}
