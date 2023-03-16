using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    [SerializeField] private Coin _template;

    public event Action<Coin> OnSpawn;

    private void Start()
    {
        Transform[] _spawnPoints = GetComponentsInChildren<Transform>();
        var spawnInJob = StartCoroutine(Spawn(_spawnPoints));
    }

    private IEnumerator Spawn(Transform[] points)
    {
        var waitForMoment = new WaitForSeconds(1f);
        Coin coinComponent;

        for (int i = 1; i < points.Length; i++)
        {
            var coin = Instantiate(_template, points[i].transform.position, Quaternion.identity);
            coin.transform.parent = points[i].transform;

            if (coin.TryGetComponent<Coin>(out coinComponent))
            {
                OnSpawn?.Invoke(coinComponent);
            }

            yield return waitForMoment;
        }
    }
}
