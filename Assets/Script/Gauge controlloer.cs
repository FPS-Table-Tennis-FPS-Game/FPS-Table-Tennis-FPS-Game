using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gaugecontroller : MonoBehaviour
{
    [SerializeField]
    private Slider Powerbar;
    private float currentGauge;
    private float maxGauge;


    void Start()
    {
        currentGauge = 0;
        SetMaxPower(1);
    }
    
    public float SetGauge
    {
        get => currentGauge;
        set => currentGauge = Mathf.Clamp(value, 0, 100);
    }

    public void SetMaxPower(float maxPower)
    {
        Powerbar.maxValue = maxPower;
        Powerbar.value = currentGauge;
    }

    public void AddGauge(float addition)
    {
        float addedGauge = currentGauge + addition;
        SetGauge = addedGauge;
        Powerbar.value = SetGauge;
        
    }

    public void ResetGauge()
    {
        SetGauge = 0;
        Powerbar.value = SetGauge;
    }
    public float GetCurrentGauge()
    { 
        return currentGauge;
    }

}




