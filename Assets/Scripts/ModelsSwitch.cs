using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelsSwitch : MonoBehaviour
{
    public GameObject StaticModel, DynamicModel, Table1, Table2;
    public Button StaticModelButton, DynamicModelButton;
    public Button ChemComposButton, ImpuritiesOxidationButton;

    private void Start()
    {
        StaticModelButton.interactable = false;
        DynamicModelButton.interactable = true;
        ChemComposButton.interactable = false;
        ImpuritiesOxidationButton.interactable = true;
        Table1.SetActive(true);
        Table2.SetActive(false);
    }

    public void SetStaticModel() 
    {
        StaticModelButton.interactable = false;
        DynamicModelButton.interactable = true;
        StaticModel.SetActive(true);
        DynamicModel.SetActive(false);
    }

    public void SetDynamicModel() 
    {
        DynamicModelButton.interactable = false;
        StaticModelButton.interactable = true;
        DynamicModel.SetActive(true);
        StaticModel.SetActive(false);
    }

    public void ChemCompos() 
    {
        ChemComposButton.interactable = false;
        ImpuritiesOxidationButton.interactable = true;
        Table1.SetActive(true);
        Table2.SetActive(false);
    }

    public void ImpuritiesOxidation() 
    {
        ImpuritiesOxidationButton.interactable = false;
        ChemComposButton.interactable = true;
        Table2.SetActive(true);
        Table1.SetActive(false);
    }



}
