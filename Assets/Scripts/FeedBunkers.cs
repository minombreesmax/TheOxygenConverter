using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBunkers : Bunker
{
    public GameObject PartOfShkhta, BunkerObject;
    public int index;
    private bool flapIsClose;

    public override void FlapOpen()
    {
        SetButtons(false, true);
        flapIsClose = false;
        StartCoroutine(ChargePouring());
    }

    public override void FlapClose()
    {
        SetButtons(true, false);
        flapIsClose = true;
    }

    private IEnumerator ChargePouring()
    {
        while (true)
        {
            var position = BunkerObject.transform.position;
            position.x += Random.Range(-1f, 1f);
            position.z += Random.Range(-1f, 1f);

            Instantiate(PartOfShkhta, position, Quaternion.identity);

            if (flapIsClose || DataHolder.BunkersFilling[index] == 0)
                break;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
