using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private const int PARAMETERS_COUNT = 5; //кількість параметрів чавуну/скрапу/металу без температури;
    private const int P = 100; //тиск
    private const float DEFAULT_B = 3.25f; //основність шлаку
    private const float R = 8.31f; //газова стала

    public Text[] IronParametersPlaceholders;
    public Text[] ScrapParametersPlaceholders;
    public Text[] InputParameters;
    public Text[] MetalParametersPlaceholders;
    public Text[] Table1, Table2, Table3, Table4;

    public Text steelTtext, steelTplaceholder, T, slagBText, slagBPlaceholder;
    public Slider timeSpeed;
    public Text timeSpeedText, MessageBoxText;
    public Button ScrapRandom, IronDefault, IronApplyData, SteelTCount, SlagBDefault, SlagBApplyData, CountMetShikhta, Smelting;
    public GameObject MessageBox, ExhaustGases;

    private string[] ParametersNames = { "C", "Mn", "Si", "P", "S", "T" };
    private float[] DefaultIronParameters = { 3.9f, 1.3f, 0.7f, 0.1f, 0.024f, 1310 };
    private float[] MinIronParameters = { 1f, 0.2f, 0.2f, 0.1f, 0.02f, 1260 };
    private float[] MaxIronParameters = { 4.5f, 1.75f, 3.75f, 1.2f, 0.08f, 1450 };
    private float[] MinScrapParameters = { 0.1f, 0.4f, 0.2f, 0f, 0f, 20f };
    private float[] MaxScrapParameters = { 0.2f, 0.5f, 0.25f, 0.04f, 0.04f, 20f };
    private float[] ShikhtaParameters, IronGParameters, ScrapGParameters;
    private float[] ResidualConcentration, GParameters, MetalParameters, StartMetalParameters;
    private float[] ScrapParameters, CurrentIronParameters;
    private double[,] MaterialsParameters, SlagParameters;
    private double[] SlagGMaterials;

    private double tmp;
    private float steelC, V = 0.047f;
    
    private float deltaMn, deltaP, deltaS;
    private float scrapG, S_TO, deltascrapG, ironG;
    private float tmpG, GCO, GCO2, GSiO2, GMnO2, GPO2;
    private float VO2_CO, VO2_CO2, VO2_SiO2, VO2_MnO2, VO2_PO2;
    private float GC_CO, GC_CO2, GSi_SiO2, GMn_MnO, GP_P2O5, GS_CaS, kol_oxydov;
    private float slagB, iC, intensity = 2f;
    private int Fe_TO, j;

    private void Start()
    {
        MessageBox.SetActive(false);
        SetSteps(false);
        StartCoroutine(ButtonsStatus());
    }

    public void Ok()
    {
        MessageBoxText.text = "";
        MessageBox.SetActive(false);
    }

    public void Default()
    {
        CurrentIronParameters = new float[PARAMETERS_COUNT + 1];

        for (int i = 0; i < CurrentIronParameters.Length; i++) 
        {
            IronParametersPlaceholders[i].text = $"{DefaultIronParameters[i]}";
            InputParameters[i].text = IronParametersPlaceholders[i].text;
            CurrentIronParameters[i] = DefaultIronParameters[i];
        }

        DataHolder.Steps[1] = true;
    }

    public void DefaultSlagB() 
    {
        slagB = DEFAULT_B;
        slagBPlaceholder.text = $"{slagB}";
        DataHolder.Steps[3] = true;
    }

    public void SetupValues()
    {
        for (int i = 0; i < 6; i++)
        {
            IronParametersPlaceholders[i].text = String.IsNullOrEmpty(InputParameters[i].text) ? IronParametersPlaceholders[i].text : InputParameters[i].text;

            if (Double.TryParse(IronParametersPlaceholders[i].text, out tmp))
            {
                CurrentIronParameters[i] = !String.IsNullOrEmpty(IronParametersPlaceholders[i].text) ? (float)Convert.ToDouble(IronParametersPlaceholders[i].text) : 0;

                if (CurrentIronParameters[i] > MaxIronParameters[i])
                {
                    CurrentIronParameters[i] = MaxIronParameters[i];
                    IronParametersPlaceholders[i].text = $"{MaxIronParameters[i]}";
                    MessageBoxText.text = $"Помилка!\n Введене значення %{ParametersNames[i]} було замінено на максимально допустиме";
                    MessageBox.SetActive(true);
                }
                else if (CurrentIronParameters[i] < MinIronParameters[i])
                {
                    CurrentIronParameters[i] = MinIronParameters[i];
                    IronParametersPlaceholders[i].text = $"{MinIronParameters[i]}";
                    MessageBoxText.text = $"Помилка!\n Введене значення %{ParametersNames[i]} було замінено на  минімально допустиме";
                    MessageBox.SetActive(true);
                }
            }
            else
            {
                CurrentIronParameters[i] = (float)tmp;
                IronParametersPlaceholders[i].text = "";
                MessageBoxText.text = $"Помилка!\n Введіть %{ParametersNames[i]} правильно!";
                MessageBox.SetActive(true);
            }
        }

        for (int i = 0; i < CurrentIronParameters.Length; i++)
        {
            if (CurrentIronParameters[i] <= 0)
            {
                DataHolder.Steps[1] = false;
                break;
            }

            DataHolder.Steps[1] = true;
        }
    }

    public void RandomScrap()
    {
        ScrapParameters = new float[PARAMETERS_COUNT + 1];

        for (int i = 0; i < ScrapParameters.Length; i++)
        {
            ScrapParameters[i] = (float)Math.Round(UnityEngine.Random.Range(MinScrapParameters[i], MaxScrapParameters[i]), 2);
            ScrapParametersPlaceholders[i].text = $"{ScrapParameters[i]}";
        }

        DataHolder.Steps[0] = true;
    }

    public void SteelTButton()
    {
        MetalParameters = new float[PARAMETERS_COUNT + 1]; //параметри металу
        steelTplaceholder.text = String.IsNullOrEmpty(steelTtext.text) ? steelTplaceholder.text : steelTtext.text;

        if (Double.TryParse(steelTplaceholder.text, out tmp))
        {
            steelC = !String.IsNullOrEmpty(steelTplaceholder.text) ? (float)Convert.ToDouble(steelTplaceholder.text) : 0;

            if (steelC > 2.11)
            {
                steelC = 2.11f;
                steelTplaceholder.text = "2.11";
                MessageBoxText.text = "Помилка!\n Введене значення %С було замінено на максимально допустиме";
                MessageBox.SetActive(true);
            }
            else if (steelC < 0.05)
            {
                steelC = 0.05f;
                steelTplaceholder.text = "0.05";
                MessageBoxText.text = "Помилка!\n Введене значення %С було замінено на  минімально допустиме";
                MessageBox.SetActive(true);
            }
            else if (steelC > CurrentIronParameters[0])
            {
                steelC = 0f;
                steelTplaceholder.text = "";
                MessageBoxText.text = "Помилка!\n Введене значення %С не може бути більше ніж в чавуні";
                MessageBox.SetActive(true);
            }
        }
        else
        {
            steelC = (float)tmp;
            steelTplaceholder.text = "";
            MessageBoxText.text = "Помилка!\n Введіть %С правильно!";
            MessageBox.SetActive(true);
        }

        SteelTemperature(steelC, ref MetalParameters[5]);
        T.text = $"{MetalParameters[5]}";
        DataHolder.Steps[2] = MetalParameters[5] > 0 ? true : false;
    }

    public void SlagBButton() 
    {
        slagBPlaceholder.text = String.IsNullOrEmpty(slagBText.text) ? slagBPlaceholder.text : slagBText.text;

        if (Double.TryParse(slagBPlaceholder.text, out tmp))
        {
            slagB = !String.IsNullOrEmpty(slagBPlaceholder.text) ? (float)Convert.ToDouble(slagBPlaceholder.text) : 0;

            if (slagB > 4f)
            {
                slagB = 4f;
                slagBPlaceholder.text = "4.0";
                MessageBoxText.text = "Помилка!\n Введене значення B було замінено на максимально допустиме (4.0)";
                MessageBox.SetActive(true);
            }
            else if (slagB < 2.5f)
            {
                slagB = 2.5f;
                slagBPlaceholder.text = "2.5";
                MessageBoxText.text = "Помилка!\n Введене значення B було замінено на  минімально допустиме (2.5)";
                MessageBox.SetActive(true);
            }
            else 
            {
                slagBPlaceholder.text = $"{slagB}";
            }
        }
        else
        {
            slagB = (float)tmp;
            slagBPlaceholder.text = "";
            MessageBoxText.text = "Помилка!\n Введіть B правильно!";
            MessageBox.SetActive(true);
        }

        DataHolder.Steps[3] = true;
    }

    public void StaticModel()
    {
        IronGParameters = new float[PARAMETERS_COUNT]; //парметри витрат чавуну
        ScrapGParameters = new float[PARAMETERS_COUNT]; //параметри витрат скрапу
        ShikhtaParameters = new float[PARAMETERS_COUNT]; //параметри шихти
        ResidualConcentration = new float[PARAMETERS_COUNT]; //залишкова концентрація
        GParameters = new float[PARAMETERS_COUNT]; //видаляється під час продувки
        StartMetalParameters = new float[PARAMETERS_COUNT + 1]; //параметри розплаву до продувки
        RandomMeterials();

        //визначення витрати лома %
        scrapG = (float)(17.4 + 4.1 * (CurrentIronParameters[0] - 4.0) + 9.5 * (CurrentIronParameters[2] - 0.5) + 0.034 * (CurrentIronParameters[5] - 1330) + 3.2 * (CurrentIronParameters[1] - 0.2) + 11 * (0.2 - steelC) + 0.05 * (1650 - MetalParameters[5]));
        S_TO = 0.062f * Fe_TO - 0.014f * (float)MaterialsParameters[2,3] - 0.633f;
        deltascrapG = 0.9f * S_TO;
        scrapG = scrapG - deltascrapG;
        ironG = 100 - scrapG; //витрати чугану %

        for(int i = 0; i < PARAMETERS_COUNT; i++) 
        {
            IronGParameters[i] = ironG * CurrentIronParameters[i] / 100; // в чувуні кг/100кг
            ScrapGParameters[i] = scrapG * ScrapParameters[i] / 100; // в ломі кг/ 100кг
            ShikhtaParameters[i] = IronGParameters[i] + ScrapGParameters[i]; //вміст в шихті
        }

        if (steelC > 0.25)  //содержание в металле в конце
        {
            DeltaRandom(70f, 75f, 80f, 85f, 45f, 50f);
            //deltaMn = (float)Math.Round(UnityEngine.Random.Range(70f, 75f), 2);
            //deltaP = (float)Math.Round(UnityEngine.Random.Range(80f, 85f), 2);
            //deltaS = (float)Math.Round(UnityEngine.Random.Range(45f, 50f), 2);
        }
        else if (steelC > 0.1 && steelC < 0.25)
        {
            DeltaRandom(75f, 80f, 85f, 90f, 40f, 45f);
            //deltaMn = (float)Math.Round(UnityEngine.Random.Range(75f, 80f), 2);
            //deltaP = (float)Math.Round(UnityEngine.Random.Range(85f, 90f), 2);
            //deltaS = (float)Math.Round(UnityEngine.Random.Range(40f, 45f), 2);
        }
        else
        {
            DeltaRandom(80f, 95f, 90f, 95f, 35f, 40f);
            //deltaMn = (float)Math.Round(UnityEngine.Random.Range(80f, 95f), 2);
            //deltaP = (float)Math.Round(UnityEngine.Random.Range(90f, 95f), 2);
            //deltaS = (float)Math.Round(UnityEngine.Random.Range(35f, 40f), 2);
        }

        ResidualConcentration[0] = steelC;
        ResidualConcentration[1] = ShikhtaParameters[1] * (100 - deltaMn) * 0.01f;
        ResidualConcentration[2] = 0;
        ResidualConcentration[3] = ShikhtaParameters[3] * (100 - deltaP) * 0.01f;
        ResidualConcentration[4] = ShikhtaParameters[4] * (100 - deltaS) * 0.01f;

        for (int i = 0; i < 5; i++)
        {
            GParameters[i] = ShikhtaParameters[i] - ResidualConcentration[i];
        }

        tmpG = (float)Math.Round(UnityEngine.Random.Range(0.80f, 0.95f), 2);
        GCO = tmpG * GParameters[0] * (0.5f * 32) / 12;
        GCO2 = (1 - tmpG) * GParameters[0] * 32 / 12;
        GMnO2 = GParameters[1] * 32 / 55;
        GSiO2 = GParameters[2] * 32 / 28;
        GPO2 = GParameters[3] * (32 * 5) / (31 * 4);

        //Об'ємні витрати кисню
        VO2_CO = GCO * 22.4f / 32;
        VO2_CO2 = GCO2 * 22.4f / 32;
        VO2_SiO2 = GSiO2 * 22.4f / 32;
        VO2_MnO2 = GMnO2 * 22.4f / 32;
        VO2_PO2 = GPO2 * 22.4f / 32;

        GC_CO = tmpG * GParameters[0] * 28 / 12;
        GC_CO2 = (1 - tmpG) * GParameters[0] * 44 / 12;
        GMn_MnO = GParameters[1] * 72 / 55;
        GSi_SiO2 = GParameters[2] * 60 / 28;
        GP_P2O5 = GParameters[3] * 2 * 142 / (4 * 31);
        GS_CaS = 0;
        kol_oxydov = GC_CO + GC_CO2 + GSi_SiO2 + GMn_MnO + GP_P2O5 + GS_CaS;

        for(int i = 0; i < 5; i++) 
        {
            MetalParameters[i] = ShikhtaParameters[i]; 
            StartMetalParameters[i] = MetalParameters[i];
        }

        SlagCalculation();
        FillingTables();
    }

    public void StartSmelting()
    {
        MessageBox.SetActive(false);
        DataHolder.furmaSet = -1;
        StartCoroutine("DynamicModel");
    }

    public void StartNewSmelting() 
    {
        SceneManager.LoadScene(0);
    }

    public void Exit() 
    {
        Application.Quit();
    }

    private void SetSteps(bool set) 
    {
        for (int i = 0; i < DataHolder.Steps.Length; i++)
            DataHolder.Steps[i] = set;
    }

    private void DeltaRandom(float dMnMin, float dMnMax, float dPMin, float dPMax, float dSMin, float dSMax) 
    {
        deltaMn = (float)Math.Round(UnityEngine.Random.Range(dMnMin, dMnMax), 2);
        deltaP = (float)Math.Round(UnityEngine.Random.Range(dPMin, dPMax), 2);
        deltaS = (float)Math.Round(UnityEngine.Random.Range(dSMin, dSMax), 2);
    }

    private void RandomSlagIron() 
    {
        SlagParameters[7, 8] = 100;

        if (steelC < 0.1f)
        {
            SlagParameters[4, 8] = UnityEngine.Random.Range(20f, 30f);
            SlagParameters[5, 8] = UnityEngine.Random.Range(6f, 12f);
        }

        if (steelC > 0.1f && steelC < 0.25f)
        {
            SlagParameters[4, 8] = UnityEngine.Random.Range(15f, 20f);
            SlagParameters[5, 8] = UnityEngine.Random.Range(4f, 6f);
        }

        if (steelC > 0.25f)
        {
            SlagParameters[4, 8] = UnityEngine.Random.Range(10f, 15f);
            SlagParameters[5, 8] = UnityEngine.Random.Range(3f, 5f);
        }

        SlagParameters[6, 8] = SlagParameters[4, 8] + SlagParameters[5, 8];
        SlagParameters[3, 8] = 100 - SlagParameters[6, 8];

        for (int i = 0; i < 3; i++)
        {
            SlagParameters[i, 8] = SlagParameters[i, 7] * SlagParameters[3, 8] / SlagParameters[3, 7];
        }
    }

    private void RandomMeterials()
    {
        SlagGMaterials = new double[PARAMETERS_COUNT];
        MaterialsParameters = new double[PARAMETERS_COUNT, 7];

        Fe_TO = (int)UnityEngine.Random.Range(59, 65);

        //Витрати на плавку
        SlagGMaterials[1] = (float)UnityEngine.Random.Range(0.1f, 0.4f);
        SlagGMaterials[2] = (float)UnityEngine.Random.Range(0f, 1.5f);
        SlagGMaterials[3] = (float)UnityEngine.Random.Range(0.2f, 1f);
        SlagGMaterials[4] = (float)UnityEngine.Random.Range(0.2f, 2f);

        //Вміст вапна
        MaterialsParameters[0, 0] = UnityEngine.Random.Range(80f, 92f);
        MaterialsParameters[0, 1] = UnityEngine.Random.Range(1f, 5f);
        var tmp = MaterialsParameters[0, 0] + MaterialsParameters[0, 1];
        MaterialsParameters[0, 4] = 100 - tmp > 10 ? UnityEngine.Random.Range(0f, 10f) : UnityEngine.Random.Range(0, 100 - (float)tmp);
        MaterialsParameters[0, 5] = 100 - tmp - MaterialsParameters[0, 4];

        //Вміст плавікового шпату
        MaterialsParameters[1, 0] = UnityEngine.Random.Range(0f, 5f);
        MaterialsParameters[1, 1] = UnityEngine.Random.Range(3f, 20f);
        MaterialsParameters[1, 5] = 100 - (MaterialsParameters[1, 0] + MaterialsParameters[1, 1]);

        //Вміст твердого окислювача
        MaterialsParameters[2, 0] = UnityEngine.Random.Range(1f, 14f);
        MaterialsParameters[2, 1] = UnityEngine.Random.Range(4f, 12f);
        MaterialsParameters[2, 3] = UnityEngine.Random.Range(1f, 18f);
        tmp = MaterialsParameters[2, 0] + MaterialsParameters[2, 1] + MaterialsParameters[2, 3];
        MaterialsParameters[2, 2] = (Fe_TO - MaterialsParameters[2, 3] * (56 / 72)) * (160 / 112);
        MaterialsParameters[2, 4] = 0;
        MaterialsParameters[2, 5] = 100 - tmp - MaterialsParameters[2, 2];

        //Вміст футерування конвертеру
        MaterialsParameters[3, 0] = UnityEngine.Random.Range(15f, 65f);
        MaterialsParameters[3, 1] = UnityEngine.Random.Range(1f, 5f);
        MaterialsParameters[3, 2] = UnityEngine.Random.Range(1f, 2f);
        MaterialsParameters[3, 3] = 0;
        MaterialsParameters[3, 4] = UnityEngine.Random.Range(0f, 20f);

        tmp = 0;
        for (int i = 0; i < 5; i++)
            tmp += MaterialsParameters[3, i];

        MaterialsParameters[3, 5] = 100 - tmp;

        //Вміст міксерного шлаку
        MaterialsParameters[4, 0] = UnityEngine.Random.Range(25f, 35f);
        MaterialsParameters[4, 1] = UnityEngine.Random.Range(30f, 40f);
        MaterialsParameters[4, 2] = UnityEngine.Random.Range(0f, 1.5f);
        MaterialsParameters[4, 3] = UnityEngine.Random.Range(5f, 7f);

        tmp = 0;
        for (int i = 0; i < PARAMETERS_COUNT; i++)
        {
            tmp += MaterialsParameters[4, i];
            MaterialsParameters[i, 6] = 100;
        }

        MaterialsParameters[4, 5] = 100 - tmp;
    }

    private void SlagCalculation() 
    {
        SlagParameters = new double[9, 9];

        SlagParameters[1, 0] = GSiO2;
        SlagParameters[2, 0] = GMnO2 + GPO2;
        SlagParameters[3, 0] = SlagParameters[1, 0] + SlagParameters[2, 0];
        SlagParameters[7, 0] = SlagParameters[3, 0];

        for (int i = 1; i < 5; i++)
        {
            SlagParameters[0, i] = MaterialsParameters[i, 0] / 100 * SlagGMaterials[i];
            SlagParameters[1, i] = MaterialsParameters[i, 1] / 100 * SlagGMaterials[i];
            SlagParameters[2, i] = MaterialsParameters[i, 5] / 100 * SlagGMaterials[i];
            SlagParameters[3, i] = SlagParameters[0, i] + SlagParameters[1, i] + SlagParameters[2, i];
            SlagParameters[4, i] = MaterialsParameters[i, 3] / 100 * SlagGMaterials[i];
            SlagParameters[5, i] = MaterialsParameters[i, 2] / 100 * SlagGMaterials[i];
            SlagParameters[6, i] = MaterialsParameters[i, 2] / 100 * SlagGMaterials[i] + MaterialsParameters[i, 3] / 100 * SlagGMaterials[i];
            SlagParameters[7, i] = SlagParameters[3, i] + SlagParameters[6, i];
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                SlagParameters[i, 5] += SlagParameters[i, j];
            }
        }

        SlagGMaterials[0] = (100 * (slagB * (SlagParameters[1, 5] - SlagParameters[0, 5]))) / (MaterialsParameters[0, 0] - slagB * MaterialsParameters[0, 1]);
        SlagParameters[0, 6] = MaterialsParameters[0, 0] / 100 * SlagGMaterials[0];
        SlagParameters[1, 6] = MaterialsParameters[0, 1] / 100 * SlagGMaterials[0];
        SlagParameters[2, 6] = MaterialsParameters[0, 5] / 100 * SlagGMaterials[0];
        SlagParameters[3, 6] = SlagParameters[0, 6] + SlagParameters[1, 6] + SlagParameters[2, 6];
        SlagParameters[4, 6] = MaterialsParameters[0, 3] / 100 * SlagGMaterials[0];
        SlagParameters[5, 6] = MaterialsParameters[0, 2] / 100 * SlagGMaterials[0];
        SlagParameters[6, 6] = MaterialsParameters[0, 2] / 100 * SlagGMaterials[0] + MaterialsParameters[0, 3] / 100 * SlagGMaterials[0];
        SlagParameters[7, 6] = SlagParameters[3, 6] + SlagParameters[6, 6];

        for (int i = 0; i < 8; i++)
        {
            for (int j = 5; j < 7; j++)
            {
                SlagParameters[i, 7] += SlagParameters[i, j];
            }
        }

        RandomSlagIron();
        SlagParameters[7, 7] = SlagParameters[3, 7] * 100 / (100 - SlagParameters[4, 8] - SlagParameters[5, 8]);
    }

    private void Table1Filling() 
    {
        for (int i = 0; i < 5; i++)
        {
            Table1[i + 1].text = Convert.ToString(Math.Round(IronGParameters[i], 3));
            Table1[i + 7].text = Convert.ToString(Math.Round(ScrapGParameters[i], 3));
            Table1[i + 13].text = Convert.ToString(Math.Round(ShikhtaParameters[i], 3));
        }

        Table1[0].text = Convert.ToString(Math.Round(ironG, 3));
        Table1[6].text = Convert.ToString(Math.Round(scrapG, 3));
        Table1[12].text = Convert.ToString(Math.Round(ironG + scrapG, 3));
    }

    private void Table2Filling() 
    {
        Table2[0].text = Convert.ToString(Math.Round(ResidualConcentration[0], 3));
        Table2[1].text = "--"; 
        Table2[2].text = "--";
        Table2[3].text = Convert.ToString(Math.Round(ResidualConcentration[2], 3));
        Table2[4].text = Convert.ToString(Math.Round(ResidualConcentration[1], 3));
        Table2[5].text = Convert.ToString(Math.Round(ResidualConcentration[3], 3));
        Table2[6].text = Convert.ToString(Math.Round(ResidualConcentration[4], 3));
        Table2[7].text = "--";
        Table2[8].text = Convert.ToString(Math.Round(GParameters[0], 3));
        Table2[9].text = Convert.ToString(Math.Round(tmpG * GParameters[0], 3));
        Table2[10].text = Convert.ToString(Math.Round((1 - tmpG) * GParameters[0], 3));
        Table2[11].text = Convert.ToString(Math.Round(GParameters[2], 3));
        Table2[12].text = Convert.ToString(Math.Round(GParameters[1], 3));
        Table2[13].text = Convert.ToString(Math.Round(GParameters[3], 3));
        Table2[14].text = Convert.ToString(Math.Round(GParameters[4], 3));
        Table2[15].text = Convert.ToString(Math.Round(GParameters[0] + GParameters[1] + GParameters[2] + GParameters[3] + GParameters[4], 3));
        Table2[16].text = Convert.ToString(Math.Round(GCO + GCO2, 3));
        Table2[17].text = Convert.ToString(Math.Round(GCO, 3));
        Table2[18].text = Convert.ToString(Math.Round(GCO2, 3));
        Table2[19].text = Convert.ToString(Math.Round(GSiO2, 3));
        Table2[20].text = Convert.ToString(Math.Round(GMnO2, 3));
        Table2[21].text = Convert.ToString(Math.Round(GPO2, 3));
        Table2[22].text = "--";
        Table2[23].text = Convert.ToString(Math.Round(GCO + GCO2 + GSiO2 + GMnO2 + GPO2, 3));
        Table2[24].text = Convert.ToString(Math.Round(VO2_CO + VO2_CO2, 3));
        Table2[25].text = Convert.ToString(Math.Round(VO2_CO, 3));
        Table2[26].text = Convert.ToString(Math.Round(VO2_CO2, 3));
        Table2[27].text = Convert.ToString(Math.Round(VO2_SiO2, 3));
        Table2[28].text = Convert.ToString(Math.Round(VO2_MnO2, 3));
        Table2[29].text = Convert.ToString(Math.Round(VO2_PO2, 3));
        Table2[30].text = "--";
        Table2[31].text = Convert.ToString(Math.Round(VO2_CO + VO2_CO2 + VO2_SiO2 + VO2_MnO2 + VO2_PO2, 3));
        Table2[32].text = "--";
        Table2[33].text = Convert.ToString(Math.Round(GC_CO, 3));
        Table2[34].text = Convert.ToString(Math.Round(GC_CO2, 3));
        Table2[35].text = Convert.ToString(Math.Round(GSi_SiO2, 3));
        Table2[36].text = Convert.ToString(Math.Round(GMn_MnO, 3));
        Table2[37].text = Convert.ToString(Math.Round(GP_P2O5, 3));
        Table2[38].text = Convert.ToString(Math.Round(GS_CaS, 3));
        Table2[39].text = Convert.ToString(Math.Round(kol_oxydov, 3));
    }

    private void Table3Filling() 
    {
        int k = 1;

        for(int i = 0; i < 5; i++) 
        {
            Table3[k - 1].text = $"{Math.Round(SlagGMaterials[i], 2)}";

            for (int j = 0; j < 7; j++) 
            {
                Table3[j + k].text = MaterialsParameters[i, j] == 0 ? $"--" : $"{Math.Round(MaterialsParameters[i, j], 2)}";
            }

            k += 8;
        }
    }

    private void Table4Filling() 
    {
        int k = 0;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Table4[j + k].text = SlagParameters[i, j] == 0 ? $"--" : $"{Math.Round(SlagParameters[i, j], 3)}";
                
                if(j == 8)
                {
                    Table4[j + k].text = SlagParameters[i, j] == 0 ? $"--" : $"{Math.Round(SlagParameters[i, j], 2)}";
                }
            }

            k += 9;
        }
    }

    private void FillingTables()
    {
        Table1Filling();
        Table2Filling();
        Table3Filling();
        Table4Filling();
        DataHolder.Steps[4] = true;
    }

    private void SteelTemperature(float C, ref float T)
    {
        T = C > 0 ? 1539 - 80 * C + 80 : 0;

        if (DataHolder.smelting)
            MetalParametersPlaceholders[5].text = $"{(int)Math.Round(T, 3)}";
    }

    private void CarbonOxidation(ref float С, float Si, float T, float speed) 
    {
        iC = (float)((6.94f * Math.Pow(10, 6)) * P * (GetNC(StartMetalParameters[0]) + 273 / T) * Math.Exp(-22236 / T)) / (R * T);

        if (Si == 0 && С - steelC < 0.1f)
        {
            V = V > 0.1 ? V -= 0.01f : 0.05f;
        }
        else if (T > 1350)
        {
            V += 0.0001f;
        }
        else
        {
            V = 0.047f;
        }

        var dC = V * iC * speed;
        С -= dC;

        if (С - steelC < 0.01f)
        {
            DataHolder.smelting = false;
            DataHolder.release = true;
            DataHolder.furmaSet = 1;
            StartCoroutine(SmeltingEnd());
        }
    }

    private void ManganeseOxidation(ref float Mn, float steelMn, float speed) 
    {
        Mn = Mn > steelMn ? (float)(StartMetalParameters[1] * Math.Pow(j, -0.17f) * speed) : Mn -= (Mn / 100000);
    }

    private void SiliconOxidation(ref float Si, float speed) 
    {
        if (Si > 0.3f)
        {
            Si = (float)(Si - 0.417 * Math.Pow(Si, 0.4f) * intensity * (28 / 22.4f) * speed);
        }
        else
        {
            Si = (float)(Si - 0.525 * Math.Pow(Si, 0.6f) * intensity * (28 / 22.4f) * speed);
        }

        Si = Si > 0 ? Si : 0;
    }

    private void PhosphorusOxidation(ref float P, float steelP, float speed) 
    {
        var dP = (float)(4.1 * V * iC * SlagParameters[7, 7] * P * speed);
        P = P > steelP ? P -= dP : P -= (dP/100);
    }

    private void SulfurOxidation(ref float S, float steelS, float speed) 
    {
        var dS = (float)(0.005 * (1 - Math.Exp(-1.8716)) * speed);
        S = S > steelS? S -= dS : S -= (dS/100);
    }

    private float GetNC(float C)
    {
        float D = 451584 + 4 * 816 * C;
        return (float)(672 + Math.Sqrt(D)) / 1632 * 100;
    }

    private IEnumerator DynamicModel()
    {
        while (true)
        {
            if (steelC < MetalParameters[0] && DataHolder.smelting)
            {
                j++;

                SteelTemperature(MetalParameters[0], ref MetalParameters[5]);
                CarbonOxidation(ref MetalParameters[0], MetalParameters[2], MetalParameters[5], 0.01f);
                ManganeseOxidation(ref MetalParameters[1], ResidualConcentration[1], 1f);
                SiliconOxidation(ref MetalParameters[2], 0.001f);
                PhosphorusOxidation(ref MetalParameters[3], ResidualConcentration[3], 0.001f);
                SulfurOxidation(ref MetalParameters[4], ResidualConcentration[4], 0.002f);

                if (!DataHolder.smelting)
                    yield return new WaitForSeconds(2);

                ExhaustGases.SetActive(DataHolder.smelting);

                Time.timeScale = timeSpeed.value;
                timeSpeedText.text = $"{Math.Round(timeSpeed.value,1)}x";

                for (int i = 0; i < PARAMETERS_COUNT; i++)
                {
                    MetalParametersPlaceholders[i].text = $"{Math.Round(MetalParameters[i], 3)}";
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ButtonsStatus() 
    {
        while (true)
        {
            ScrapRandom.interactable = DataHolder.scrapLoaded && !DataHolder.smelting? true : false;
            IronDefault.interactable = DataHolder.ironPoured && !DataHolder.smelting ? true : false;
            IronApplyData.interactable = DataHolder.ironPoured && !DataHolder.smelting ? true : false;
            SteelTCount.interactable = DataHolder.scrapLoaded && DataHolder.ironPoured && !DataHolder.smelting ? true : false;
            SlagBDefault.interactable = SteelTCount.interactable;
            SlagBApplyData.interactable = SteelTCount.interactable;
            CountMetShikhta.interactable = DataHolder.Steps[0] == true && DataHolder.Steps[1] == true && DataHolder.Steps[2] == true && DataHolder.Steps[3] == true && !DataHolder.smelting ? true : false;
            Smelting.interactable = DataHolder.Steps[4] && DataHolder.converterTurn == 0 && !DataHolder.smelting ? true : false;

            if (DataHolder.smelting)
                break;

            yield return null;
        }
    }

    private IEnumerator SmeltingEnd() 
    {
        while (true)
        {
            if (DataHolder.steelPoured && DataHolder.slagPoured)
            {
                MessageBox.SetActive(true);
                MessageBoxText.text = "Плавка пройшла успішно!\n Якщо треба провести ще одну, натисніть 'Нова плавка'";
                break;
            }

            yield return null;
        }
    }
}
