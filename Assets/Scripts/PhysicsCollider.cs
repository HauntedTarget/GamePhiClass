using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCollider : MonoBehaviour
{
    string status;

    private void OnCollisionEnter(Collision collision)
    {
        status = "Collision Enter: " + collision.gameObject.name;
    }

    private void OnCollisionStay(Collision collision)
    {
        status = "Collision Stay: " + collision.gameObject.name;
    }

    private void OnCollisionExit(Collision collision)
    {
        status = "Collision Exit: " + collision.gameObject.name;
    }

    private void OnTriggerEnter(Collider other)
    {
        status = "Trigger Enter: " + other.gameObject.name;
    }

    private void OnTriggerStay(Collider other)
    {
        status = "Trigger Stay: " + other.gameObject.name;
    }

    private void OnTriggerExit(Collider other)
    {
        status = "Trigger Exit: " + other.gameObject.name;
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 16;
        Vector2 screen = Camera.main.WorldToScreenPoint(transform.position);
        GUI.Label(new(screen.x, Screen.height - screen.y, 250, 70), status);
    }
}
