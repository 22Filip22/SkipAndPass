using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponSlot
{
    public GameObject weapon;          // the prefab in the scene
    public int fireRate = 1;           // shots per second
    public float cooldown = 0f;        // runtime use
    public float detectionRange = 10f; // specific to this weapon
    public float fireRateScale = 1f;   // scaling for fire rate
}

public class Weapons : MonoBehaviour
{
    [Header("Weapon Slots")]
    [SerializeField] private List<WeaponSlot> weapons = new List<WeaponSlot>();

    public List<WeaponSlot> WeaponsList => weapons;
}