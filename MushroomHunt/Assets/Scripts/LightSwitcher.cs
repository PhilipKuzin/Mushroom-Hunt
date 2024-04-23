using UnityEngine;
public class LightSwitcher : MonoBehaviour
{
    private Light _directLight;
    private int _pressCounter = 0;

    private void Awake()
    {
        _directLight = GetComponent<Light>();
    }
    public void Switch()
    {
        if (_pressCounter % 2 == 0) 
            _directLight.enabled = false;

        if (_pressCounter % 2 == 1)
            _directLight.enabled = true;
 
        _pressCounter++;
    }
}
