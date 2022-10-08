using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladle : MonoBehaviour
{
    private const float SPEED = 0.15f;

    private Animator ladleAnimator;
    private Rigidbody ladleRigidbody;
    private float positionX, positionY;
    private bool load;

    private void Start()
    {
        ladleRigidbody = GetComponent<Rigidbody>();
        ladleAnimator = GetComponent<Animator>();
        load = false;
        
    }

    private void Moving()
    {
        positionX = ladleRigidbody.position.x;
        positionY = ladleRigidbody.position.y;

        if (Input.GetKey(KeyCode.D))
        {
            positionX += SPEED;
        }
        else if (Input.GetKey(KeyCode.A) && positionX > -30)
        {
            positionX -= SPEED;
        }

        if (Input.GetKey(KeyCode.W) && positionY < 3)
        {
            positionY += SPEED;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            positionY -= SPEED;
        }

        ladleRigidbody.position = new Vector3(positionX, positionY, 3);
    }

    public IEnumerator LadleLoadAnim()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!load)
                {
                    ladleAnimator.Play("LadleLoad");
                    yield return new WaitForSeconds(2f);
                    load = true;
                }
                else
                {
                    ladleAnimator.Play("LadleUnload");
                    yield return new WaitForSeconds(2f);
                    load = false;
                }
            }

            yield return null;
        }
    }

    private void FixedUpdate()
    {
        if (this.gameObject.activeInHierarchy) 
        {
            Moving();
            StartCoroutine(LadleLoadAnim());
        }
        else 
        {
            StopCoroutine(LadleLoadAnim());
        }
    }
}
