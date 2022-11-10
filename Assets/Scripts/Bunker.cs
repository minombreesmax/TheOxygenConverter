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

    public virtual void FlapOpen() 
    {
        SetButtons(false, true);
    }

    public virtual void FlapClose() 
    {
        SetButtons(true, false);
    }

    protected void SetButtons(bool buttonOpen, bool buttonClose)
    {
        flapCollider.isTrigger = buttonOpen ? false : true;
        Open.interactable = buttonOpen;
        Close.interactable = buttonClose;
    }

   
}
