using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BunkersUI : MonoBehaviour
{
    private const int BUNKERS_COUNT = 6;
    private const float POURING_SPEED = 0.2f;

    public Text[] BunkersPlaceholders;
    public Button[] BunkersOpenButton;

    private bool[] BunkerIsOpen = new bool[BUNKERS_COUNT];
    private float[] BunkersStartFill = new float[BUNKERS_COUNT];

    private void Start()
    {
        BunkersFill();
        StartCoroutine(BunkersStatus());
        StartCoroutine(ChargePouring());
    }

    private void BunkersFill() 
    {
        DataHolder.BunkersFilling[0] = UnityEngine.Random.Range(1000, 5000);
        DataHolder.BunkersFilling[1] = UnityEngine.Random.Range(4000, 5000);
        DataHolder.BunkersFilling[2] = UnityEngine.Random.Range(150, 200);
        DataHolder.BunkersFilling[3] = UnityEngine.Random.Range(500, 1000);
        DataHolder.BunkersFilling[4] = UnityEngine.Random.Range(50, 200);
        DataHolder.BunkersFilling[5] = UnityEngine.Random.Range(1000, 5000);

        for(int i = 0; i < BUNKERS_COUNT; i++) 
        {
            BunkersStartFill[i] = DataHolder.BunkersFilling[i];
        }

        UpdateBunkersFill();
    }

    private float BunkerFillLevel(int bunkerNumber) 
    {
        return bunkerNumber <= BUNKERS_COUNT? DataHolder.BunkersFilling[bunkerNumber] * 100 / BunkersStartFill[bunkerNumber] : 0;
    }

    private void UpdateBunkersFill() 
    {
        for (int i = 0; i < BunkersPlaceholders.Length; i++)
        {
            BunkersPlaceholders[i].text = $"{Math.Round(BunkerFillLevel(i), 0)} %";
        }
    }

    private IEnumerator BunkersStatus() 
    {
        while (true)
        {
            for (int i = 0; i < BUNKERS_COUNT; i++)
            {
                BunkerIsOpen[i] = !BunkersOpenButton[i].interactable;
            }

            yield return null;
        }
    }

    private IEnumerator ChargePouring() 
    {
        while (true) 
        {
            for(int i = 0; i< BUNKERS_COUNT; i++) 
            {
                if (BunkerIsOpen[i]) 
                {
                    DataHolder.BunkersFilling[i] = DataHolder.BunkersFilling[i] > 0 ? DataHolder.BunkersFilling[i] -= POURING_SPEED : 0;
                }
            }

            UpdateBunkersFill();

            yield return null;
        }
    }
}
