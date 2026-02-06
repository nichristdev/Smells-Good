using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeFallUI : MonoBehaviour
{
    [SerializeField] Slider HealthUI;
    [SerializeField] Slider ShieldUI;
    bool UsingShield;
    [SerializeField] Slider SpeedBoostUI;
    bool UsingSpeedBoost;

    private void Start()
    {
        ShieldUI.gameObject.SetActive(false);
        SpeedBoostUI.gameObject.SetActive(false);
    }

    public void UpdateHealthSlider(float CurrentValue, float MinValue, float MaxValue)
    {
        HealthUI.value = CurrentValue;
        HealthUI.minValue = MinValue;
        HealthUI.maxValue = MaxValue;
    }

    public void ToogleShield(bool OnOrOff, float Duration)
    {
        if(OnOrOff)
        {
            ShieldUI.gameObject.SetActive(true);
            ShieldUI.maxValue = Duration;
            ShieldUI.value = ShieldUI.maxValue;
            UsingShield = true;
        }
        else
        {
            ShieldUI.gameObject.SetActive(false);
            UsingShield = false;
        }
    }

    public void ToogleSpeedBoost(bool OnOrOff, float Duration)
    {
        if (OnOrOff)
        {
            SpeedBoostUI.gameObject.SetActive(true);
            SpeedBoostUI.maxValue = Duration;
            SpeedBoostUI.value = SpeedBoostUI.maxValue;
            UsingSpeedBoost = true;
        }
        else
        {
            SpeedBoostUI.gameObject.SetActive(false);
            UsingSpeedBoost = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(UsingShield)
        {
            ShieldUI.value -= Time.deltaTime;
        }

        if (UsingSpeedBoost)
        {
            SpeedBoostUI.value -= Time.deltaTime;
        }
    }
}
