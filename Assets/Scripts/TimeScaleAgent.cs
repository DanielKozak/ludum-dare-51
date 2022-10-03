using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeScaleAgent : MonoBehaviour
{

    float currentTimeScale = 1f;

    float masterTimeScale;
    TMP_Text debugLabel;


    // Start is called before the first frame update
    void Start()
    {
        currentTimeScale = Time.timeScale;
        debugLabel = GetComponentInChildren<TMP_Text>();
    }

    public void SetTimeScale(float timeScale)
    {
        currentTimeScale = Mathf.Min(masterTimeScale, timeScale);
    }

    void Update()
    {
        masterTimeScale = Time.timeScale;
        currentTimeScale = Mathf.Min(masterTimeScale, TimeController.Instance.GetCurrentAttenuation(transform));


        debugLabel.text = $"{currentTimeScale} /n {Time.deltaTime / currentTimeScale}";
    }

    public float GetCurrentTimeScale() => currentTimeScale;
}
