using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Furma : MonoBehaviour
{
    private const float SPEED = 0.05f;
    private const int POS_SPEED = 50;
    private const int MAX_HEIGHT = 51;
    private const int MIN_HEIGHT = 36;
    private const int MAX_ALLOWABLE_ANGLE = 357;
    private const int MIN_ALLOWABLE_ANGLE = 3;

    private Rigidbody furmaRigidbody;
    

    private void Start()
    {
        furmaRigidbody = GetComponent<Rigidbody>();
        furmaRigidbody.position = new Vector3(0, MAX_HEIGHT, 0);
        DataHolder.furmaPosition = 15000;
        DataHolder.furmaSet = 0;
    }

    private void FurmaMoving(int direction) 
    {
        float positionY = furmaRigidbody.position.y;

        if (DataHolder.converterTurn <= MIN_ALLOWABLE_ANGLE || DataHolder.converterTurn > MAX_ALLOWABLE_ANGLE)
        {
            if (direction == 1 && positionY < MAX_HEIGHT)
            {
                positionY += SPEED;
                DataHolder.furmaPosition += POS_SPEED;
            }
            else if (direction == -1 && positionY > MIN_HEIGHT + SPEED)
            {
                positionY -= SPEED;
                DataHolder.furmaPosition -= POS_SPEED;
            }

            DataHolder.smelting = DataHolder.furmaPosition < 7500 ? true : false;
        }

        DataHolder.furmaInConverter = positionY < (MAX_HEIGHT - 5) ? true : false;
        furmaRigidbody.position = new Vector3(0, positionY, 0);
    }

    private void FixedUpdate()
    {
        FurmaMoving(DataHolder.furmaSet);
    }
}
