using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FightSceneWave
{
    [SerializeField] public string WaveName;
    [SerializeField] public List<SceneEnemy> waveEnemies = new List<SceneEnemy>();
    [SerializeField] public UnityEvent onWaveClear = new UnityEvent();
}