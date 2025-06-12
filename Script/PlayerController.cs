using UnityEngine;
using Ursaanimation.CubicFarmAnimals; // AnimationController용 네임스페이스

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 90f;
    public float jumpForce = 2f;

    private Rigidbody rb;
    private AnimationController animController;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animController = GetComponent<AnimationController>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        if (!CompareTag("Player")) return;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // 애니메이션 처리
        if (animController != null)
        {
            if (Mathf.Abs(vertical) > 0.1f)
                animController.PlayAnimation(vertical > 0 ? animController.walkForwardAnimation : animController.walkBackwardAnimation);
            else if (Mathf.Abs(horizontal) > 0.1f)
                animController.PlayAnimation(horizontal > 0 ? animController.turn90RAnimation : animController.turn90LAnimation);
            else
                animController.PlayAnimation(animController.standtositAnimation);
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // 회전 (회전은 Update에서 처리!)
        float turnAmount = horizontal * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void FixedUpdate()
    {
        if (!CompareTag("Player")) return;

        float vertical = Input.GetAxis("Vertical");

        // 이동만 FixedUpdate에서 처리
        Vector3 forwardMove = transform.forward * vertical * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}
