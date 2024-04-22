using UnityEngine;

public class MushroomController : MonoBehaviour
{
    private float _rotationSpeed = 100;

    private void Start()
    {
        _rotationSpeed += Random.Range(0, _rotationSpeed / 4f);
    }

    private void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {         
            CollectController.Instance.Collect();
            SoundManager.Instance.PlayCollectSound();
            transform.parent.gameObject.SetActive(false);
        }
    }
}
