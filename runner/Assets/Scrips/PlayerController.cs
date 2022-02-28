using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    Animator animator;
    Vector3 startGamePosition;
    Quaternion startGameRotation;
    float laneOffset = 2.5f;
    float laneChangeSpeed = 15;
    Rigidbody rb;
    Vector3 targetVelocity;
    float pointStart;
    float pointFinish;
    bool isJumping = false;
    float jumpPower = 15;
    float jumpGravity = -40;
    float realGravity = -9.8f;



    // Start is called before the first frame update


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && pointFinish > -laneOffset)
        {
            pointStart = pointFinish;
            pointFinish -= laneOffset;
            targetVelocity = new Vector3(-laneChangeSpeed, 0, 0);
           
            animator.applyRootMotion = false;
        }
        if (Input.GetKeyDown(KeyCode.D) && pointFinish < laneOffset)
        {
            pointStart = pointFinish;
            pointFinish += laneOffset;
            targetVelocity = new Vector3(+laneChangeSpeed, 0, 0);
            
            animator.applyRootMotion = false;
        }
        if (Input.GetKeyDown(KeyCode.W) && isJumping == false)
        {
            Jump();
        }
        float x = Mathf.Clamp(transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));
        transform.position = new Vector3(x, transform.position.y, transform.position.z);

    }

    void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, jumpGravity, 0);
    }
    private void FixedUpdate()
    {
        rb.velocity = targetVelocity;
        if ((transform.position.x > pointFinish && targetVelocity.x > 0) ||
            (transform.position.x < pointFinish && targetVelocity.x < 0))
        {
            targetVelocity = Vector3.zero;
            rb.velocity = targetVelocity;
            rb.position = new Vector3 (pointFinish, rb.position.y, rb.position.z);
        }
    }
    public void StartGame()
    {
        animator.SetTrigger("Run");
        RoadGenerator.instance.StartLevel();
    }
    

    public void ResetGame()
    {
        rb.velocity = Vector3.zero;
        pointStart = 0;
        pointFinish = 0;
        animator.applyRootMotion = true;
        animator.SetTrigger("Idle");
        transform.position = startGamePosition;
        transform.rotation = startGameRotation;
        RoadGenerator.instance.ResetLevel();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "lose")
        {
            ResetGame();
        }
    }
}
