using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StateModPair
{
    [SerializeField] public EnemyStates state;
    [SerializeField] public int mod;
}

[CreateAssetMenu(fileName = "Enemy Properties", menuName = "Enemy Properties")]
public class EnemyProperties : ScriptableObject
{
    public List<StateModPair> engageMods = new List<StateModPair>();
    public List<StateModPair> backlineMods = new List<StateModPair>();
    public List<string> Attacks = new List<string>();

    public List<string> DeathSounds = new List<string>();
    public List<string> AttackSounds = new List<string>();

    public float EngageRadius = 1.8f;
    public float RetreatHeight = 1.7f;
    public float RetreatRadius = 4.0f;
    public float StrafeSpeed = 20.0f;

    public float RetreatDuration = 1.6f;

    public int startingHealth = 3;

    public float EnemyHeight = 1.7f;

    public bool spawnFromSky = true;
}
