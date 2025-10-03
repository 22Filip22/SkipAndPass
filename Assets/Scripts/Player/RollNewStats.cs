using UnityEngine;
using TMPro;
using System.Linq;

public enum StatKinds
{
    AttackSpeed,
    MoveSpeed,
    Damage,
    Hp,
    Heal,
    Shield
}

[System.Serializable]
public class StatOption
{
    public StatKinds stat;
    public string title;
    public int minCount = 1;
    public int maxCount = 100;
    [Range(0f, 1f)] public float probability = 0.5f; // weight
}

[System.Serializable]
public class WeaponOption
{
    public string weaponName;
    public GameObject weaponPrefab;
    [Range(0f, 1f)] public float probability = 0.5f;
    public int maxCount = 0; // 0 = unlimited
    [HideInInspector] public int currentCount = 0;

    public float cooldown;

    [Header("Weapon Stats")]
    [Range(0.1f, 5f)]
    public float fireRateScale = 1f;       // scaling for this weapon's fire rate
    public float detectionRange = 10f;     // optional: set detection range per weapon
    public int baseFireRate = 1;           // optional: base fire rate for this weapon
}


[System.Serializable]
public class Card
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    [HideInInspector] public bool isStat;
    [HideInInspector] public StatOption stat;
    [HideInInspector] public int rolledValue;
    [HideInInspector] public WeaponOption weapon;
}

public class RollNewStats : MonoBehaviour
{
    [Header("Stats Settings")]
    public StatOption[] stats;

    [Header("Weapons Settings")]
    public WeaponOption[] weapons;

    [Header("UI Cards")]
    [SerializeField] private Card[] cards;
    [SerializeField] private GameObject cardCanvas;

    Stats statsManager;
    PlayerHealth playerHealth;
    Weapons weaponsScript;

    void Start()
    {
        statsManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        weaponsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapons>();
    }

    public void ShowCardCanvas()
    {
        cardCanvas.SetActive(true);
        RollThreeCards();
    }

    void RollThreeCards()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
                RollCard(cards[i]);
        }
    }

    void RollCard(Card card)
    {
        // Filter out Heal if player is full
        var availableStats = stats.Where(s => s.stat != StatKinds.Heal || playerHealth.health < playerHealth.maxHealth).ToArray();
        float totalStatWeight = availableStats.Sum(s => s.probability);

        // Only include weapons that are under the limit
        var availableWeapons = weapons.Where(w => w.maxCount == 0 || w.currentCount < w.maxCount).ToArray();
        float totalWeaponWeight = availableWeapons.Sum(w => w.probability);

        float total = totalStatWeight + totalWeaponWeight;
        if (total <= 0f)
        {
            card.title.text = "No Options Left";
            card.description.text = "";
            return;
        }

        float roll = Random.Range(0f, total);

        if (roll <= totalStatWeight)
        {
            // Stat
            StatOption stat = RollRandomStat(availableStats);
            int rolledValue = Random.Range(stat.minCount, stat.maxCount + 1);

            card.title.text = stat.title;
            card.description.text = $"+{rolledValue}";

            card.isStat = true;
            card.stat = stat;
            card.rolledValue = rolledValue;
            card.weapon = null;
        }
        else
        {
            // Weapon
            WeaponOption weapon = RollRandomWeapon(availableWeapons);
            if (weapon == null) return;

            card.title.text = weapon.weaponName;
            card.description.text = "";

            card.isStat = false;
            card.stat = null;
            card.weapon = weapon;
        }
    }

    public StatOption RollRandomStat(StatOption[] availableStats)
    {
        float totalWeight = availableStats.Sum(s => s.probability);
        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var stat in availableStats)
        {
            cumulative += stat.probability;
            if (roll <= cumulative) return stat;
        }
        return availableStats.Length > 0 ? availableStats[0] : null;
    }

    public WeaponOption RollRandomWeapon(WeaponOption[] availableWeapons)
    {
        float totalWeight = availableWeapons.Sum(w => w.probability);
        if (totalWeight <= 0f) return null;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var weapon in availableWeapons)
        {
            cumulative += weapon.probability;
            if (roll <= cumulative) return weapon;
        }
        return null;
    }

    public void OnCardChosen(int cardIndex)
    {
        if (cardIndex < 0 || cardIndex >= cards.Length) return;
        Card chosen = cards[cardIndex];

        if (chosen.isStat)
        {
            ApplyStat(chosen.stat, chosen.rolledValue);
        }
        else if (chosen.weapon != null)
        {
            ApplyWeapon(chosen.weapon);
        }

        cardCanvas.SetActive(false);
    }

    private void ApplyStat(StatOption stat, int value)
    {

        switch (stat.stat)
        {
            case StatKinds.AttackSpeed:
                statsManager.AttackSpeed += value;
                break;
            case StatKinds.MoveSpeed:
                statsManager.MoveSpeed += value;
                break;
            case StatKinds.Damage:
                statsManager.Damage += value;
                break;
            case StatKinds.Hp:
                statsManager.Hp += value;
                break;
            case StatKinds.Shield:
                playerHealth.Shield(value);
                break;
            case StatKinds.Heal:
                playerHealth.Heal(value); // guaranteed to be usable because filtered earlier
                break;
        }

        statsManager.UpdateSetStats();
    }

    private void ApplyWeapon(WeaponOption weapon)
    {
        if (weapon.weaponPrefab == null) return;

        // Instantiate weapon object
        GameObject weaponObj = Instantiate(weapon.weaponPrefab, weaponsScript.transform);
        weaponObj.name = weapon.weaponName;

        // Add weapon to the player's weapons list with stats from WeaponOption
        weaponsScript.WeaponsList.Add(new WeaponSlot()
        {
            weapon = weaponObj,
            fireRate = weapon.baseFireRate,          // from WeaponOption
            cooldown = weapon.cooldown,
            detectionRange = weapon.detectionRange,  // from WeaponOption
            fireRateScale = weapon.fireRateScale     // from WeaponOption
        });

        weapon.currentCount++;
    }

}
