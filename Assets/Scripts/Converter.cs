using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Converter : MonoBehaviour
{
    public GameObject converterSteel, carrierSteel;
    public GameObject converterSlag, carrierSlag;
    public Text converterTurnText; 

    private Rigidbody converterRigidbody;
    private float rotationX = -90;
    private int turn;
    private bool steelFlood = false, slagFlood = false;

    void Start()
    {
        converterRigidbody = GetComponent<Rigidbody>();
    }

    private IEnumerator SteelMerging() 
    {
        while (true)
        {
            if (!steelFlood)
            {
                converterSteel.SetActive(true);
                DataHolder.carrierGo = false;
                DataHolder.steelMerging = true;
                steelFlood = true; 
            }
            else
            {
                yield return new WaitForSeconds(10);
                DataHolder.carrierGo = true;
                DataHolder.steelMerging = false;
                converterSteel.SetActive(false);
                StopCoroutine(SteelMerging()); 
            }

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator SlagMerging() 
    {
        while (true)
        {
            if (!slagFlood)
            {
                converterSlag.SetActive(true);
                DataHolder.carrierGo = false;
                DataHolder.slagMerging = true;
                slagFlood = true;
            }
            else
            {
                yield return new WaitForSeconds(12);
                DataHolder.carrierGo = true;
                DataHolder.slagMerging = false;
                converterSlag.SetActive(false);
                StopCoroutine(SlagMerging());
            }

            yield return new WaitForSeconds(1);
        }
    }

    private void SetRotation(float x, float y, float z)
    {
        converterRigidbody.rotation = Quaternion.Euler(x, y, z);
        TurnCalculate(x);
    }

    private void ConverterRotate() 
    {
        if (!DataHolder.furmaInConverter && DataHolder.carrierGo)
        {
            if (Input.GetKey(KeyCode.RightArrow) && rotationX < -15)
            {
                rotationX++;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && rotationX > -205)
            {
                rotationX--;
            }

            if(DataHolder.converterTurn == 285) 
            {
                StartCoroutine(SteelMerging());
            }

            if(DataHolder.converterTurn == 115) 
            {
                StartCoroutine(SlagMerging());
            }

        }

        SetRotation(rotationX, 90, 0);
    }

    private void TurnCalculate(float angle) 
    {
        turn = (int)(angle + 90);
        turn = converterRigidbody.rotation.y > 0.5 ? 360 - turn : -turn;
        converterTurnText.text = $"{turn}°";
        DataHolder.converterTurn = turn;
    }

    void FixedUpdate()
    {
        ConverterRotate();
    }
}
