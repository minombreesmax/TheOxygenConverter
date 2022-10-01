using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Furma : MonoBehaviour
{
    private const float SPEED = 0.1f;
    private const int MAX_HEIGHT = 51;
    private const int MIN_HEIGHT = 36;
    private const int MAX_ALLOWABLE_ANGLE = 357;
    private const int MIN_ALLOWABLE_ANGLE = 3;

    private Rigidbody furmaRigidbody;

    private void Start()
    {
        furmaRigidbody = GetComponent<Rigidbody>();
        furmaRigidbody.position = new Vector3(0, MAX_HEIGHT, 0);
    }

    private void FurmaMoving() 
    {
        float positionY = furmaRigidbody.position.y;

        if (DataHolder.converterTurn <= MIN_ALLOWABLE_ANGLE || DataHolder.converterTurn > MAX_ALLOWABLE_ANGLE)
        {
            if (Input.GetKey(KeyCode.UpArrow) && positionY < MAX_HEIGHT)
            {
                positionY += SPEED;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && positionY > MIN_HEIGHT)
            {
                positionY -= SPEED;
            }
        }

        DataHolder.furmaInConverter = positionY < (MAX_HEIGHT - 5) ? true : false;
        furmaRigidbody.position = new Vector3(0, positionY, 0);
    }

    private void FixedUpdate()
    {
        FurmaMoving();
    }
}
