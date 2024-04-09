using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rb;
    private Vector3 _startGamePosition;
    private Quaternion _startGameRotation;
    private Vector3 _targetPos;
    private float _laneOffset = 2.2f;
    private float _laneChangeSpeed = 15;

    private bool _isJumping = false;
    private bool _allowMovement = false;

    private const string RUN = "Run";
    private const string IDLE = "Idle";
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _startGamePosition = transform.position;
        _startGameRotation = transform.rotation;
        _targetPos = transform.position;
    }

    private void Update()
    {
        if (_allowMovement == false) { return; }

        // Проверяем, не находится ли персонаж в процессе прыжка
        if (!_isJumping)
        {
            if (Input.GetKeyDown(KeyCode.A) && _targetPos.x > -_laneOffset)
            {
                _targetPos = new Vector3(_targetPos.x - _laneOffset, transform.position.y, transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.D) && _targetPos.x < _laneOffset)
            {
                _targetPos = new Vector3(_targetPos.x + _laneOffset, transform.position.y, transform.position.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && _isJumping == false)
        {
            _isJumping = true;
            _animator.SetTrigger("Jump");
            float jumpHeight = 1.8f;
            StartCoroutine(JumpSequence(jumpHeight));
        }
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, _laneChangeSpeed * Time.deltaTime);
    }


    IEnumerator JumpSequence(float jumpHeight)
    {
        Vector3 startPos = transform.position;
        Vector3 peakPos = startPos + Vector3.up * jumpHeight;

        float jumpDuration = 0.3f; 

        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / jumpDuration;
            transform.position = Vector3.Lerp(startPos, peakPos, t);

            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / jumpDuration;
            transform.position = Vector3.Lerp(peakPos, startPos, t);

            yield return null;
        }

        _isJumping = false;
    }


    public void StartGame()
    {
        _allowMovement = true;
        _animator.SetTrigger(RUN);
        StartLevel();
    }

    public void StartLevel()
    {
        RoadGenerator.Instance.StartLevel();
    }

    public void ResetGame()
    {
        _allowMovement = false;
        transform.position = _startGamePosition;
        transform.rotation = _startGameRotation;
        _targetPos = transform.position;
        _animator.SetTrigger(IDLE);
        RoadGenerator.Instance.ResetLevel();
    }

}