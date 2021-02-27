using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private void FixedUpdate()
    {
        if (GameController.singleton.GameStarted)
            transform.position = transform.position + Vector3.forward * speed * Time.fixedDeltaTime;
    }
}
