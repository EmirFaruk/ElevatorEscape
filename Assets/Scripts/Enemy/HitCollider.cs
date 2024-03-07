using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerHealth health))
        {
            health.ApplyDamage(50);
            print("apply damage");
        }
    }
}
