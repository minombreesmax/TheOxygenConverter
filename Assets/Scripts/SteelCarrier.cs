using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelCarrier : MonoBehaviour
{
    public GameObject Steel;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Steel.SetActive(false);
        StartCoroutine(SteelLevel());
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
}
