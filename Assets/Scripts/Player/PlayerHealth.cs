using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int health = 10;

    [SerializeField] public int maxHealth = 10;

    [SerializeField] public int shield = 5;

    [SerializeField]
    TextMeshProUGUI healthText;

    [SerializeField]
    TextMeshProUGUI shieldText;

    [SerializeField]
    TextMeshProUGUI totalHitsText;



    [SerializeField]
    GameObject lostSreen;

    [SerializeField]
    TextMeshProUGUI levelReached;

    [SerializeField]
    TextMeshProUGUI enemiesSolved;



    [SerializeField]
    GameObject[] otherCanvasas;

    private void Start()
    {
        UpdateGUI();
    }


    private void UpdateGUI()
    {
        healthText.text = $"+ {health} Health";
        shieldText.text = $"   {shield} Shield";
        totalHitsText.text = $"   {health + shield} Hits";
    }

    public void Heal(int count)
    {
        if ((health + count) > maxHealth)
        {
            health = maxHealth;
            UpdateGUI();
            return;
        }

        health += count;


        UpdateGUI();
    }

    public void Shield(int count)
    {
        shield += count;

        UpdateGUI();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            int damage = 1; // You can adjust this if enemies have different damage

            if (shield > 0)
            {
                int remainingDamage = damage - shield;
                shield -= damage;

                // If shield went below zero, apply leftover damage to health
                if (shield < 0)
                {
                    health += shield; // shield is negative, so this subtracts the leftover from health
                    shield = 0;
                }
            }
            else
            {
                health -= damage;
            }

            if (health <= 0)
            {
                foreach (var canvas in otherCanvasas)
                {
                    canvas.SetActive(false);
                }

                levelReached.text = $"{XPCounter.Instance.CurrentLevel}";

                enemiesSolved.text = $"{EnemySolverCounter.Instance.counter}";

                GameObject.FindGameObjectWithTag("Player").SetActive(false);

                lostSreen.SetActive(true);
            }

            UpdateGUI();
        }
    }
}
