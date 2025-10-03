using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int Damage { get;  set; }
    public int MoveSpeed { get;  set; }
    public int AttackSpeed{ get;  set; }
    public int Hp{ get;  set; }


    [SerializeField]
    TextMeshProUGUI hpStat;

    [SerializeField]
    TextMeshProUGUI attackSpeedStat;

    [SerializeField]
    TextMeshProUGUI damageStat;

    [SerializeField]
    TextMeshProUGUI moveSpeedStat;


    private AutoFireAtNearestTarget fireScript;
    private TopDownCharacterController playerScript;
    private AutoFireAtNearestTarget autoFireScript;
    private PlayerHealth healthScript;




    private void Start()
    {
        AttackSpeed = 1;
        MoveSpeed = 6;
        Damage = 2;
        Hp = 5;

        fireScript = GetComponent<AutoFireAtNearestTarget>();
        playerScript = GetComponent<TopDownCharacterController>();
        autoFireScript = GetComponent<AutoFireAtNearestTarget>();
        healthScript = GetComponent<PlayerHealth>();


        UpdateSetStats();
    }





    public void UpdateSetStats()
    {
        fireScript.SetFireRateBonus(AttackSpeed);
        
        autoFireScript.SetDamage(Damage);

        playerScript.moveSpeed = MoveSpeed;

        healthScript.maxHealth = Hp;



        hpStat.text = $"{Hp}";
        attackSpeedStat.text = $"{AttackSpeed}";
        damageStat.text = $"{Damage}";
        moveSpeedStat.text = $"{MoveSpeed}";
    }
}
