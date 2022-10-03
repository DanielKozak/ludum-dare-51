using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Forge : MonoBehaviour
{
    public Animator DwarfAnimator;

    public TMP_Text UpperLabel;
    public Slider ProgressSlider;

    public TimeScaleAgent timeAgent;

    float timer = 10f;
    float currentTimer = 10f;
    bool isWorking = false;

    int fuelCount = 0;

    public void AddFuel()
    {
        fuelCount += 1;
        SetForgeLabelText(fuelCount);
    }

    void Start()
    {
        SetForgeLabelText(fuelCount);
    }

    // Update is called once per frame
    void Update()
    {
        DwarfAnimator.SetFloat("timeScale", timeAgent.GetCurrentTimeScale());

        if (!isWorking)
        {
            if (fuelCount >= 5)
            {
                isWorking = true;
                fuelCount -= 5;
                SetForgeLabelText(fuelCount);
                DwarfAnimator.SetBool("Working", true);
                currentTimer = timer;
                ProgressSlider.gameObject.SetActive(true);
                ProgressSlider.value = 0;
            }
        }
        else
        {
            currentTimer -= Time.deltaTime * timeAgent.GetCurrentTimeScale();
            ProgressSlider.value += timer - currentTimer;

            if (currentTimer <= 0f)
            {
                GameConroller.Instance.AddSecond();
                isWorking = false;
                DwarfAnimator.SetBool("Working", false);
                ProgressSlider.gameObject.SetActive(false);
            }
        }
    }


    void SetForgeLabelText(int count)
    {
        UpperLabel.text = $"{count}/5 = 1";
    }
}
