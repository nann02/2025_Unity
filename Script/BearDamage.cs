using UnityEngine;

public class BearDamage : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HungerSystem hunger = other.gameObject.GetComponent<HungerSystem>();
            if (hunger != null)
            {
                hunger.TakeDamage(1);
                Debug.Log("�� �浹 - ü�� ����");
            }
        }
    }
}
