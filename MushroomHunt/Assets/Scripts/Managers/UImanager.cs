using TMPro;
using UnityEngine;

public class UImanager : SingletonGeneric<UImanager>
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private TMP_Text _shroomsNumber;
    public void Pause()
    {
        Time.timeScale = 0f;
        PlayerController.Instance.PauseMovement();
        _pausePanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        PlayerController.Instance.ResumeMovement();
        _pausePanel.SetActive(false);
    }

    public void ShowLosePanel ()
    {
        Time.timeScale = 0f;
        _shroomsNumber.text = CollectController.Instance.Counter.ToString();
        _losePanel.SetActive(true);
    }

    public void Home ()
    {
        HideLosePanel();
        PlayerController.Instance.ResetGame();
    }
    private void HideLosePanel()
    {
        Time.timeScale = 1f;
        _losePanel.SetActive(false);
    }
}


