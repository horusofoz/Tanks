﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankShooting : MonoBehaviour {

    // Prefab of the shell
    public Rigidbody m_Shell;
    // A child of the tank where the shells are spawned
    public Transform m_FireTransform;

    // The force fiven to the shell when firing
    public float m_LaunchForce = 30f;

    public float m_ShootDelay = 1f;

    private bool m_CanShoot;
    private float m_ShootTimer;

    private void Awake()
    {
        m_CanShoot = false;
    }


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(m_CanShoot == true)
        {
            m_ShootTimer -= Time.deltaTime;
            if (m_ShootTimer <= 0)
            {
                m_ShootTimer = Time.deltaTime;
                Fire();
            }
        }
	}

    private void Fire()
    {
        // Create an instance of the shell and store a reference to it's rigidbody
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction
        shellInstance.velocity = m_LaunchForce * m_FireTransform.forward;

        m_ShootTimer = m_ShootDelay;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_CanShoot = true;
            m_ShootTimer = m_ShootDelay;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_CanShoot = false;
        }
    }
}
