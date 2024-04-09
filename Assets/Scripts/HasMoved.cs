using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class HasMoved : MonoBehaviour
{
    [SerializeField] UnityEvent onMove = null;
    [SerializeField] bool destroyAfterMove = false;
    [SerializeField] GameObject destroyEffect = null;
    [SerializeField] float distanceMoved = 0.5f, angleMoved = 45;

    Transform initialTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        initialTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckMoved())
        {
                if (onMove != null) onMove.Invoke();

                if (destroyAfterMove)
                {
                    if (destroyEffect != null) Instantiate(destroyEffect, gameObject.transform, true);
                    Destroy(this.gameObject);
                }
        }
    }

    bool CheckMoved()
    {
        if (transform.position.x > initialTransform.position.x + distanceMoved || transform.position.x < initialTransform.position.x - distanceMoved)
            return true;
        if (transform.position.y > initialTransform.position.y + distanceMoved || transform.position.y < initialTransform.position.y - distanceMoved)
            return true;
        if (transform.position.z > initialTransform.position.z + distanceMoved || transform.position.z < initialTransform.position.z - distanceMoved)
            return true;

        if (transform.rotation.x > initialTransform.rotation.x + angleMoved || transform.rotation.x < initialTransform.rotation.x - angleMoved)
            return true;
        if (transform.rotation.y > initialTransform.rotation.y + angleMoved || transform.rotation.y < initialTransform.rotation.y - angleMoved)
            return true;
        if (transform.rotation.z > initialTransform.rotation.z + angleMoved || transform.rotation.z < initialTransform.rotation.z - angleMoved)
            return true;

        return false;
    }
}
