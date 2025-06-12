using UnityEngine;

public class VehicleDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HungerSystem hunger = collision.gameObject.GetComponent<HungerSystem>();
            if (hunger != null)
            {
                hunger.TakeDamage(1);
                Debug.Log("차량 충돌 - 체력 감소!");
            }
        }
    }
}