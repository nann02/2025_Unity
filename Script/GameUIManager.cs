using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public GameObject introPanel;
    public GameObject characterSelectUI;

    private bool hasStarted = false;

    void Start()
    {
        introPanel.SetActive(true);
        characterSelectUI.SetActive(false);
    }

    void Update()
    {
        if (!hasStarted && Input.anyKeyDown)
        {
            introPanel.SetActive(false);
            characterSelectUI.SetActive(true);
            hasStarted = true; // �� ���� ����ǵ��� ����
        }
    }
}
