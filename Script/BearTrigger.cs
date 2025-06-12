using UnityEngine;

public class BearTrigger : MonoBehaviour
{
    public GameObject[] bears;       // 따라올 곰들
    public Transform player;         // 추적 대상 플레이어

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

            Debug.Log("플레이어가 곰 트리거에 진입했습니다.");
        }
    }
}
