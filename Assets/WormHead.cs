using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHead : MonoBehaviour
{

    Vector3 currentWormTarget;
    bool isTargetSet = false;
    float rotateSpeed = 0.3f;
    float wormSpeed = 10f;

    public bool isDead = false;

    void ProcessMovement()
    {
        if (!isTargetSet)
        {
            currentWormTarget = TimeFieldController.Instance.GetRandomWorldTarget();
            // Debug.Log($"worm {GetInstanceID()} chose target {currentWormTarget}");
            isTargetSet = true;
        }
        float dist = Vector3.Distance(transform.position, currentWormTarget);
        if (dist < 1f)
        {
            CheckWormCollisions();
            isTargetSet = false;
        }

        if (isTargetSet)
        {
            Vector3 dir = currentWormTarget - transform.position;
            transform.up = Vector3.RotateTowards(transform.up, dir, Time.deltaTime * 5f, 10f);
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.up, wormSpeed * Time.deltaTime);
        }
    }

    void CheckWormCollisions()
    {

    }

    void Update()
    {
        if (!isDead) ProcessMovement();
    }


}
