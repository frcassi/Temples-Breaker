using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovementController : MonoBehaviour
{
    private float speed = 12.0f;
    public Vector3 direction;
    private Rigidbody ballRb;

    private float speedIncrement = 0.2f;
    private float speedIncreaseInterval = 10.0f;

    // Maximum angular deviation from standard bounce angle when bouncing from paddle
    // Deviation added proportionally to the distance between contact point and paddle center
    private float maxAngleDeviation = 0.8f;

    // Lower bound of screen, after which ball gets destroyed and lost
    private float lowerScreenBound = -30.0f;

    // Minimum vertical speed the ball should keep to avoid getting stuck
    private float zSpeedBound = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        direction = Vector3.back;

        StartCoroutine(IncreaseSpeed());
    }

    // Update is called once per frame
    void Update()
    {
        ballRb.velocity = direction * speed;

        if(transform.position.z < lowerScreenBound)
        {
            Debug.Log("GAME OVER!");
            Destroy(gameObject);
        }
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

                direction.x += angleDeviation;
            } else
            {
                // Get vector normal to the collided surface
                Vector3 normalDirection = collision.GetContact(0).normal;

                // Invert direction only along normal to the collison
                direction -= Vector3.Dot(direction, normalDirection) * normalDirection * 2;
            }

            // After bouncing, if the horizontal speed is too low increase it in order to avoid
            // getting stuck bouncing horizontally
            if (direction.z > -zSpeedBound && direction.z < zSpeedBound)
            {
                if(direction.z > 0)
                {
                    direction.z = zSpeedBound;
                }
                else
                {
                    direction.z = -zSpeedBound;
                }
            }
        }
        direction = direction.normalized;
    }

    IEnumerator IncreaseSpeed()
    {
        while(true)
        {
            yield return new WaitForSeconds(speedIncreaseInterval);
            speed += speedIncrement;
        }
    }
}
