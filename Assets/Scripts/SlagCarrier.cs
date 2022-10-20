using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlagCarrier : MonoBehaviour
{
    public GameObject Slag;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Slag.SetActive(false);
        StartCoroutine(SlagLevel());
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
}
