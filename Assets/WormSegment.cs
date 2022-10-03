using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSegment : MonoBehaviour
{
    public static float wormDisplacementDistance = 1.2f;
    public static float wormSpeed = 2;

    public WormSegment PreviousSegment;
    public WormSegment NextSegment;

    public bool isHead = false;

    float distanceSpeedModifier;


    void Start()
    {

    }

    void Update()
    {
        if (isHead) return;
        float dist = Vector3.Distance(transform.position, PreviousSegment.transform.position);
        if (dist > wormDisplacementDistance)
        {
            distanceSpeedModifier = dist / wormDisplacementDistance;
            transform.position = Vector3.Lerp(transform.position, PreviousSegment.transform.position, Time.deltaTime * wormSpeed * distanceSpeedModifier * 3f);
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.up, PreviousSegment.transform.position - transform.position), 360f);
            transform.up = PreviousSegment.transform.position - transform.position;
        }
    }


    public void RecieveHit()
    {

    }
}
