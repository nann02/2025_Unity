using UnityEngine;

public class BearTrigger : MonoBehaviour
{
    public GameObject[] bears;       // ����� ����
    public Transform player;         // ���� ��� �÷��̾�

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject bear in bears)
            {
                BearFollower follower = bear.GetComponent<BearFollower>();
                if (follower != null)
                {
                    follower.FollowPlayer();
                }
            }

            Debug.Log("�÷��̾ �� Ʈ���ſ� �����߽��ϴ�.");
        }
    }
}
