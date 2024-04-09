using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarMoverOnCollision : MonoBehaviour
{
    [SerializeField] GameObject collisionActivator = null;
    [SerializeField, Range(-1, 1)] float forewardSpeed = 1, directionalSpeed = 0;

    public bool activated = false;

    [System.Serializable]
    public struct Wheel
    {
        public WheelCollider collider;
    }
    [System.Serializable]
    public struct Axle
    {
        public Wheel leftWheel;
        public Wheel rightWheel;
        public bool isMotor;
        public bool isSteering;
    }
    [SerializeField] Axle[] axles;
    [SerializeField] float maxMotorTorque;
    [SerializeField] float maxSteeringAngle;
    public void FixedUpdate()
    {
        float motor = (activated? forewardSpeed : 0) * maxMotorTorque;
        float steering = (activated ? directionalSpeed : 0) * maxSteeringAngle;
    foreach (Axle axle in axles)
        {
            if (axle.isSteering)
            {
                axle.leftWheel.collider.steerAngle = steering;
                axle.rightWheel.collider.steerAngle = steering;
            }
            if (axle.isMotor)
            {
                axle.leftWheel.collider.motorTorque = motor;
                axle.rightWheel.collider.motorTorque = motor;
            }
        }
    if (Input.GetKeyDown(KeyCode.Space))
        {
            activated = true;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == collisionActivator.name)
        {
            activated = true;
        }
    }
}