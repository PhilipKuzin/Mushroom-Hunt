using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : SingletonGeneric<SwipeManager>
{
    public delegate void MoveDelegate(bool[] swipes);
    public MoveDelegate MoveEvent;
    public delegate void ClickDelegate(Vector2 pos);
    public ClickDelegate ClickEvent;
    public enum Directions { Left, Right, Up, Down };
    private bool[] _swipes = new bool[4];

    private const float SWIPE_THRESHOLD = 50f;
    private Vector2 _startTouch;
    private Vector2 _swipeDelta;
    private bool _touchMoved;

    private Vector2 TouchPosition() { return (Vector2)Input.mousePosition; }
    private bool TouchBegan() { return Input.GetMouseButtonDown(0); }
    private bool TouchEnded() { return Input.GetMouseButtonUp(0); }
    private bool GetTouch() { return Input.GetMouseButton(0); }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        // start / finish
        if (TouchBegan())
        {
            _startTouch = TouchPosition();
            _touchMoved = true;
        }
        else if (TouchEnded() && _touchMoved == true)
        {
            SendSwipe();
            _touchMoved = false;
        }

        // calculate distance
        _swipeDelta = Vector2.zero;
        if (_touchMoved == true && GetTouch())
        {
            _swipeDelta = TouchPosition() - _startTouch;
        }

        //check swipe
        if (_swipeDelta.magnitude > SWIPE_THRESHOLD)
        {
            if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
            {
                _swipes[(int)Directions.Left] = _swipeDelta.x < 0;
                _swipes[(int)Directions.Right] = _swipeDelta.x > 0;
            }
            else
            {
                _swipes[(int)Directions.Down] = _swipeDelta.y < 0;
                _swipes[(int)Directions.Up] = _swipeDelta.y > 0;
            }
            SendSwipe();
        }
    }
    private void SendSwipe()
    {
        if (_swipes[0] || _swipes[1] || _swipes[2] || _swipes[3])
        {
            Debug.Log(_swipes[0] + "|" + _swipes[1] + "|" + _swipes[2] + "|" + _swipes[3]);
            MoveEvent?.Invoke(_swipes);
        }
        else
        {
            Debug.Log("Click");
            ClickEvent?.Invoke(TouchPosition());
        }
        Reset();
    }
    private void Reset()
    {
        _startTouch = _swipeDelta = Vector2.zero;
        _touchMoved = false;
        for (int i = 0; i < _swipes.Length; i++)
        {
            _swipes[i] = false;
        }
    }
}
