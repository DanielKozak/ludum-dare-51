using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeScaleAgent : MonoBehaviour
{

    float currentTimeScale = 1f;

    float masterTimeScale;


    // Start is called before the first frame update
    void Start()
    {
        currentTimeScale = Time.timeScale;
    }

    public void SetTimeScale(float timeScale)
    {
        currentTimeScale = Mathf.Min(masterTimeScale, timeScale);
    }

    void Update()
    {
        masterTimeScale = Time.timeScale;
        currentTimeScale = Mathf.Min(masterTimeScale, TimeController.Instance.GetCurrentAttenuation(transform));

    }

    public float GetCurrentTimeScale() => currentTimeScale;
}
