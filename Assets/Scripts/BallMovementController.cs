using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovementController : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    private Rigidbody ballRb;

    private float maxAngleDeviation = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        direction = Vector3.back;
    }

    // Update is called once per frame
    void Update()
    {
        ballRb.velocity = direction * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Plane")
        {
            if(collision.gameObject.CompareTag("Paddle"))
            {
                GameObject paddle = collision.gameObject;

                direction.z *= -1; // Invert direction on z axis

                // If the ball doesn't bounce in the center of the paddle, add an horizontal angle
                // proportional to distance from center
                float paddleHorizontalLength = paddle.transform.localScale.x;
                float distanceFromPaddleCenter = transform.position.x - paddle.transform.position.x;

                // distanceFromPaddleCenter:maxAngleDeviation = angleDeviation:maxAngleDeviation
                float angleDeviation = distanceFromPaddleCenter * maxAngleDeviation / (paddleHorizontalLength / 2);

                Debug.Log(angleDeviation);

                direction.x += angleDeviation;
            } else
            {
                // Get vector normal to the collided surface
                Vector3 normalDirection = collision.GetContact(0).normal;

                // Invert direction only along normal to the collison
                direction += normalDirection * 2;
            }
        }
        direction = direction.normalized;
    }
}
