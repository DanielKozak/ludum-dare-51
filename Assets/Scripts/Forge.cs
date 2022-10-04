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
        StartCoroutine(BeerRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        DwarfAnimator.SetFloat("timeScale", timeAgent.GetCurrentTimeScale());

        if (!isWorking)
        {
            if (fuelCount >= 3)
            {
                isWorking = true;
                fuelCount -= 3;
                SetForgeLabelText(fuelCount);
                DwarfAnimator.SetBool("Working", true);
                currentTimer = timer;
                ProgressSlider.gameObject.SetActive(true);
                ProgressSlider.value = 0;
                canDrink = false;
            }
        }
        else
        {

            currentTimer -= Time.deltaTime * timeAgent.GetCurrentTimeScale();
            ProgressSlider.value = (timer - currentTimer);
            Debug.Log($"{currentTimer} {ProgressSlider.value}");

            if (currentTimer <= 0f)
            {
                canDrink = true;
                // UIController.Instance.TweenSecondsCrystal(transform.position);
                GameConroller.Instance.AddSecond();
                isWorking = false;
                DwarfAnimator.SetBool("Working", false);
                ProgressSlider.gameObject.SetActive(false);
                if (fuelCount == 0)
                    DwarfAnimator.SetBool("hasBlue", false);
            }
        }
    }


    void SetForgeLabelText(int count)
    {
        UpperLabel.text = $"{count}/3 = 1";
    }
    bool canDrink = true;
    IEnumerator BeerRoutine()
    {
        while (canDrink)
        {
            yield return new WaitForSeconds(2.0f);
            if (Random.Range(0f, 1f) < 0.2f)
                DwarfAnimator.SetTrigger("Beer");
        }
    }
}
