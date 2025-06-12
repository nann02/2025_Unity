using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HungerSystem : MonoBehaviour
{
    public int hunger = 60;
    public int maxHunger = 60;
    public int health = 3;

    private Image[] heartImages;
    private Image hungerFillImage;
    private Text hungerText;

    public float hungerDecreaseInterval = 1f;
    private bool isHungerZero = false;

    private bool isInvincible = false;
    private bool isTotallyInvincible = false;
    private bool eatPenaltyActive = false;

    private Renderer[] renderers;

    private GameOverManager gameOverManager;
    private EatTracker eatTracker;

    void Start()
    {
        heartImages = new Image[3];
        heartImages[0] = GameObject.Find("Heart").GetComponent<Image>();
        heartImages[1] = GameObject.Find("Heart (1)").GetComponent<Image>();
        heartImages[2] = GameObject.Find("Heart (2)").GetComponent<Image>();

        hungerFillImage = GameObject.Find("Hungerbar").GetComponent<Image>();
        hungerText = GameObject.Find("Text").GetComponent<Text>();

        renderers = GetComponentsInChildren<Renderer>();
        gameOverManager = FindObjectOfType<GameOverManager>();
        eatTracker = FindObjectOfType<EatTracker>();

        UpdateUI();
        StartCoroutine(HungerDecreaseRoutine());
    }

    IEnumerator HungerDecreaseRoutine()
    {
        while (health > 0)
        {
            yield return new WaitForSeconds(hungerDecreaseInterval);

            if (!isTotallyInvincible && hunger > 0)
            {
                hunger--;
                UpdateUI();
            }

            if (hunger <= 0 && !isHungerZero)
            {
                StartCoroutine(HungerZeroHealthDrain());
            }
        }
    }

    IEnumerator HungerZeroHealthDrain()
    {
        isHungerZero = true;

        while (hunger <= 0 && health > 0)
        {
            if (!isTotallyInvincible)
                DecreaseHealth();

            UpdateUI(); // 배고픔 UI 실시간 반영
            yield return new WaitForSeconds(10f);
        }

        isHungerZero = false;
    }

    void DecreaseHealth()
    {
        if (health > 0)
        {
            health--;
            StartCoroutine(BlinkHeart(health));
            UpdateUI();
        }

        if (health <= 0)
        {
            Debug.Log("게임 오버!");
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver(eatTracker.CurrentEatCount, eatTracker.BestEatRecord);
        }

        if (TryGetComponent<PlayerController>(out var controller))
        {
            controller.enabled = false;
        }
    }

    IEnumerator BlinkHeart(int index)
    {
        Image heart = heartImages[index];

        for (int i = 0; i < 6; i++)
        {
            heart.enabled = !heart.enabled;
            yield return new WaitForSeconds(0.15f);
        }

        heart.enabled = false;
    }

    IEnumerator BlinkPlayer(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            foreach (Renderer r in renderers)
                r.enabled = !r.enabled;
            yield return new WaitForSeconds(0.15f);
            elapsed += 0.15f;
        }

        foreach (Renderer r in renderers)
            r.enabled = true;
    }

    public void IncreaseHunger(int amount)
    {
        if (eatPenaltyActive)
        {
            hunger = Mathf.Clamp(hunger - 5, 0, maxHunger); // 패널티로 5 감소
            Debug.Log("먹는 중 패널티 적용됨! -5 감소");
        }
        else
        {
            hunger = Mathf.Clamp(hunger + amount, 0, maxHunger); // 평상시 증가
        }

        UpdateUI();
    }


    public void TakeDamage(int amount)
    {
        if (isInvincible || isTotallyInvincible) return;

        StartCoroutine(TemporaryInvincibility(2f));

        for (int i = 0; i < amount; i++)
        {
            DecreaseHealth();
        }
    }

    IEnumerator TemporaryInvincibility(float duration)
    {
        isInvincible = true;
        StartCoroutine(BlinkPlayer(duration));
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    public void ActivateTotalInvincibility(float duration)
    {
        if (isTotallyInvincible) return;
        StartCoroutine(TotalInvincibilityRoutine(duration));
    }

    IEnumerator TotalInvincibilityRoutine(float duration)
    {
        isTotallyInvincible = true;
        StartCoroutine(BlinkPlayer(duration));
        Debug.Log("무적 발동");
        yield return new WaitForSeconds(duration);
        isTotallyInvincible = false;
        Debug.Log("무적 종료");
    }

    public void EnableEatPenalty(float duration)
    {
        StartCoroutine(EatPenaltyRoutine(duration));
    }

    IEnumerator EatPenaltyRoutine(float duration)
    {
        eatPenaltyActive = true;
        Debug.Log("음식 패널티 시작!");
        yield return new WaitForSeconds(duration);
        eatPenaltyActive = false;
        Debug.Log("음식 패널티 종료");
    }

    void UpdateUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = (i < health);
        }

        if (hungerText != null)
            hungerText.text = hunger + " / " + maxHunger;

        if (hungerFillImage != null)
            hungerFillImage.fillAmount = (float)hunger / maxHunger;
    }
}
