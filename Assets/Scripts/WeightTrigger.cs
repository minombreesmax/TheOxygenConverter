using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightTrigger : MonoBehaviour
{
    protected const float MASS = 0.97f;

    public Text[] UnloadingPlaceholders;
    public Text MainPlaceholder;

    private void Start()
    {
        StartCoroutine(UnloadingUpdate());
    }

    private IEnumerator UnloadingUpdate() 
    {
        while (true) 
        {
            float sum = 0;

            for(int i = 0; i < 4; i++) 
            {
                UnloadingPlaceholders[i].text = $"{Math.Round(DataHolder.UnloadMaterials[i], 0)}";
                sum += DataHolder.UnloadMaterials[i];
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
                DataHolder.UnloadMaterials[0] += MASS;
                break;
            case "Flurite":
                DataHolder.UnloadMaterials[1] += MASS;
                break;
            case "Staurolite":
                DataHolder.UnloadMaterials[2] += MASS;
                break;
            case "Som":
                DataHolder.UnloadMaterials[3] += MASS;
                break;
        }
    }
}
