using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectController : SingletonGeneric<CollectController>
{
    [SerializeField] private int _counter;
    public TMP_Text textDisplay;

    private void Update()
    {
        textDisplay.text = "" + _counter;
    }

    public void ResetCollectController ()
    {
        _counter = 0;
    }
    public void Collect()
    {
        _counter++;
    }
}
