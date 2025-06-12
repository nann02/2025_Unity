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

        // ���� ���� �õ�
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
        // ���߿��� ����
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else return;
        }

        // ���� ������ ���� ������ ����
        if (isFollowing && player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void FollowPlayer()
    {
        isFollowing = true;
        agent.isStopped = false;

        // �ȱ� AI�� �浹 ����
        WanderBear wander = GetComponent<WanderBear>();
        if (wander != null) wander.enabled = false;
    }
}
