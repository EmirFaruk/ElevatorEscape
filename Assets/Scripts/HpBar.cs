using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnTakeDamage += UpdateHealthBar;
        //PlayerHealth.OnHeal += UpdateHealthBar;
    }

    private void OnDisable()
    {
        PlayerHealth.OnTakeDamage -= UpdateHealthBar;
        //PlayerHealth.OnHeal -= UpdateHealthBar;
    }

    void UpdateHealthBar(float value)
    {
        image.fillAmount = Mathf.Max(0, image.fillAmount - value / 100);
    }
}
