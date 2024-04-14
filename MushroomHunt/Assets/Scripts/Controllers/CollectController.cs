using TMPro;
using UnityEngine;

public class CollectController : SingletonGeneric<CollectController>
{
    [SerializeField] private TMP_Text textDisplay;
    public int Counter { get; private set; }

    private void Update()
    {
        textDisplay.text = Counter.ToString();
    }

    public void ResetCollectController ()
    {
        Counter = 0;
    }
    public void Collect() 
    {
        Counter++;
    }
}
