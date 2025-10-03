using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDone : MonoBehaviour
{
    public void Done()
    {
        GameObject.FindGameObjectWithTag("WinScreen").SetActive(true);

        GameObject.FindGameObjectWithTag("Player").SetActive(false);
    }
}
