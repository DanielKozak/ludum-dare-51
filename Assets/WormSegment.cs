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
    public WormHead HeadReference;

    float distanceSpeedModifier;

    public Vector3 deathDisplaceDirection;
    Quaternion deathRotation;

    SpriteRenderer mRenderer;

    void Start()
    {
        deathDisplaceDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        deathRotation = Random.rotation;
        // Debug.Log(transform.position + deathDisplaceDirection);
        mRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        if (isHead)
        {
            if (HeadReference.isDead)
            {
                mRenderer.color = Color.black;
                // Debug.Log(transform.position + deathDisplaceDirection);

                transform.position = Vector3.Lerp(transform.position, transform.position + deathDisplaceDirection, Time.deltaTime); //TODO Fix
                // transform.rotation = Quaternion.Lerp(transform.rotation, deathRotation, Time.deltaTime);
            }
            else
            {
                return;
            }
        }

        if (HeadReference.isDead)
        {
            mRenderer.color = Color.black;

            transform.position = Vector3.Lerp(transform.position, transform.position + deathDisplaceDirection, Time.deltaTime); //TODO Fix
            // transform.rotation = Quaternion.Lerp(transform.rotation, deathRotation, Time.deltaTime);
        }
        else
        {
            float dist = Vector3.Distance(transform.position, PreviousSegment.transform.position);
            if (dist > wormDisplacementDistance)
            {
                distanceSpeedModifier = dist / wormDisplacementDistance;
                transform.position = Vector3.Lerp(transform.position, PreviousSegment.transform.position, Time.deltaTime * wormSpeed * distanceSpeedModifier * 3f);
                // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.up, PreviousSegment.transform.position - transform.position), 360f);
                transform.up = PreviousSegment.transform.position - transform.position;
            }
        }

    }


    public void RecieveHit()
    {

    }
}
