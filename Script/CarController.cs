using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    public static List<CarController> allCars = new List<CarController>();

    public Transform waypointsParent; // 웨이포인트 부모 오브젝트
    private Transform[] waypoints;

    public float speed = 5f;
    public float turnSpeed = 5f;

    [SerializeField] private int startIndex = 0;
    private int currentIndex;

    private float originalSpeed;
    private bool isStopped = false;
    private bool isReversed = false;

    // 중첩 이벤트 카운트
    private int boostCount = 0;
    private int stopCount = 0;

    void OnEnable()
    {
        if (!allCars.Contains(this))
            allCars.Add(this);
    }

    void OnDisable()
    {
        if (allCars.Contains(this))
            allCars.Remove(this);
    }

    void Start()
    {
        if (waypointsParent != null)
        {
            waypoints = new Transform[waypointsParent.childCount];
            for (int i = 0; i < waypointsParent.childCount; i++)
            {
                waypoints[i] = waypointsParent.GetChild(i);
            }
        }

        currentIndex = Mathf.Clamp(startIndex, 0, waypoints.Length - 1);
        originalSpeed = speed;
    }

    void Update()
    {
        if (waypoints.Length == 0 || isStopped) return;

        // 다음 목표 웨이포인트 설정
        int nextIndex = isReversed ?
            (currentIndex - 1 + waypoints.Length) % waypoints.Length :
            (currentIndex + 1) % waypoints.Length;

        Transform target = waypoints[nextIndex];

        // 이동 방향
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // 회전
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        // 목표 지점 도달 시 인덱스 갱신
        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            currentIndex = nextIndex;
        }
    }

    // 🚀 스피드 부스트
    public void BoostSpeed(float multiplier, float duration)
    {
        boostCount++;
        speed *= multiplier;
        StartCoroutine(ResetSpeedAfter(duration, multiplier));
    }

    private IEnumerator ResetSpeedAfter(float duration, float multiplier)
    {
        yield return new WaitForSeconds(duration);
        boostCount--;
        if (boostCount == 0)
            speed = originalSpeed;
        else
            speed /= multiplier;
    }

    // ⛔ 정지
    public void StopTemporarily(float duration)
    {
        stopCount++;
        isStopped = true;
        StartCoroutine(ResetStopAfter(duration));
    }

    private IEnumerator ResetStopAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        stopCount--;
        if (stopCount <= 0)
        {
            stopCount = 0;
            isStopped = false;
        }
    }

    // 🔁 역주행 (시간 제한 없음, 토글)
    public void ReverseDirection()
    {
        isReversed = !isReversed;
        Debug.Log($"[CarController] {gameObject.name} 역방향 상태: {(isReversed ? "ON" : "OFF")}");
    }
}
