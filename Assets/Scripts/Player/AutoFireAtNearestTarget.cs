using System.Collections.Generic;
using UnityEngine;

public class AutoFireAtNearestTarget : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;

    [Header("References")]
    [SerializeField] private Weapons weapons;
    [SerializeField] private GameObject bulletCollector;

    [Header("Stats")]
    [SerializeField] private int damage = 10;
    [SerializeField] private int globalFireRateBonus = 0;

    [Header("Target Settings")]
    public string targetTag = "Enemy";

    [Header("Performance")]
    [SerializeField] private float fireCheckInterval = 0.05f; // check every 0.05s
    private float fireCheckTimer = 0f;

    private List<GameObject> cachedTargets = new List<GameObject>();

    void Update()
    {
        fireCheckTimer -= Time.deltaTime;
        if (fireCheckTimer <= 0f)
        {
            fireCheckTimer = fireCheckInterval;
            UpdateTargets();
            FireWeapons();
        }
    }

    void UpdateTargets()
    {
        cachedTargets.Clear();
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        if (targets.Length > 0)
            cachedTargets.AddRange(targets);
    }

    void FireWeapons()
    {
        if (cachedTargets.Count == 0) return;

        Vector3 playerPos = transform.position;

        foreach (var weapon in weapons.WeaponsList)
        {
            if (weapon.weapon == null) continue;

            weapon.cooldown -= fireCheckInterval;

            if (weapon.cooldown <= 0f)
            {
                // Find the nearest target to this weapon
                GameObject nearestTarget = null;
                float minSqrDistance = weapon.detectionRange * weapon.detectionRange;

                for (int i = 0; i < cachedTargets.Count; i++)
                {
                    Vector3 diff = cachedTargets[i].transform.position - playerPos;
                    float sqrDistance = diff.sqrMagnitude;

                    if (sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        nearestTarget = cachedTargets[i];
                    }
                }

                if (nearestTarget != null)
                {
                    FireAtTarget(nearestTarget, weapon);

                    // Apply fire rate scaling and global bonus
                    float effectiveRate = Mathf.Max(1f, (weapon.fireRate + globalFireRateBonus) * weapon.fireRateScale);
                    weapon.cooldown = 1f / effectiveRate;
                }
            }
        }
    }

    private void FireAtTarget(GameObject target, WeaponSlot weapon)
    {
        if (projectilePrefab == null) return;

        GameObject projectile = Instantiate(
            projectilePrefab,
            transform.position,
            transform.rotation,
            bulletCollector.transform
        );

        StraightProjectile straight = projectile.GetComponent<StraightProjectile>();
        if (straight != null)
        {
            straight.SetDamage(damage);
            straight.SetTarget(target);
        }
    }

    public void SetFireRateBonus(int fireRate) => globalFireRateBonus = fireRate;
    public void SetDamage(int damage)
    {
        damage += damage;
    }
}