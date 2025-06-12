using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    public enum AnimalType { Goat, CreamSheep, DarkSheep }
    public static AnimalType selectedAnimalType;

    public GameObject goatPrefab;
    public GameObject sheepCreamPrefab;
    public GameObject sheepDarkPrefab;
    public Transform spawnPoint;
    public Canvas uiCanvas;

    public GameObject hungerBar;
    public GameObject staminaUI;
    public GameObject itemEffectText;

    public GameObject topLeftUI; // ✅ 인스펙터에서 연결할 UI

    private bool hasSpawned = false;

    void Update()
    {
        if (hasSpawned) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectGoat();
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SelectCreamSheep();
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SelectDarkSheep();
    }

    public void SelectGoat()
    {
        selectedAnimalType = AnimalType.Goat;
        Spawn(goatPrefab);
    }

    public void SelectCreamSheep()
    {
        selectedAnimalType = AnimalType.CreamSheep;
        Spawn(sheepCreamPrefab);
    }

    public void SelectDarkSheep()
    {
        selectedAnimalType = AnimalType.DarkSheep;
        Spawn(sheepDarkPrefab);
    }

    void Spawn(GameObject prefab)
    {
        if (hasSpawned || prefab == null) return;

        GameObject player = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        player.tag = "Player";

        Camera.main.transform.SetParent(player.transform);
        Camera.main.transform.localPosition = new Vector3(0f, 2f, -4f);
        Camera.main.transform.LookAt(player.transform);

        uiCanvas.gameObject.SetActive(false);
        if (hungerBar != null) hungerBar.SetActive(true);
        if (staminaUI != null) staminaUI.SetActive(true);
        if (itemEffectText != null) itemEffectText.SetActive(true);
        if (topLeftUI != null) topLeftUI.SetActive(true); // ✅ 여기에서 직접 켜줌

        hasSpawned = true;
    }
}
