using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] Reference<Transform> player;
    [SerializeField] Reference<int> EnemyMax;
    int currentEnemyCount;
    [SerializeField] GameObject enemyPrefab;

    private void Update()
    {
        if (!IsServer) return;
        if (EnemyMax > currentEnemyCount)
        {
            var randPos = new Vector3(10, 0, 10);
            var enemy = Instantiate(enemyPrefab, randPos, Quaternion.identity);
            enemy.GetComponent<Death>().OnDeath.AddListener(Countdown);
            Spawn(enemy);
            currentEnemyCount++;
        }
    }

    private void Countdown()
    {
        currentEnemyCount--;
    }
}
