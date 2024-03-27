using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private Vector3 _startGamePosition;
    private Quaternion _startGameRotation;

    private const string RUN = "Run";
    private const string IDLE = "Idle";
    void Start()
    {
        _animator = GetComponent<Animator>();
        _startGamePosition = transform.position;
        _startGameRotation = transform.rotation;
    }

    public void StartGame ()
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
        _animator.SetTrigger(IDLE);
        RoadGenerator.Instance.ResetLevel();
    }

}
