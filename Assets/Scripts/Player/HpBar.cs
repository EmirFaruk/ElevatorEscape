using System.Threading.Tasks;
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
    }

    private void OnDisable()
    {
        PlayerHealth.OnTakeDamage -= UpdateHealthBar;
    }

    async void UpdateHealthBar(float value)
    {
        float fillAmount = Mathf.Max(0, image.fillAmount + value / 100);

        while (image.fillAmount > fillAmount)
        {
            image.fillAmount -= Time.deltaTime;
            await Task.Delay(10);
        }

        if (image.fillAmount == .0f) PlayerHealth.OnDeath.Invoke();
    }
}