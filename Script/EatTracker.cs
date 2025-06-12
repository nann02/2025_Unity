using UnityEngine;
using TMPro;

public class EatTracker : MonoBehaviour
{
    public TMP_Text eatCountText;
    public TMP_Text newRecordText;

    private int eatCount = 0;
    private int bestRecord = 0;

    public int CurrentEatCount => eatCount;      // ✅ 추가: 외부에서 읽기용
    public int BestEatRecord => bestRecord;      // ✅ 추가

    void Start()
    {
        bestRecord = PlayerPrefs.GetInt("BestEatRecord", 0);
        UpdateUI();
        newRecordText.gameObject.SetActive(false); // NEW! 처음엔 숨김
    }

    public void AddEat()
    {
        eatCount++;

        if (eatCount > bestRecord)
        {
            bestRecord = eatCount;
            PlayerPrefs.SetInt("BestEatRecord", bestRecord);
            newRecordText.gameObject.SetActive(true); // NEW! 표시
        }

        UpdateUI();
    }

    public void ResetEat()
    {
        eatCount = 0;
        newRecordText.gameObject.SetActive(false);
        UpdateUI();
    }

    private void UpdateUI()
    {
        eatCountText.text = $"섭취 수: {eatCount} (최고기록: {bestRecord})";
    }
}
