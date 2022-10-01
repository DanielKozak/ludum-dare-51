using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleInfluencer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TimeController.Instance.RegisterInfluence(transform);
    }

    void OnDestroy()
    {
        TimeController.Instance.UnRegisterInfluence(transform);
    }
}
