using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviour
{
    public GameObject MainCamera, BunkersCamera;
    public Button MainCameraButton, BunkersCameraButton;

    void Start()
    {
        SetMainCamera();
    }

    private void SwitchCamera() 
    {
        if (DataHolder.currentCamera == "BunkersCamera")
        {
            SetBunkersCamera();
        }
        else if (DataHolder.currentCamera == "MainCamera")
        {
            SetMainCamera();
        }
    }

    public void SetMainCamera() 
    {
        DataHolder.currentCamera = "MainCamera";
        MainCamera.SetActive(true);
        BunkersCamera.SetActive(false);

        MainCameraButton.interactable = false;
        BunkersCameraButton.interactable = true;
    }

    public void SetBunkersCamera() 
    {
        DataHolder.currentCamera = "BunkersCamera";
        MainCamera.SetActive(false);
        BunkersCamera.SetActive(true);

        MainCameraButton.interactable = true;
        BunkersCameraButton.interactable = false;
    }

    private void FixedUpdate()
    {
        SwitchCamera();
    }

}
