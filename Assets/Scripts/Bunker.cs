using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bunker : MonoBehaviour
{
    public GameObject Flap;
    public Button Open, Close;
    public Slider productivitySlider;
    public Text productivityText;
    
    private Collider flapCollider;

    void Start()
    {
        flapCollider = Flap.GetComponent<Collider>();
        SetButtons(true, false);
        StartCoroutine(SetProductivityValue());
    }

    public IEnumerator SetProductivityValue() 
    {
        while (true)
        {
            yield return null;
            productivityText.text = $"{(int)productivitySlider.value}";
        }
    }

    public void FlapOpen() 
    {
        SetButtons(false, true);
    }

    public void FlapClose() 
    {
        SetButtons(true, false);
    }

    private void SetButtons(bool buttonOpen, bool buttonClose)
    {
        flapCollider.isTrigger = buttonOpen ? false : true;
        Open.interactable = buttonOpen;
        Close.interactable = buttonClose;
    }

}
