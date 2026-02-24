using UnityEngine;
using System.Collections.Generic;

public class VFXManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnTarget
    {
        public float startBeat;
        public float endBeat;
        public Vector3 spawnPoint;
        public int prefabIndex;
        [HideInInspector]public bool hasSpawned;
        [HideInInspector]public GameObject spawnedObject;
    }

    public GameObject[] spawnedVFX;
    public List<SpawnTarget> spawnQueue = new List<SpawnTarget>();

    private Conductor conductor;

    void Start()
    {
        conductor = FindObjectOfType<Conductor>();
    }

    void LateUpdate()
    {
        if (conductor == null) return;

        float currentBeat = conductor.songPositionInBeats;

        foreach (var target in spawnQueue)
        {
            // Spawn
            if (!target.hasSpawned && currentBeat >= target.startBeat)
            {
                if (target.prefabIndex >= 0 && target.prefabIndex < spawnedVFX.Length)
                {
                    target.spawnedObject = Instantiate(
                        spawnedVFX[target.prefabIndex],
                        target.spawnPoint,
                        Quaternion.identity
                    );

                    target.hasSpawned = true;
                }
                else
                {
                    Debug.LogWarning("Invalid prefab index!");
                }
            }

            // Despawn
            if (target.hasSpawned && currentBeat >= target.endBeat)
            {
                Destroy(target.spawnedObject);
            }
        }
    }

    public void AddVFXSpawn(float startBeat, float endBeat, Vector3 spawn, int prefabIndex)
    {
        spawnQueue.Add(new SpawnTarget
        {
            startBeat = startBeat,
            endBeat = endBeat,
            spawnPoint = spawn,
            prefabIndex = prefabIndex,
            hasSpawned = false
        });
    }
}
