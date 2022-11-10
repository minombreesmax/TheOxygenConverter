using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechnologyTips : MonoBehaviour
{
    private Text TipsText;
    public Button PauseButton, ResumeButton;
    public GameObject PausePanel;

    private void Start()
    {
        TipsText = GetComponent<Text>();
        PausePanel.SetActive(false);
        StartCoroutine(Technology());
    }

    private IEnumerator Technology() 
    {
        while (true)
        {
            if (!DataHolder.Steps[0])
            {
                TipsText.text = "������� ��:\n ���� 1: ������� ����� �� ���������� �� ���������� ���� �����";
            }
            else if (!DataHolder.Steps[1])
            {
                TipsText.text = "������� ��:\n ���� 2: ������� ����� �� ����������,������ ���� ����� ��� ���������� ����� �� �����������";
            }
            else if (!DataHolder.Steps[2]) 
            {
                TipsText.text = "������� ��:\n ���� 3: ������� %� � ���� �� ���������� �� �����������";
            }
            else if (!DataHolder.Steps[3]) 
            {
                TipsText.text = "������� ��:\n ���� 4: ������ �������� ��������� ����� ��� ���������� �� �����������";
            }
            else if (!DataHolder.Steps[4]) 
            {
                TipsText.text = "������� ��:\n ���� 5: ���������� �����������";
            }
            else 
            {
                TipsText.text = "������� ��:\n ���� 6: ������ ������. ��� �������� ���������� �� ���� 0�";
            }

            if (DataHolder.smelting) 
            {
                TipsText.text = "";
            }

            if (DataHolder.release) 
            {
                TipsText.text = "������� ��:\n ���� 7: ������ ����� �� ����. ��� ����� �������� �������� �������� �� ��������";
            }

            if(DataHolder.slagCarrierReady && DataHolder.steelCarrierReady) 
            {
                TipsText.text = "";
                break;
            }
            
            yield return new WaitForSeconds(2);
        }
    }
}
