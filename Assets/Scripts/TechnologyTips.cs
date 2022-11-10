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
                TipsText.text = "Порядок дій:\n Крок 1: Засипте скрап до конвертеру та згенеруйте його склад";
            }
            else if (!DataHolder.Steps[1])
            {
                TipsText.text = "Порядок дій:\n Крок 2: Залийте чавун до конвертеру,введіть його склад або застосуйте склад за замовченням";
            }
            else if (!DataHolder.Steps[2]) 
            {
                TipsText.text = "Порядок дій:\n Крок 3: Введить %С в сталі та розрахуйте її температуру";
            }
            else if (!DataHolder.Steps[3]) 
            {
                TipsText.text = "Порядок дій:\n Крок 4: Введіть значення основності шлаку або застосуйте за замовченням";
            }
            else if (!DataHolder.Steps[4]) 
            {
                TipsText.text = "Порядок дій:\n Крок 5: Розрахуйте металошихту";
            }
            else 
            {
                TipsText.text = "Порядок дій:\n Крок 6: Почніть плавку. Кут повороту конвертеру має бути 0°";
            }

            if (DataHolder.smelting) 
            {
                TipsText.text = "";
            }

            if (DataHolder.release) 
            {
                TipsText.text = "Порядок дій:\n Крок 7: Злийте сталь та шлак. Для цього спочатку викличте сталевоз та шлаковоз";
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
