using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem part;
    void Awake()
    {
        part = GetComponent<ParticleSystem>();
    }
    public void SetShapeAngle(Vector3 angle)
    {
        var eff = part.shape;
        eff.rotation = angle;
    }
}
