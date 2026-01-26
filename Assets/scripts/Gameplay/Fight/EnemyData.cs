using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Fight/EnemyData")]
public class EnemyData : ScriptableObject
{
    public Direction[] pattern;

    public int maxHealth = 1;

    [Header("Difficulty/Feel")]
    public float projectileTravelTime = 0.6f;
    public float chainGap = 0.45f;

    [Header("Randomness")]
    public float startJitter = 0.15f;
    public float controlJitter = 0.25f;

    [Header("Visual")]
    public Projectile projectilePrefab; // optional


}
