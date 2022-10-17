using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : MonoBehaviour
{
    public GameObject converterSteel, carrierSteel;

    private Rigidbody converterRigidbody;
    private float rotationX = -90;
    private int turn;

    void Start()
    {
        converterRigidbody = GetComponent<Rigidbody>();
        converterSteel.SetActive(false); 
        carrierSteel.SetActive(false);
        SetRotation(rotationX, 90, 0);
    }

    private void SetRotation(float x, float y, float z) 
    {
        converterRigidbody.rotation = Quaternion.Euler(x, y, z);
        TurnCalculate(x);
    }

    private void ConverterRotate() 
    {
        if (DataHolder.furmaInConverter == false)
        {
            if (Input.GetKey(KeyCode.RightArrow) && rotationX < -15)
            {
                rotationX++;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && rotationX > -205)
            {
                rotationX--;
            }
        }

        var casting = DataHolder.converterTurn == 285 ? true : false;
        converterSteel.SetActive(casting);
        carrierSteel.SetActive(casting);

        SetRotation(rotationX, 90, 0);
        print(DataHolder.converterTurn);
        print(rotationX);
    }

    private void TurnCalculate(float angle) 
    {
        turn = (int)(angle + 90);
        turn = converterRigidbody.rotation.y > 0.5 ? 360 - turn : -turn;
        DataHolder.converterTurn = turn;
    }

    void FixedUpdate()
    {
        ConverterRotate();
    }
}
