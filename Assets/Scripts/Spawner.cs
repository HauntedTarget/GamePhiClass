using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn = null;
    [SerializeField] KeyCode keyToSpawn;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToSpawn))
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation);
        }
    }
}
