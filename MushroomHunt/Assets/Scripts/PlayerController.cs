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
        if (Input.GetKeyDown(KeyCode.A) && _targetPos.x > -_laneOffset)
        {
            _targetPos = new Vector3 (_targetPos.x - _laneOffset, transform.position.y, transform.position.z);
        }
        if(Input.GetKeyDown(KeyCode.D) && _targetPos.x < _laneOffset) 
        {
            _targetPos = new Vector3(_targetPos.x + _laneOffset, transform.position.y, transform.position.z);
        }
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, _laneChangeSpeed * Time.deltaTime);
        
    }

    public void StartGame()
    {
        _animator.SetTrigger(RUN);
    }

    public void StartLevel()
    {
        RoadGenerator.Instance.StartLevel();
    }

    public void ResetGame()
    {
        transform.position = _startGamePosition;
        transform.rotation = _startGameRotation;
        _targetPos = transform.position;
        _animator.SetTrigger(IDLE);
        RoadGenerator.Instance.ResetLevel();
    }

}
