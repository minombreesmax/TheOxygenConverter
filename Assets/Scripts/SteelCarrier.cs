using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelCarrier : MonoBehaviour
{
    public GameObject Steel;
    public float position;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Steel.SetActive(false);
        StartCoroutine(SteelLevel());
        StartCoroutine(CarrierReady());
    }

    private IEnumerator SteelLevel() 
    {
        while (true)
        {
            if (DataHolder.steelMerging)
            {
                Steel.SetActive(true);
                animator.Play("SteelAnimation");
                StopCoroutine(SteelLevel());
            }

            yield return new WaitForSeconds(1);
        }

    }

    private IEnumerator CarrierReady()
    {
        while (true)
        {
            DataHolder.steelCarrierReady = transform.position.z <= position ? true : false;
            yield return new WaitForSeconds(1);
        }
    }
}
