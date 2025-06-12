using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance;

    public GameObject carrotPrefab;
    public GameObject applePrefab;
    public GameObject cornPrefab;

    public Transform[] spawnPoints;

    void Awake()
    {
        Instance = this;
        Debug.Log("✅ FoodSpawner 인스턴스 생성됨");
    }

    void Start()
    {
        Debug.Log("🍠 carrotPrefab 연결됨: " + (carrotPrefab != null));
        Debug.Log("🍎 applePrefab 연결됨: " + (applePrefab != null));
        Debug.Log("🌽 cornPrefab 연결됨: " + (cornPrefab != null));

        SpawnFoods();
    }

    void SpawnFoods()
    {
        if (spawnPoints.Length < 15)
        {
            Debug.LogError("❌ 스폰 포인트가 15개보다 적습니다!");
            return;
        }

        List<GameObject> foodPrefabs = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            foodPrefabs.Add(carrotPrefab);
            foodPrefabs.Add(applePrefab);
            foodPrefabs.Add(cornPrefab);
        }

        Shuffle(spawnPoints);
        Shuffle(foodPrefabs);

        for (int i = 0; i < 15; i++)
        {
            Instantiate(foodPrefabs[i], spawnPoints[i].position, Quaternion.identity);
        }
    }

    public GameObject GetRandomFoodPrefab()
    {
        int rand = Random.Range(0, 3);
        if (rand == 0) return carrotPrefab;
        if (rand == 1) return applePrefab;
        return cornPrefab;
    }

    void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }

    // ✅ 포함된 리스폰 기능
    public void Respawn(Vector3 position, float delay = 5f)
    {
        StartCoroutine(RespawnRoutine(position, delay));
    }

    private IEnumerator RespawnRoutine(Vector3 position, float delay)
    {
        Debug.Log("⏳ 리스폰 타이머 시작 (" + delay + "초)");

        yield return new WaitForSeconds(delay);

        Debug.Log("🧭 리스폰 로직 진입");

        GameObject prefab = GetRandomFoodPrefab();
        if (prefab != null)
        {
            // ✅ 고정값 0.5 제거 → 원래 위치 그대로 사용
            Vector3 respawnPosition = position;

            GameObject newFood = Instantiate(prefab, respawnPosition, Quaternion.identity);
            Debug.Log("🍏 새 먹이 생성 완료: " + newFood.name + " 위치: " + respawnPosition);
        }
        else
        {
            Debug.LogWarning("⚠️ 랜덤 프리팹이 null입니다!");
        }
    }

    public void RespawnSpecific(GameObject prefab, Vector3 position, float delay = 5f)
    {
        StartCoroutine(RespawnSpecificRoutine(prefab, position, delay));
    }

    private IEnumerator RespawnSpecificRoutine(GameObject prefab, Vector3 position, float delay)
    {
        Debug.Log("⏳ 아이템 리스폰 타이머 시작 (" + delay + "초)");

        yield return new WaitForSeconds(delay);

        if (prefab != null)
        {
            Vector3 fixedPosition = position; // Y 고정하지 않음
            GameObject newObj = Instantiate(prefab, fixedPosition, Quaternion.identity);
            Debug.Log("✨ 아이템 리스폰 완료: " + newObj.name + " at " + fixedPosition);
        }
    }

}
