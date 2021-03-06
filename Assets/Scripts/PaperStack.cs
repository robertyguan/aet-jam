﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaperStack : InteractableObject
{
    public Rigidbody rb;
    public Collider collider;
//     public float explosionForce;
//     public float explosionRadius;
//     public float upwardsModifier;
//     [Range(0, 1)]
//     public float explosionLerpDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DoAction(Transform playerTransform, Vector3 hitPoint)
    {
        base.DoAction(playerTransform, hitPoint);

        transform.parent = playerTransform;
        playerTransform.gameObject.GetComponent<CharacterInteract>().hasStack = true;
        rb.useGravity = false;
        collider.enabled = false;
    }

//     public void LaunchStack(Vector3 playerPos)
//     {
//         rb.AddExplosionForce(explosionForce, Vector3.Lerp(playerPos, transform.position, explosionLerpDistance), explosionRadius, upwardsModifier);
// 
//     }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cabinet"))
        {
            collider.isTrigger = true;
        }
        else if (collision.gameObject.CompareTag("Clear"))
        {
            SceneManager.LoadScene("TreeDialogScene");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Clear"))
        {
            SceneManager.LoadScene("TreeDialogScene");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cabinet"))
        {
            collider.isTrigger = false;
        }
    }
}
