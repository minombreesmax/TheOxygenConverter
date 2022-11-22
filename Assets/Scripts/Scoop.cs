using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoop : MonoBehaviour
{
    private const float SPEED = 0.15f;

    public GameObject[] ScrapObjects;
    public GameObject ScoopObject;

    private Animator scoopAnimator;
    private Rigidbody scoopRigidbody;
    private float positionX, positionY;
    private int scrapCount = 0;
    private bool unloadingPermission;

    private void Start()
    {
        scoopRigidbody = GetComponent<Rigidbody>();
        scoopAnimator = GetComponent<Animator>();
        DataHolder.scoopLoad = false;
        DataHolder.scrapLoaded = false;
        StartCoroutine(ScrapSpawn());
    }

    private void Moving() 
    {
        positionX = scoopRigidbody.position.x;
        positionY = scoopRigidbody.position.y;

        if (!DataHolder.scoopLoad)
        {
            if (Input.GetKey(KeyCode.D) && positionX < 14f)
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
            else if (Input.GetKey(KeyCode.S) && positionY > 15f)
            {
                positionY -= SPEED;
            }
        }
        else 
        {
            if (Input.GetKey(KeyCode.D) && positionX < 15f)
            {
                positionX += SPEED;
            }
            else if (Input.GetKey(KeyCode.A) && positionX > 13.5f)
            {
                positionX -= SPEED;
            }

            if (Input.GetKey(KeyCode.W) && positionY < 21f)
            {
                positionY += SPEED;
            }
            else if (Input.GetKey(KeyCode.S) && positionY > 18f)
            {
                positionY -= SPEED;
            }
        }

        unloadingPermission = positionX > 14 && Between(positionY, 18, 22) ? true : false;
        scoopRigidbody.position = new Vector3 (positionX, positionY, 3);
    }

    private IEnumerator ScrapSpawn() 
    {
        while (true) 
        {
            if (DataHolder.scoopLoad)
            {
                var i = Random.Range(0, 2);
                var position = ScoopObject.transform.position;
                var rotation = Quaternion.Euler (0, 0, 90);

                for (int j = 0; j < 5; j++)
                {
                    Instantiate(ScrapObjects[i], position, rotation);
                    scrapCount++;
                }

                if(scrapCount == 25) 
                {
                    break;
                }

                yield return new WaitForSeconds(0.25f);
            }

            yield return null; 
        }
    }

    private IEnumerator ScoopLoadAnim() 
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Q) && unloadingPermission)
            {
                if (!DataHolder.scoopLoad)
                {
                    scoopAnimator.Play("ScoopLoad");
                    yield return new WaitForSeconds(2f);
                    DataHolder.scoopLoad = true;
                    DataHolder.scrapLoaded = true;
                }
                else
                {
                    scoopAnimator.Play("ScoopUnload");
                    yield return new WaitForSeconds(2f);
                    DataHolder.scoopLoad = false;
                }
            }

            yield return null;
        }
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
            StartCoroutine(ScoopLoadAnim());
        }
        else 
        {
            StopCoroutine(ScoopLoadAnim());
        }
    }
}
