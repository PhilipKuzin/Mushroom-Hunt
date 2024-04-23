using UnityEngine;
public class SoundManager : SingletonGeneric<SoundManager>
{
    [SerializeField] private AudioClip _collectSound;
    [SerializeField] private AudioClip _obstacleCollisionSound;
    [SerializeField] private AudioClip _uiTouchSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayCollectSound()
    {
        _audioSource.PlayOneShot(_collectSound);
    }

    public void PlayObstacleCollisionSound()
    {
        _audioSource.PlayOneShot(_obstacleCollisionSound);
    }
    public void PlayUISound()
    {
        _audioSource.PlayOneShot(_uiTouchSound);
    }

}
