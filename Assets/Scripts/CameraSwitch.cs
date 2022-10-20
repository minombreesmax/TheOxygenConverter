using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject MainCamera, BunkersCamera;

    void Start()
    {
        MainCamera.SetActive(true);
        BunkersCamera.SetActive(false);
    }

    private void SwitchCamera() 
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            MainCamera.SetActive(false);
            BunkersCamera.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            MainCamera.SetActive(true);
            BunkersCamera.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        SwitchCamera();
    }

}
