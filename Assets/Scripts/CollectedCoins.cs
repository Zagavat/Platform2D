using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollectedCoins : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private CoinSpawn _coinSpawn;

    private int _score;
    private List<Coin> _coins;
    private AudioSource _audioSource;

    private void Start()
    {
        _coins = new List<Coin>();

        if (_coinSpawn != null)
            _coinSpawn.OnSpawn += AddSpawnedCoin;

        TryGetComponent<AudioSource>(out _audioSource);
        _coinSpawn.enabled = true;
    }

    private void OnDisable()
    {
        _coinSpawn.OnSpawn -= AddSpawnedCoin;

        foreach(Coin coin in _coins)
        {
            coin.Collected -= AddCollectedCoin;
        }
    }

    private void AddSpawnedCoin(Coin coin)
    {
        _coins.Add(coin);
        coin.Collected += AddCollectedCoin;
    }

    private void AddCollectedCoin()
    {
        _audioSource.PlayOneShot(_clip);
        _score++;
        Debug.Log("Монеток собрано - " + _score);
    }
}
