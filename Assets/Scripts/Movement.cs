using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AnimatorMario
{
    public static class Params
    {
        public const string Run = nameof(Run);
        public const string Jump = nameof(Jump);
    }
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private UnityEvent _dead;
    [SerializeField] private AudioClip _clip;

    private Rigidbody2D _rigidbody2D;
    private RaycastHit2D _bottomRay;
    private RaycastHit2D _leftRay;
    private RaycastHit2D _rightRay;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private AudioSource _audioSource;
    private float _downRayDistance;
    private float _sidesRayDistance;
    private bool _isAlive;

    private void Start()
    {
        TryGetComponent<Rigidbody2D>(out _rigidbody2D);
        TryGetComponent<SpriteRenderer>(out _spriteRenderer);
        TryGetComponent<Animator>(out _animator);
        TryGetComponent<AudioSource>(out _audioSource);
        _downRayDistance = _rigidbody2D.transform.localScale.y / 2 + 0.1f;
        _sidesRayDistance = _rigidbody2D.transform.localScale.x / 2 + 0.1f;
        _isAlive = true;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Flip(true);
            _animator.SetTrigger(AnimatorMario.Params.Run);
            transform.Translate(_speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Flip(false);
            _animator.SetTrigger(AnimatorMario.Params.Run);
            transform.Translate(_speed * Time.deltaTime * -1, 0, 0);
        }

        if (Input.GetKey(KeyCode.W) == true && StayOnGround() == true)
        {
            _animator.SetTrigger(AnimatorMario.Params.Jump);
            _rigidbody2D.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }

        if (MeetEnemy() && _isAlive)
        {
            _audioSource.PlayOneShot(_clip);
            Debug.Log("Игра окончена. Вы проиграли.");
            _isAlive = false;
            _dead.Invoke();
        }
    }

    private void Flip(bool isForward)
    {
        if (isForward == true)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;
    }

    private bool MeetEnemy()
    {
        _leftRay = Physics2D.Raycast(_rigidbody2D.position, Vector2.left, _sidesRayDistance, LayerMask.GetMask("Enemies"));
        _rightRay = Physics2D.Raycast(_rigidbody2D.position, Vector2.right, _sidesRayDistance, LayerMask.GetMask("Enemies"));
        return _leftRay.collider != null || _rightRay.collider != null;
    }

    private bool StayOnGround()
    {
        _bottomRay = Physics2D.Raycast(_rigidbody2D.position, Vector2.down, _downRayDistance, LayerMask.GetMask("Ground"));
        return _bottomRay.collider != null;
    }
}
