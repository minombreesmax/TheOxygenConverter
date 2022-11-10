using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladle : MonoBehaviour
{
    private const float SPEED = 0.15f;

    public GameObject Iron, IronLevel;
    private Animator ladleAnimator;
    private Rigidbody ladleRigidbody;
    private float positionX, positionY;
    private bool flooded, moving;

    private void Start()
    {
        ladleRigidbody = GetComponent<Rigidbody>();
        ladleAnimator = GetComponent<Animator>();
        flooded = false; moving = true;
        DataHolder.ironPoured = false;
        Iron.SetActive(flooded);
    }

    public IEnumerator LadleLoadAnim()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Q) && ladleRigidbody.position.x > -20)
            {
                if (Between(DataHolder.converterTurn, 30, 50) && Between(transform.position.y, -2f, 1f))
                {
                    if (!flooded)
                    {
                        moving = false;
                        ladleAnimator.Play("LadleLoad");
                        yield return new WaitForSeconds(2f);
                        DataHolder.ladleLoad = true;
                        Iron.SetActive(true);
                    }

                    if (Iron.activeInHierarchy)
                    {
                        yield return new WaitForSeconds(7.5f);
                        Iron.SetActive(false);
                        IronLevel.SetActive(false);
                        flooded = true;
                        ladleAnimator.Play("LadleUnload");
                        DataHolder.ladleLoad = false;
                        DataHolder.ironPoured = true;
                        moving = true;
                    }
                }
            }

            yield return null;
        }
    }

    private void Moving()
    {
        if (moving)
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
    }

    private bool Between(int value, int min, int max) 
    {
        return value > min && value < max ? true : false;
    }

    private bool Between(float value, float min, float max) 
    {
        return value > min && value < max ? true : false;
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
