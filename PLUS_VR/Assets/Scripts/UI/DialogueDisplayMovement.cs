using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDisplayMovement : MonoBehaviour {

    private Transform m_UIElement;         // The transform of the UI to be affected.
    public Transform m_camera;            // The transform of the camera to follow
    private bool m_rotating;       // Whether the UI should be rotating to the center of the camera.
    public float m_followSpeed = 10f;     // The speed with which the UI should follow the camera.
    public Vector3 m_initialDistanceFromObject = Vector3.zero; // Initial distance, if zero, use world space coord

    private Vector3 targetPosition;
    private Vector3 targetLookPos;

    private void Start()
    {
        m_UIElement = transform;
    }


    private void Update()
    {
        Vector3 yOffset = new Vector3(0, m_initialDistanceFromObject.y, 0);
        Vector3 toUI = (m_UIElement.position - yOffset) - m_camera.position;

        //if the angle between the vectors is greater than ~45 degrees or the UI is too far away or too close, set a new target position and lookpos
        if (Vector3.Dot(toUI.normalized, m_camera.forward) < 0.7f || toUI.magnitude > m_initialDistanceFromObject.z * 1.2f 
            || toUI.magnitude < m_initialDistanceFromObject.z * 0.8f)
        {
            m_rotating = true;
        }
        else
        {
            m_rotating = false;
        }

        // If the UI should rotate with the camera...
        if (m_rotating)
        {
            // Find the direction the camera is looking but on a flat plane.
            Vector3 targetDirection = Vector3.ProjectOnPlane(m_camera.forward, Vector3.up).normalized;

            // Calculate a target position from the object in the direction at the same distance from the tracked object as it was at Start.
            targetPosition = m_camera.position + (targetDirection * m_initialDistanceFromObject.z);

            targetPosition += yOffset;
            targetLookPos = m_camera.position;
        }
        //always lerp towards the target and look towards the target
        m_UIElement.position = Vector3.Slerp(m_UIElement.position, targetPosition, m_followSpeed * Time.deltaTime);
        m_UIElement.rotation = Quaternion.LookRotation(m_UIElement.position - targetLookPos);
    }
}
