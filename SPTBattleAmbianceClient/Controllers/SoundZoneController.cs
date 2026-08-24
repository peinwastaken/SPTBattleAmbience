using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SPTBattleAmbience.Controllers;

public class SoundZoneController : MonoBehaviour
{
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = gameObject.AddComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    public Vector3 PickRandomPoint()
    {
        Vector3 vector =  _collider.center + new Vector3(
            Random.Range(-0.5f, 0.5f) * _collider.size.x,
            Random.Range(-0.5f, 0.5f) * _collider.size.y,
            Random.Range(-0.5f, 0.5f) * _collider.size.z
        );
        
        return transform.TransformPoint(vector);
    }
}
