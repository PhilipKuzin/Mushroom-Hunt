using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    public void Pause ()
    {
        Time.timeScale = 0f;
        _pausePanel.SetActive(true);
    }

    public void Resume ()
    {
        Time.timeScale = 1.0f;
        _pausePanel.SetActive(false);
    }
}
