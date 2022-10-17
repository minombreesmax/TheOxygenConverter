using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    private Rigidbody scrapRigidbody;

    private void Start()
    {
        scrapRigidbody = GetComponent<Rigidbody>();
        scrapRigidbody.isKinematic = true;
        StartCoroutine(IsKinematicOff());
    }

    private IEnumerator IsKinematicOff()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (DataHolder.scoopLoad)
            {
                scrapRigidbody.isKinematic = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Converter") 
        {
            Destroy(gameObject);
            StopCoroutine(IsKinematicOff());
        }
    }
}
