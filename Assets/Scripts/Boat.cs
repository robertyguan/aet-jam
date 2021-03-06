﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public BoatCounter boatCounter;
    public List<Transform> wayPoints;
    public float boatSpeed;
    public float turnSpeed;
    public float explosionTorque;
    private int currentWayPoint = 0;
    List<Rigidbody> rigidbodies;
    Rigidbody rb;
    public float explosionForce;
    public float explosionRadius;
    public float upwardsModifier;
    [Range(0,1)]
    public float explosionLerpDistance;
    public Transform oceanTransform;
    private float oceanY;
    private bool beenHit = false;
    AudioSource source;

    void Start()
    {
        
        oceanY = oceanTransform.position.y - 500f;
        rb = GetComponent<Rigidbody>();
        rigidbodies = this.gameObject.GetComponentsInChildren<Rigidbody>().Where(go => go.gameObject != this.gameObject).ToList<Rigidbody>();
        source = GetComponent<AudioSource>();
        // rigidbodies = GetComponentsInChildren<Rigidbody>();
        // rb.useGravity
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != wayPoints[currentWayPoint].position)
        {
            int lastWayPoint = (currentWayPoint + wayPoints.Count - 1) % wayPoints.Count;
            Vector3 something = (wayPoints[currentWayPoint].position - transform.position).normalized * (boatSpeed * Time.deltaTime);
            // Vector3 pos = Vector3.MoveTowards(transform.position, wayPoints[currentWayPoint].position, boatSpeed * Time.deltaTime);
            Quaternion directionToGo = Quaternion.LookRotation(wayPoints[currentWayPoint].position - transform.position, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, directionToGo, turnSpeed * Time.deltaTime);
            if(Vector3.Distance(transform.position, wayPoints[currentWayPoint].position) < something.magnitude)
            {
                transform.position = wayPoints[currentWayPoint].position;
            } 
            else
            {
                transform.position += something;
            }
            //rb.MovePosition(pos);
        } 
        else 
        {
            currentWayPoint = (currentWayPoint + 1) % wayPoints.Count;
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        if(!beenHit)
        {
            beenHit = true;
            CapsuleCollider cc = GetComponent<CapsuleCollider>();
            BoxCollider bc = GetComponent<BoxCollider>();
            cc.enabled = false;
            bc.enabled = false;
            boatCounter.anotherBoatYeeted();
            source.Play();
            foreach(Rigidbody r in rigidbodies)
            {
                r.isKinematic = false;
                r.useGravity = true;
                Vector3 direction = r.transform.position - other.transform.position + Random.insideUnitSphere;
                r.transform.parent = null;
                r.AddTorque(Random.insideUnitSphere * explosionTorque);
                r.AddForceAtPosition((direction.normalized + (Vector3.up * upwardsModifier) + Random.insideUnitSphere) * explosionForce, transform.position, ForceMode.Impulse);
                r.AddForce(Vector3.down * 10, ForceMode.Acceleration);
                StartCoroutine("goDownFaster");
                //r.AddExplosionForce(explosionForce * Random.Range(1, 5), Vector3.Lerp(other.transform.position, r.transform.position, explosionLerpDistance)/*r.transform.position + Random.insideUnitSphere * explosionRadius*/, explosionRadius,upwardsModifier);
            }
        }
        
        
        // Rigidbody r = rigidbodies[0];
        //     r.isKinematic = false;
        //     r.useGravity = true;
        //     r.AddExplosionForce(explosionForce, Vector3.Lerp(transform.position, r.transform.position, explosionLerpDistance), explosionRadius,upwardsModifier);
        // rb.useGravity = true;
        // rb.AddExplosionForce(explosionForce, Vector3.Lerp(other.transform.position, transform.position, explosionLerpDistance), explosionRadius,upwardsModifier);
    }
    
    IEnumerator goDownFaster()
    {
        yield return new WaitForSeconds(1f);

        int underTheSea = 0;
        int numParts = rigidbodies.Count;
        foreach(Rigidbody r in rigidbodies)
        {
            r.gameObject.layer = 12;
        }
        while(underTheSea <= numParts)
        {
            for(int i = rigidbodies.Count - 1; i >= 0; i--)
            {
                
                rigidbodies[i].velocity+=Vector3.down * .05f;
                if(rigidbodies[i].transform.position.y < oceanY)
                {
                    Destroy(rigidbodies[i].transform.gameObject);
                    rigidbodies.Remove(rigidbodies[i]);
                    underTheSea++;
                }
            }
            yield return null;
        }
    }
}
