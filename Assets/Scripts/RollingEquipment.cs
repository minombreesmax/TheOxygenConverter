using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingEquipment : MonoBehaviour
{
    public GameObject Scoop;
    public GameObject Ladle;
    public GameObject SlagCarrier;
    public GameObject SteelCarrier;

    private Animator scoopAnimator;
    private Animator ladleAnimator;
    private Animator slagCarrierAnimator;
    private Animator steelCarrierAnimator;
    private bool substitution = false, driveUp = false;

    private void Start()
    {
        ScoopLadleStart();
        CarriersStart();
    }

    private IEnumerator ScoopLadleSubstitutions()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.E) && DataHolder.scrapLoaded && !DataHolder.scoopLoad && !DataHolder.ladleLoad) 
            {
                if (!substitution) 
                {
                    Ladle.SetActive(true);
                    ladleAnimator.Play("LadleComeIn");
                    scoopAnimator.Play("ScoopGetOut");

                    yield return new WaitForSeconds(1.5f);

                    Scoop.SetActive(false);
                    substitution = true;
                }
                else 
                {
                    Scoop.SetActive(true);
                    scoopAnimator.Play("ScoopComeIn");
                    ladleAnimator.Play("LadleGetOut");

                    yield return new WaitForSeconds(1.5f);

                    Ladle.SetActive(false);
                    substitution = false;
                }

                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
    }

    private IEnumerator CarriersControl()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.R) && DataHolder.carrierGo)
            {
                if (!driveUp)
                {
                    slagCarrierAnimator.Play("SlagCarrierComeIn");
                    steelCarrierAnimator.Play("SteelCarrierComeIn");
                    yield return new WaitForSeconds(4f);
                    driveUp = true;
                }
                else
                {
                    slagCarrierAnimator.Play("SlagCarrierGetOut");
                    steelCarrierAnimator.Play("SteelCarrierGetOut");
                    yield return new WaitForSeconds(4f);
                    driveUp = false;
                }
            }

            yield return null;
        }
    }

    private void ScoopLadleStart()
    {
        scoopAnimator = Scoop.GetComponent<Animator>();
        ladleAnimator = Ladle.GetComponent<Animator>();

        Scoop.transform.position = new Vector3(10, 20, 3);
        Ladle.transform.position = new Vector3(-25, 0, 3);

        Scoop.SetActive(true);
        Ladle.SetActive(false);

        scoopAnimator.Play("ScoopComeIn");
        StartCoroutine("ScoopLadleSubstitutions");
    }

    private void CarriersStart() 
    {
        slagCarrierAnimator = SlagCarrier.GetComponent<Animator>();
        steelCarrierAnimator = SteelCarrier.GetComponent<Animator>();

        SlagCarrier.transform.position = new Vector3(-15, 1.35f, 35);
        SteelCarrier.transform.position = new Vector3(-1, -1.2f, 39);

        DataHolder.carrierGo = true;
        StartCoroutine("CarriersControl");
    }

}
