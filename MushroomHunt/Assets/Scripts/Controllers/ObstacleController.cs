using UnityEngine;
public class ObstacleController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UImanager.Instance.ShowLosePanel();
            SoundManager.Instance.PlayObstacleCollisionSound();
        }
    }
}
