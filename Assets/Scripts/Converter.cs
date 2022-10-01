using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : MonoBehaviour
{
    private Rigidbody converterRigidbody;
    private float rotationX = -90;
    private int turn;

    void Start()
    {
        converterRigidbody = GetComponent<Rigidbody>();
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
            if (Input.GetKey(KeyCode.RightArrow) && rotationX < 0)
            {
                rotationX++;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && rotationX > -210)
            {
                rotationX--;
            }
        }

        SetRotation(rotationX, 90, 0);
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
