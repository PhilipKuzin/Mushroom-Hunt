using System.Collections;
using UnityEngine;

public class PlayerController : SingletonGeneric<PlayerController>
{
    private Animator _animator;
    private Rigidbody _rb;
    private Vector3 _startGamePosition;
    private Quaternion _startGameRotation;
    private Vector3 _targetPos;
    private float _laneOffset;
    private float _laneChangeSpeed = 15;

    private bool _isJumping = false;
    private bool _allowMovement = false;

    private bool _turnLeft = false;
    private bool _turnRight = false;
    private bool _turnJump = false;

    private const string RUN = "Run";
    private const string IDLE = "Idle";

    private void OnEnable()
    {
        SwipeManager.Instance.MoveEvent += SwipeHandler;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        _laneOffset = MapGenerator.Instance.LaneOffset;
        _startGamePosition = transform.position;
        _startGameRotation = transform.rotation;
        _targetPos = transform.position;
    }

    private void Update()
    {
        if (_allowMovement == false) { return; }

        if (!_isJumping)
        {
            if ((Input.GetKeyDown(KeyCode.A) || _turnLeft == true) && _targetPos.x > -_laneOffset)
            {
                _turnLeft = false;
                _targetPos = new Vector3(_targetPos.x - _laneOffset, transform.position.y, transform.position.z);
            }
            if ((Input.GetKeyDown(KeyCode.D) || _turnRight == true) && _targetPos.x < _laneOffset)
            {
                _turnRight = false;
                _targetPos = new Vector3(_targetPos.x + _laneOffset, transform.position.y, transform.position.z);
            }
        }

        if ((Input.GetKeyDown(KeyCode.W) || _turnJump == true) && _isJumping == false)
        {
            _turnJump = false;
            _isJumping = true;
            _animator.SetTrigger("Jump");
            float jumpHeight = 1.8f;
            StartCoroutine(JumpSequence(jumpHeight));
        }
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, _laneChangeSpeed * Time.deltaTime);
    }

    //private void OnDisable()
    //{
    //    SwipeManager.Instance.MoveEvent -= SwipeHandler;
    //}

    IEnumerator JumpSequence(float jumpHeight)
    {
        Vector3 startPos = transform.position;
        Vector3 peakPos = startPos + Vector3.up * jumpHeight;

        float jumpDuration = 0.3f;

        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration && _allowMovement)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / jumpDuration;
            transform.position = Vector3.Lerp(startPos, peakPos, t);

            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < jumpDuration && _allowMovement)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / jumpDuration;
            transform.position = Vector3.Lerp(peakPos, startPos, t);

            yield return null;
        }

        _isJumping = false;
    }

    public void SwipeHandler(bool[] swipes)
    {
        if (swipes[0] == true) { _turnLeft = true; }
        else if (swipes[1] == true) { _turnRight = true; }
        else if (swipes[2] == true) { _turnJump = true; }
    }

    public void StartGame()
    {
        _allowMovement = true;
        transform.position = _startGamePosition;
        _animator.SetTrigger(RUN);
        StartLevel();
    }

    public void StartLevel()
    {
        RoadGenerator.Instance.StartLevel();
    }

    public void ResetGame()
    {
        _animator.Play(IDLE, 0, 0f);
        _isJumping = false;
        _allowMovement = false;
        transform.position = _startGamePosition;
        transform.rotation = _startGameRotation;
        _targetPos = transform.position;
        //_animator.SetTrigger(IDLE);       
        RoadGenerator.Instance.ResetLevel();
    }

}