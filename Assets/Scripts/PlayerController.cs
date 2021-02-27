using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float force = 100f;
    private Rigidbody rb;
    private Vector2 lastTouchPosition;
    private float wallDistance = 5f;
    private float camDistance = 1f;
    private Animator anim;

    [SerializeField] private ParticleSystem[] dusts;

    // Для теста:
    private bool android;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        
        android = (Application.platform == RuntimePlatform.Android) ? true: false;
    }

    private void Update()
    {
        if (GameController.singleton.GameEnded)
            return;

        if (android)
        {
            Vector2 deltaPosition = Vector2.zero;

            if (Input.touchCount > 0)
            {
                if (!GameController.singleton.GameStarted)
                {
                    GameController.singleton.StartGame();
                    anim.SetBool("Run", true);
                    
                    foreach (ParticleSystem item in dusts)
                        item.Play();
                }

                Vector2 currentTouchPosition = Input.GetTouch(0).position;

                if (lastTouchPosition == Vector2.zero)
                {
                    lastTouchPosition = currentTouchPosition;
                }

                deltaPosition = currentTouchPosition - lastTouchPosition;

                lastTouchPosition = currentTouchPosition;

                Vector3 addForce = new Vector3(deltaPosition.x, 0, deltaPosition.y) * force;
                rb.AddForce(addForce);
            }
            else
            {
                lastTouchPosition = Vector2.zero;
            }
        }
        // Для теста управления в редакторе Unity
        else
        {
            Vector2 deltaPos = Vector2.zero;

            if (Input.GetMouseButton(0))
            {
                if (!GameController.singleton.GameStarted)
                {
                    GameController.singleton.StartGame();
                    anim.SetBool("Run", true);

                    foreach (ParticleSystem item in dusts)
                        item.Play();
                }

                Vector2 currentTouchPosition = Input.mousePosition;

                if (lastTouchPosition == Vector2.zero)
                {
                    lastTouchPosition = currentTouchPosition;
                }

                deltaPos = currentTouchPosition - lastTouchPosition;

                lastTouchPosition = currentTouchPosition;

                Vector3 addForce = new Vector3(deltaPos.x, 0, deltaPos.y) * force;
                rb.AddForce(addForce);
            }
            else
            {
                lastTouchPosition = Vector2.zero;
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameController.singleton.GameEnded)
            return;
        if (GameController.singleton.GameStarted)
        {
            rb.MovePosition(transform.position + Vector3.forward * speed * Time.fixedDeltaTime);
        }
    }

    private void LateUpdate()
    {
        if (GameController.singleton.GameEnded)
            return;

        Vector3 pos = transform.position;

        if (transform.position.x < -wallDistance)
        {
            pos.x = -wallDistance;
            rb.velocity = Vector3.zero;
        }
        else if (transform.position.x > wallDistance)
        {
            pos.x = wallDistance;
            rb.velocity = Vector3.zero;
        }

        
        if (transform.position.z < Camera.main.transform.position.z + camDistance)
        {
            pos.z = Camera.main.transform.position.z + camDistance + 0.1f; // 0.1f - погрешность, чтобы персонаж не застревал
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        }

        transform.position = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameController.singleton.GameEnded)        
            return;

        if (collision.transform.tag == "Death")
        {
            GameController.singleton.EndGame(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameController.singleton.GameEnded)
            return;

        if (other.transform.tag == "End")
        {
            GameController.singleton.EndGame(true);
        }
    }
}
