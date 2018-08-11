using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float m_DampTime = 0.2f;

    public Transform m_target;

    public float scrollSize = 5f;

    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;

    private Camera m_camera;

    private void Awake()
    {
        m_target = GameObject.FindGameObjectWithTag("Player").transform;
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<Camera>();
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        ManageCameraZoom();
    }

    private void Move()
    {
        m_DesiredPosition = m_target.position;
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }

    private void ManageCameraZoom()
    {
        // Get mouse scroll value
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        // Is thge scroll value 0?
        if(scroll == 0f)
        {
            return;
        }

        // Modify the orthographic size by the scroll value
        m_camera.orthographicSize += scroll * scrollSize;
        
    }
}
