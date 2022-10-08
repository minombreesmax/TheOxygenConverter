using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoop : MonoBehaviour
{
    private const float SPEED = 0.15f;

    private Animator scoopAnimator;
    private Rigidbody scoopRigidbody;
    private float positionX, positionY;
    private bool load;

    private void Start()
    {
        scoopRigidbody = GetComponent<Rigidbody>();
        scoopAnimator = GetComponent<Animator>();
        load = false;
    }

    private void Moving() 
    {
        positionX = scoopRigidbody.position.x;
        positionY = scoopRigidbody.position.y;
        
        if (Input.GetKey(KeyCode.D)) 
        {
            positionX += SPEED;
        }
        else if (Input.GetKey(KeyCode.A) && positionX > 0) 
        {
            positionX -= SPEED;
        }

        if (Input.GetKey(KeyCode.W) && positionY < 25) 
        {
            positionY += SPEED;
        }
        else if (Input.GetKey(KeyCode.S)) 
        {
            positionY -= SPEED;
        }

        scoopRigidbody.position = new Vector3 (positionX, positionY, 3);
    }

    public IEnumerator ScoopLoadAnim() 
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!load)
                {
                    scoopAnimator.Play("ScoopLoad");
                    yield return new WaitForSeconds(2f);
                    load = true;
                }
                else
                {
                    scoopAnimator.Play("ScoopUnload");
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
            StartCoroutine(ScoopLoadAnim());
        }
        else 
        {
            StopCoroutine(ScoopLoadAnim());
        }
    }
}
