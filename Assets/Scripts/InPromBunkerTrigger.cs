using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InPromBunkerTrigger : MonoBehaviour
{
    protected const float MASS = 0.97f;

    public Text[] MaterialWeightPlaceholders;
    public Text MainPlaceholder;

    private void Start()
    {
        StartCoroutine(UnloadingUpdate());
    }

    protected IEnumerator UnloadingUpdate()
    {
        while (true)
        {
            float sum = 0;

            for (int i = 0; i < 4; i++)
            {
                MaterialWeightPlaceholders[i].text = $"{Math.Round(DataHolder.PromBunkerMaterials[i], 0)}";
                sum += DataHolder.PromBunkerMaterials[i];
                sum = sum < 0 ? 0 : sum;
            }

            MainPlaceholder.text = $"{Math.Round(sum, 0)}";

            yield return new WaitForSeconds(1);
        }
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Lime":
                DataHolder.PromBunkerMaterials[0] += MASS;
                break;
            case "Flurite":
                DataHolder.PromBunkerMaterials[1] += MASS;
                break;
            case "Staurolite":
                DataHolder.PromBunkerMaterials[2] += MASS;
                break;
            case "Som":
                DataHolder.PromBunkerMaterials[3] += MASS;
                break;
        }
    }



}
