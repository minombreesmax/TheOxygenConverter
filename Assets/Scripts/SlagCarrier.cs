using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlagCarrier : MonoBehaviour
{
    public GameObject Slag;
    public float position;
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        Slag.SetActive(false);
        StartCoroutine(SlagLevel());
        StartCoroutine(CarrierReady());
    }

    private IEnumerator SlagLevel()
    {
        while (true)
        {
            if (DataHolder.slagMerging)
            {
                Slag.SetActive(true);
                animator.Play("SlagAnimation"); 
                StopCoroutine(SlagLevel());
            }

            yield return new WaitForSeconds(1);
        }

    }

    private IEnumerator CarrierReady()
    {
        while (true) 
        {
            DataHolder.slagCarrierReady = transform.position.z <= position ? true : false;
            yield return new WaitForSeconds(1);
        }
    }

}
