using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Bamboo.Utility;

public class BaseFightSceneManager : MonoBehaviour
{
    [SerializeField] protected  string StartingWaveName;
    [SerializeField] protected List<FightSceneWave> fightWaves;
    protected Dictionary<string, FightSceneWave> waveNameToWave = new Dictionary<string, FightSceneWave>();

    protected int currentWaveEnemies = 0;
    protected FightSceneWave currentWave;

    public void Start()
    {
        currentWave = null;
        foreach (var a in fightWaves)
        {
            waveNameToWave.Add(a.WaveName, a);
        }
    }

    public void RunWave(string wavename)
    {
        currentWave = waveNameToWave[wavename];
        currentWaveEnemies = currentWave.waveEnemies.Count;
        foreach (var a in currentWave.waveEnemies)
        {
            Instantiate(a.Prefab).GetComponent<EnemyController>().OnEnemyDeath = OnEnemyDeath;
        }
    }

    public void SwitchLevel(string lvl)
    {
        LoadHandler.Instance.ChangeScene(lvl);
    }

    public virtual void OnEnemyDeath()
    {
        currentWaveEnemies--;
        if (currentWaveEnemies == 0)
        {
            currentWave.onWaveClear?.Invoke();
        }
    }
}
