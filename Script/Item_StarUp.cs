using UnityEngine;

public class Item_StarUp : MonoBehaviour
{
    public string prefabName = "Star_Up";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 spawnPosition = transform.position;

            ApplyRandomEffectToAllCars();
            ApplyPlayerEffect(other.gameObject);

            // 자동 리스폰
            if (FoodSpawner.Instance != null)
            {
                GameObject prefab = Resources.Load<GameObject>(prefabName);
                if (prefab != null)
                {
                    FoodSpawner.Instance.RespawnSpecific(prefab, spawnPosition, 30f);
                }
                else
                {
                    Debug.LogWarning(" Resources에서 프리팹을 불러올 수 없습니다: " + prefabName);
                }
            }

            Destroy(gameObject); // 아이템 제거
        }
    }

    void ApplyRandomEffectToAllCars()
    {
        int eventIndex = Random.Range(0, 6); // 0~5까지 6개
        string message = "";

        foreach (CarController car in CarController.allCars)
        {
            if (car == null) continue;

            switch (eventIndex)
            {
                case 0:
                    message = "스피드 업!";
                    car.BoostSpeed(2f, 10f);
                    break;

                case 1:
                    message = "10초간 정지!";
                    car.StopTemporarily(10f);
                    break;

                case 2:
                    message = "역방향 주행!";
                    car.ReverseDirection();
                    break;

                case 3:
                    message = "배고픔 회복!";
                    RestorePlayerHunger();
                    break;

                case 4:
                    message = "무적 10초!";
                    GrantInvincibilityToPlayer();
                    break;

                case 5:
                    message = "10초동안 먹을수록 더 배고픔!";
                    EnableHungerPenaltyOnEat();
                    break;

            }
        }

        FindObjectOfType<ItemEffectDisplay>()?.ShowMessage(message);
    }

    void RestorePlayerHunger()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            HungerSystem hungerSystem = player.GetComponent<HungerSystem>();
            if (hungerSystem != null)
            {
                hungerSystem.IncreaseHunger(hungerSystem.maxHunger);
            }
        }
    }

    void GrantInvincibilityToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            HungerSystem hungerSystem = player.GetComponent<HungerSystem>();
            if (hungerSystem != null)
            {
                hungerSystem.ActivateTotalInvincibility(10f);
            }
        }
    }

    void EnableHungerPenaltyOnEat()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            HungerSystem hungerSystem = player.GetComponent<HungerSystem>();
            if (hungerSystem != null)
            {
                hungerSystem.EnableEatPenalty(10f); // 10초 패널티 적용
            }
        }
    }
}
