using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinScript : MonoBehaviour
{
    PlayerController playerController;
    Transform playerTransform;

    public Transform targetTransform;
    private float angle;

    private void Start()
    {
        playerController = PlayerController.instance;

        playerTransform = playerController.transform;
    }

    private void Update()
    {
        if (targetTransform == null) Destroy(gameObject);
        else
        {
            var dir = targetTransform.position - playerTransform.position;
            var angle2 = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle2, Vector3.forward);
        }

    }

}
