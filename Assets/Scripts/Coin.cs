using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public event Action Collected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Mario>(out Mario mario))
        {
            Collected?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
