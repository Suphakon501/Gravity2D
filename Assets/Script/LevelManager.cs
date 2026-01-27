using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Map Settings")]
    public List<GameObject> levelPrefabs;
    public Transform player;
    public float mapLength = 126.75f;
    public float fixedY = 0f;
    public float triggerOffset = 50f;

    private List<GameObject> activeChunks = new List<GameObject>();
    private float nextSpawnX = 0f;

    void Awake()
    {
        // ❌ ห้าม DontDestroyOnLoad กับ LevelManager
        Instance = this;
    }

    void Start()
    {
        ResetLevel();
    }

    public void ResetLevel()
    {
        // 🔥 ลบแมพเก่าทั้งหมด
        foreach (GameObject chunk in activeChunks)
        {
            Destroy(chunk);
        }

        activeChunks.Clear();
        nextSpawnX = 0f;

        // 🔁 สร้างแมพใหม่
        for (int i = 0; i < 3; i++)
        {
            SpawnLevel();
        }
    }

    void Update()
    {
        if (player == null) return;

        if (player.position.x > nextSpawnX - (mapLength + triggerOffset))
        {
            SpawnLevel();
        }

        if (activeChunks.Count > 0)
        {
            float firstChunkEndX =
                activeChunks[0].transform.position.x + mapLength;

            if (player.position.x > firstChunkEndX + 30f)
            {
                RemoveOldLevel();
            }
        }
    }

    void SpawnLevel()
    {
        int randomIndex = Random.Range(0, levelPrefabs.Count);
        Vector3 spawnPos = new Vector3(nextSpawnX, fixedY, 0);

        GameObject chunk = Instantiate(
            levelPrefabs[randomIndex],
            spawnPos,
            Quaternion.identity
        );

        activeChunks.Add(chunk);
        nextSpawnX += mapLength;
    }

    void RemoveOldLevel()
    {
        if (activeChunks.Count == 0) return;

        GameObject oldChunk = activeChunks[0];
        activeChunks.RemoveAt(0);
        Destroy(oldChunk);
    }
}
