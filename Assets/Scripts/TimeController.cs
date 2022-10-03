using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{

    List<TimeScaleAgent> agents;
    List<Transform> influencers;

    public float MaxAttenuationDistance = 7.5f;
    public float AttenuationFallof = 1f;

    public void ResetState()
    {
        agents = new List<TimeScaleAgent>();
        influencers = new List<Transform>();
    }

    public bool RegisterAgent(TimeScaleAgent agent)
    {
        if (!agents.Contains(agent)) agents.Add(agent);
        else return false;
        return true;
    }
    public bool UnRegisterAgent(TimeScaleAgent agent)
    {
        if (agents.Contains(agent)) agents.Remove(agent);
        else return false;
        return true;
    }
    public bool RegisterInfluence(Transform influence)
    {
        if (!influencers.Contains(influence)) influencers.Add(influence);
        else return false;
        return true;
    }
    public bool UnRegisterInfluence(Transform influence)
    {
        if (influencers.Contains(influence)) influencers.Remove(influence);
        else return false;
        return true;
    }

    public float GetCurrentAttenuation(Transform transform)
    {
        float minDistance = float.MaxValue;
        foreach (var influncer in influencers)
        {
            float dist = Vector3.Distance(transform.position, influncer.position);

            if (dist < MaxAttenuationDistance) minDistance = dist;
        }
        if (minDistance > MaxAttenuationDistance) return 1f;
        return minDistance / MaxAttenuationDistance;
    }


    // Start is called before the first frame update
    void Awake()
    {
        ResetState();
    }
}


