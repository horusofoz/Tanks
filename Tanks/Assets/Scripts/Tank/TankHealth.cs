using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHealth : MonoBehaviour {

    // The amoun of health each tank starts with
    public float m_StartingHealth = 100f;

    // A prefab that will be instantiated in Awake, then used whenever the tank dies
    public GameObject m_ExplosionPrefab;

    // Holder for camera
    public Camera m_mainCamera;

    private float m_CurrentHealth;
    private bool m_Dead;

    // The particle sstem that will play when the tank is destroyed
    private ParticleSystem m_ExplosionParticles;

    // Reference to enemy base/spawn location
    private Transform m_StartingPosition;

    // Reference to tank renderer game object
    private GameObject m_TankRenderers;

    private void Awake()
    {
        // Instantiate the explosion prefab and get a reference to the particle system on it
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

        // Disable the prefab so it can be activated when it's required
        m_ExplosionParticles.gameObject.SetActive(false);

        // Get reference to enemy base/spawn location
        m_StartingPosition = GetStartingPosition();

        // Get reference to tank renderer game object
        m_TankRenderers = gameObject.transform.Find("TankRenderers").gameObject;
    }

    private void OnEnable()
    {
        // When the tank is enabled, reset the tank's health and whether or not it's dead
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }

    private void SetHealthUI()
    {
        // TODO: Update the user interface showing the tank's health
    }

    public void TakeDamage(float amount)
    {
        // Reduce current health by the amount of damage done
        m_CurrentHealth -= amount;

        // Change th eUI elements appropriately
        SetHealthUI();

        // If the current health is at or below zerio and it has not yet been registered, call OnDeath
        if(m_CurrentHealth <= 0 && !m_Dead)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        // Set the flag that this function is only called once
        m_Dead = true;

        // move the instantiated explosion prefab to the tank's position and turn it on
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        // Play the particle system of the tank exploding
        m_ExplosionParticles.Play();

        // Unparent Main Camera
        //if(m_mainCamera != null)
        //{
        //    m_mainCamera.transform.parent = null;
        //}

        StartCoroutine(ReSpawn());

        // Turn the tank off
        //gameObject.SetActive(false);

        // Turn renderer gameObject off instead of object so can still run ReSpawn
        m_TankRenderers.SetActive(false);
        
    }

    private IEnumerator ReSpawn()
    {
        // Wait 2 seconds
        yield return new WaitForSeconds(2);

        // Move object to spawn position
        gameObject.transform.position = m_StartingPosition.position;

        // Enable object so that it utilises the existing OnEnable function
        //gameObject.SetActive(true);

        m_TankRenderers.SetActive(true);

        // Potential issue with main camera which is currently childed to the player tank
    }

    private Transform GetStartingPosition()
    {
        if(gameObject.tag == "Enemy Tank")
        {
             return GameObject.FindGameObjectWithTag("Enemy Base").transform;
        }
        else
        {
            return GameObject.FindGameObjectWithTag("Player Base").transform;
        }
    }

    
}
