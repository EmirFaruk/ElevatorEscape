using UnityEngine;

public class HitCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth health))
        {
            health.ApplyDamage(-10);
            print("apply damage");
            //PlayerHealth.OnTakeDamage?.Invoke(-10);
        }
    }
}
