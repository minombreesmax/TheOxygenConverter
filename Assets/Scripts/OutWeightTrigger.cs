using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutWeightTrigger : WeightTrigger
{
    protected override void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Lime":
                DataHolder.UnloadMaterials[0] -= MASS;
                break;
            case "Flurite":
                DataHolder.UnloadMaterials[1] -= MASS;
                break;
            case "Staurolite":
                DataHolder.UnloadMaterials[2] -= MASS;
                break;
            case "Som":
                DataHolder.UnloadMaterials[3] -= MASS;
                break;
        }
    }

}
