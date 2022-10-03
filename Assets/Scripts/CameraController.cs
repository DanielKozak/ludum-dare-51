using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mCam;

    void Start()
    {
        mCam = Camera.main;
    }

    Vector3 newPosition = new Vector3();
    public float CameraSpeed = 5f;
    void Update()
    {
        newPosition = new Vector3();
        Vector3 godPosition = mCam.WorldToViewportPoint(GodController.Instance.transform.position);
        if (godPosition.x < 0.2f)
        {
            newPosition.x = -1;
        }
        if (godPosition.x > 0.8f)
        {
            newPosition.x = 1;
        }
        if (godPosition.y < 0.2f)
        {
            newPosition.y = -1;
        }
        if (godPosition.y > 0.8f)
        {
            newPosition.y = +1;
        }

        transform.position = Vector3.Lerp(transform.position, transform.position + newPosition, Time.deltaTime * CameraSpeed);
    }
}
