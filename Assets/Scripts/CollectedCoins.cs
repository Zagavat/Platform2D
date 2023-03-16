using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollectedCoins : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    private int _score;
    private List<Coin> _coins;
    private CoinSpawn _coinSpawn;
    private AudioSource _audioSource;

    private void Start()
    {
        _coinSpawn = FindObjectOfType<CoinSpawn>();
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
        Debug.Log("Монетка родилась");
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
