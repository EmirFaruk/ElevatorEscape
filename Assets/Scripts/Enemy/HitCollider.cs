using UnityEngine;

public class HitCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth health))
        {
            //PlayerHealth.OnTakeDamage?.Invoke(50);
            health.ApplyDamage(10);
            print("apply damage");
        }
    }
}
