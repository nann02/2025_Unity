using UnityEngine;
using UnityEngine.AI;

public class BearFollower : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private bool isFollowing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = 4f;

        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;

        // 최초 연결 시도
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        // 나중에라도 연결
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else return;
        }

        // 추적 상태일 때만 목적지 설정
        if (isFollowing && player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void FollowPlayer()
    {
        isFollowing = true;
        agent.isStopped = false;

        // 걷기 AI와 충돌 방지
        WanderBear wander = GetComponent<WanderBear>();
        if (wander != null) wander.enabled = false;
    }
}
