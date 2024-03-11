using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private float maxHealth = 100;
    public float MaxHealth => maxHealth;
    private float currentHealth;
    public static Action<float> OnHeal;

    //Damage
    public static Action<float> OnTakeDamage;
    public static Action<float> OnHealthChange;
    public static Action<float> OnDamage;
    public static Action OnDeath;
    //public static Action<float, Transform> OnTakeDamageIndicator;
    private DamageFeedbackTest damageFeedback;

    #endregion

    #region UNITY EVENT FUNCTIONS

    private void Awake()
    {
        if (damageFeedback == null) damageFeedback = GetComponent<DamageFeedbackTest>();
    }

    private void OnEnable()
    {
        OnTakeDamage += ApplyDamage;
        OnHeal += ApplyHeal;
        //OnTakeDamageIndicator += ApplyDamage;
    }

    private void OnDisable()
    {
        OnTakeDamage -= ApplyDamage;
        OnHeal -= ApplyHeal;
    }

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

        if (currentHealth == 0) OnDeath.Invoke();

        //  Helper.Camera.DOShakeRotation(.5f, 30, 5);                 
    }

    public void ApplyDamage(float damage, Transform enemy)
    {
        currentHealth = Math.Max(0, currentHealth + damage);

        HUD.Instance.ActivateTakeDamageEffect();

        if (currentHealth == 0) OnDeath.Invoke();

        //  Helper.Camera.DOShakeRotation(.5f, 30, 5);                 
    }

    public void ApplyHeal(float healValue)
    {
        currentHealth = Mathf.Min(currentHealth + healValue, maxHealth);
    }

    #endregion
}