using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSpawner : Singleton<WormSpawner>
{
    public GameObject WormHeadPrefab;
    public GameObject WormSegmentPrefab;

    public GameObject WormContainer;

    List<GameObject> CurrentWorms;

    public List<Transform> WormSpawnPoints = new List<Transform>();

    bool isSpawnerRunning = false;

    private void Start()
    {
        ResetState();
    }

    public void SpawnWorm(Vector3 spawnPoint, int segmentCount)
    {
        var newWorm = new GameObject("worm");
        newWorm.transform.SetParent(WormContainer.transform);
        newWorm.transform.position = spawnPoint;

        WormSegment[] segments = new WormSegment[segmentCount + 1];

        CurrentWorms.Add(newWorm);

        var head = Instantiate(WormHeadPrefab, newWorm.transform);
        segments[0] = head.GetComponent<WormSegment>();
        segments[0].isHead = true;
        for (int i = 1; i < segmentCount; i++)
        {
            segments[i] = Instantiate(WormSegmentPrefab, newWorm.transform).GetComponent<WormSegment>();
            segments[i].PreviousSegment = segments[i - 1];
            segments[i].transform.position = new Vector3(spawnPoint.x, spawnPoint.y - (WormSegment.wormDisplacementDistance * i), 0);
        }

    }


    public void ResetState()
    {
        StopCoroutine(WormStateCheckerRoutine());
        StopCoroutine(WormSpawnerRoutine());
        while (WormContainer.transform.childCount > 0)
        {
            Destroy(WormContainer.transform.GetChild(0));
        }
        CurrentWorms = new List<GameObject>();
        wormCount = 0;
        preferredWormCount = 0;
        currentSpawnDelay = 10f;
        needWorm = false;
        timeLastWormSpawned = Time.realtimeSinceStartup;
        isSpawnerRunning = true;
        StartCoroutine(WormStateCheckerRoutine());
        StartCoroutine(WormSpawnerRoutine());

    }



    int wormCount;
    int preferredWormCount;
    float currentSpawnDelay;

    bool needWorm = false;

    float timeLastWormSpawned;

    void CheckWormPrerequisites()
    {
        int level = TreeController.Instance.GetUpgradeLevel();
        preferredWormCount = level * level;
        currentSpawnDelay = level == 0 ? 30 : 45 / level;
        wormCount = CurrentWorms.Count;

        float wormSpawnError = (float)preferredWormCount * 0.2f;
        if (preferredWormCount - wormCount > wormSpawnError)
        {
            needWorm = true;
        }
    }

    public IEnumerator WormSpawnerRoutine()
    {
        while (isSpawnerRunning)
        {
            yield return new WaitUntil(() => needWorm);
            while (Time.realtimeSinceStartup - timeLastWormSpawned < currentSpawnDelay)
            {
                yield return null;
            }
            timeLastWormSpawned = Time.realtimeSinceStartup;

            int spawnPointIndex = Random.Range(0, WormSpawnPoints.Count);
            SpawnWorm(WormSpawnPoints[spawnPointIndex].position, Random.Range(3, 8));
        }
    }
    public IEnumerator WormStateCheckerRoutine()
    {
        while (isSpawnerRunning)
        {
            yield return new WaitForSecondsRealtime(2f);
            CheckWormPrerequisites();
        }
    }
}
