using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private float maxHealth = 100;
    public float MaxHealth => maxHealth;
    private float currentHealth;

    //Damage
    public static Action<float> OnTakeDamage;
    public static Action OnDeath;

    #endregion

    #region UNITY EVENT FUNCTIONS    

    private void Start()
    {
        currentHealth = maxHealth;
    }

    #endregion

    #region METHODS

    public void ApplyDamage(float damage)
    {

        currentHealth = Math.Max(0, currentHealth + damage);

        HUD.Instance.ActivateTakeDamageEffect();

        OnTakeDamage?.Invoke(damage);

        //  Helper.Camera.DOShakeRotation(.5f, 30, 5);                 
    }

    #endregion
}