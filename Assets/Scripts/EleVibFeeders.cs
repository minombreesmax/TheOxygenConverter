using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EleVibFeeders : MonoBehaviour
{
    public Slider productivitySlider;
    public Collider Flap;

    private Rigidbody eleVibFeederRigidbody;
    private int productivity;

    void Start()
    {
        eleVibFeederRigidbody = GetComponent<Rigidbody>();
        StartCoroutine(SetProductivity());
    }

    private IEnumerator SetProductivity()
    {
        while (true)
        {
            yield return null;
            productivity = (int)productivitySlider.value;
            eleVibFeederRigidbody.rotation = Quaternion.Euler(ProductivityFunc(productivity), 0, 0);
            Flap.isTrigger = productivity == 0? false : true;
        }
    }

    private float ProductivityFunc(int prodProcent)
    {
        return -0.06f * prodProcent + 1;
    }
}
