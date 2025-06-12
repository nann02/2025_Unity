using UnityEngine;
using TMPro;
using System.Collections;

public class ItemEffectDisplay : MonoBehaviour
{
    public TextMeshProUGUI effectText;
    public CanvasGroup canvasGroup;
    public float displayDuration = 1.5f;

    void Start()
    {
        effectText.text = "";
        canvasGroup.alpha = 0;
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowAndHide(message));
    }

    IEnumerator ShowAndHide(string message)
    {
        effectText.text = message;
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(displayDuration);
        canvasGroup.alpha = 0;
    }
}
