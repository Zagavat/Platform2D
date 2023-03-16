using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class MashroomMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private UnityEvent _marioOnHead;
    [SerializeField] private AudioClip _clip;

    private int _direction = 1;
    private float _sidesRayDistance;
    private float _bottomRayDistance;
    private RaycastHit2D _sidesObstacle;
    private RaycastHit2D _bottomObstacle;
    private RaycastHit2D _topObstacle;
    private AudioSource _audioSource;
    private bool _isAlive;

    void Start()
    {
        _sidesRayDistance = transform.localScale.x / 2 + 0.1f;
        _bottomRayDistance = transform.localScale.y / 2 + 0.1f;
        TryGetComponent<AudioSource>(out _audioSource);
        _isAlive = true;
    }

    private void Update()
    {
        transform.Translate(_speed * Time.deltaTime * _direction, 0, 0);
        _sidesObstacle = Physics2D.Raycast(transform.position, new Vector2(_direction, 0), _sidesRayDistance, LayerMask.GetMask("Ground"));
        _bottomObstacle = Physics2D.Raycast(transform.position, Vector2.down, _bottomRayDistance, LayerMask.GetMask("Ground"));
        _topObstacle = Physics2D.Raycast(transform.position, Vector2.up, _bottomRayDistance, LayerMask.GetMask("Mario"));

        if (_sidesObstacle.collider != null || _bottomObstacle.collider == null)
            _direction *= -1;

        if (_topObstacle.collider != null)
            if(_topObstacle.collider.TryGetComponent<Mario>(out Mario mario) && _isAlive)
            {
                _direction = 0;
                _isAlive = false;
                _audioSource.PlayOneShot(_clip);
                _marioOnHead.Invoke();
            }
    }
}
