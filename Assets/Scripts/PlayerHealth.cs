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
    public static Action<float, Transform> OnTakeDamageIndicator;
    public static Action<float> OnDamage;
    public static Action OnDeath;
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
        OnTakeDamageIndicator += ApplyDamage;
        OnHeal += ApplyHeal;
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
        currentHealth = Math.Max(0, currentHealth - damage);

        HUD.Instance.ActivateTakeDamageEffect();

        if (currentHealth == 0) OnDeath.Invoke();

        //  Helper.Camera.DOShakeRotation(.5f, 30, 5);                 
    }

    public void ApplyDamage(float damage, Transform enemy)
    {
        currentHealth = Math.Max(0, currentHealth - damage);

        HUD.Instance.ActivateTakeDamageEffect();

        if (currentHealth == 0) OnDeath.Invoke();

        //  Helper.Camera.DOShakeRotation(.5f, 30, 5);                 
    }

    #region CALISMAYAN APPLY DAMAGE
    /*
      public void ApplyDamage(float damage, Transform enemy) // calısmıyor.
      {
          damageFeedback.ShowUI(enemy);
          damageFeedback.ShowDamageIndicator(enemy);

          OnDamage?.Invoke(currentHealth);

          Helper.Camera.DOShakeRotation(.5f, 30, 5);            
      }
    */
    #endregion

    public void ApplyHeal(float healValue)
    {
        currentHealth = Mathf.Min(currentHealth + healValue, maxHealth);
    }

    #endregion
}

#region Backup

/*#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            private PlayerInput _playerInput;
    #endif*/

#region Is Current Device Mouse
/* {
     get
     {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
         return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                 return false;
#endif
     }
 }*/
#endregion

#endregion