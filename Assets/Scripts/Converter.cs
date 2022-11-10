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
    private float rotationX = -90, minAngle, maxAngle;
    private int turn;
    private bool steelFlood = false, slagFlood = false;

    void Start()
    {
        converterRigidbody = GetComponent<Rigidbody>();
        DataHolder.release = false;
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
                DataHolder.steelPoured = true;
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
                yield return new WaitForSeconds(13);
                DataHolder.carrierGo = true;
                DataHolder.slagMerging = false;
                converterSlag.SetActive(false);
                DataHolder.slagPoured = true;
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
        if (!DataHolder.furmaInConverter && DataHolder.carrierGo && !DataHolder.scoopLoad && !DataHolder.ladleLoad)
        {
            if(DataHolder.steelCarrierReady && DataHolder.slagCarrierReady) 
            {
                minAngle = -205;
                maxAngle = -15;
            }
            else 
            {
                minAngle = -150;
                maxAngle = -90;
            }

            if (Input.GetKey(KeyCode.RightArrow) && rotationX < maxAngle)
            {
                rotationX++;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && rotationX > minAngle)
            {
                rotationX--;
            }

            if (DataHolder.release)
            {
                if (DataHolder.converterTurn == 285)
                {
                    StartCoroutine(SteelMerging());
                }

                if (DataHolder.converterTurn == 115)
                {
                    StartCoroutine(SlagMerging());
                }
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
