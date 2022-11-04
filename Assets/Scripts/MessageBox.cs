using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text MesText;

    public void OK()
    {
        MesText.text = "";
        gameObject.SetActive(false);
    }
}
