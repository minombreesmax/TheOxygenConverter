using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutPromBunkerTrigger : InPromBunkerTrigger
{
    protected override void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Lime":
                DataHolder.PromBunkerMaterials[0] -= MASS;
                break;
            case "Flurite":
                DataHolder.PromBunkerMaterials[1] -= MASS;
                break;
            case "Staurolite":
                DataHolder.PromBunkerMaterials[2] -= MASS;
                break;
            case "Som":
                DataHolder.PromBunkerMaterials[3] -= MASS;
                break;
        }
    }
}
