using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointRotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 3;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0.1f * rotationSpeed, 0);
    }
}
