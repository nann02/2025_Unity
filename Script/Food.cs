using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("😋 플레이어가 먹이를 먹음: " + gameObject.name);

        GameObject player = other.gameObject;
        HungerSystem hungerSystem = player.GetComponent<HungerSystem>();

        if (hungerSystem != null)
        {
            string foodName = gameObject.name.ToLower();
            bool hungerIncreased = false;

            // ✅ 음식 효과 적용
            switch (CharacterSelectUI.selectedAnimalType)
            {
                case CharacterSelectUI.AnimalType.Goat:
                    if (foodName.Contains("carrot"))
                    {
                        hungerSystem.IncreaseHunger(20); // 내부에서 패널티도 적용됨
                        hungerIncreased = true;
                    }
                    break;

                case CharacterSelectUI.AnimalType.CreamSheep:
                    if (foodName.Contains("apple"))
                    {
                        hungerSystem.IncreaseHunger(20);
                        hungerIncreased = true;
                    }
                    break;

                case CharacterSelectUI.AnimalType.DarkSheep:
                    if (foodName.Contains("corn"))
                    {
                        hungerSystem.IncreaseHunger(20);
                        hungerIncreased = true;
                    }
                    break;
            }

            if (hungerIncreased)
            {
                ShowHeartAbove(player);
                FindObjectOfType<EatTracker>()?.AddEat();
            }

            Debug.Log(hungerIncreased ? "🍽️ 배고픔 20 회복됨!" : "😶 배고픔 변화 없음");
        }

        if (FoodSpawner.Instance != null)
        {
            FoodSpawner.Instance.Respawn(transform.position);
        }

        Destroy(gameObject);
    }

    private void ShowHeartAbove(GameObject target)
    {
        GameObject heartPrefab = Resources.Load<GameObject>("FloatingHeart");
        if (heartPrefab == null)
        {
            Debug.LogWarning("💔 Resources/FloatingHeart 프리팹을 찾을 수 없습니다.");
            return;
        }

        Vector3 spawnPos = target.transform.position + Vector3.up * 1.2f;
        GameObject heart = Instantiate(heartPrefab, spawnPos, Quaternion.identity);
        heart.transform.localScale = Vector3.one * 0.02f;

        Camera cam = Camera.main;
        if (cam != null)
        {
            heart.transform.LookAt(cam.transform);
            heart.transform.Rotate(0, 180f, 0);
        }

        Destroy(heart, 2f);
    }
}
