using TMPro;

public class CollectController : SingletonGeneric<CollectController>
{
    public TMP_Text textDisplay;
    public int Counter { get; private set; }

    private void Update()
    {
        textDisplay.text = "" + Counter;
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
