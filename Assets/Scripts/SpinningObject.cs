using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObject : MonoBehaviour
{
    [SerializeField] Vector3 rotationPerSecond;
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + (rotationPerSecond * Time.deltaTime));
    }
}
