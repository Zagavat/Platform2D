using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Death : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private bool _isBlinkInJob = true;

    public void Dead()
    {
        TryGetComponent<SpriteRenderer>(out _spriteRenderer);
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        float iterationStep = 0.08f;
        float direction = 0;
        int maxBlinks = 8;
        float alpha = _spriteRenderer.color.a;
        Color color = _spriteRenderer.color;
        _isBlinkInJob = true;

        for(int i = 0; i < maxBlinks; i++)
        {
            while (alpha != direction)
            {
                alpha = Mathf.MoveTowards(_spriteRenderer.color.a, direction, iterationStep);
                color.a = alpha;
                _spriteRenderer.color = color;
                yield return null;
            }

            direction = (i + 3) % 2;
        }

        _isBlinkInJob = false;

        if (_isBlinkInJob != true)
            Destroy(this.gameObject);
    }
}
