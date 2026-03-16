using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillMask;
    [SerializeField] private float animationSpeed = 8f;
    [SerializeField] private TMP_Text healthText;

    private float fullWidth;
    private float targetWidth;
    private float currentWidth;

    private void Awake()
    {
        fullWidth = fillMask.sizeDelta.x;
        currentWidth = fullWidth;
        targetWidth = fullWidth;


    }

    private void Update()
    {
        if (fillMask == null) return;
        currentWidth = Mathf.Lerp(currentWidth, targetWidth, Time.deltaTime * animationSpeed);

        if (Mathf.Abs(currentWidth - targetWidth) < 0.1f)
            currentWidth = targetWidth;

        fillMask.sizeDelta = new Vector2(currentWidth, fillMask.sizeDelta.y);
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        float percent = Mathf.Clamp01(currentHealth / maxHealth);
        targetWidth = fullWidth * percent;

        if (healthText != null)
            healthText.text = Mathf.RoundToInt(currentHealth).ToString();

        //Debug.Log("SetHealth: " + Mathf.RoundToInt(currentHealth));

        if (healthText != null)
        {
            healthText.text = Mathf.RoundToInt(currentHealth).ToString();
            //Debug.Log("Text set to: " + healthText.text);
        }
    }

    public void SetHealthInstant(float currentHealth, float maxHealth)
    {
        float percent = Mathf.Clamp01(currentHealth / maxHealth);
        targetWidth = fullWidth * percent;
        currentWidth = targetWidth;

        fillMask.sizeDelta = new Vector2(currentWidth, fillMask.sizeDelta.y);

        if (healthText != null)
            healthText.text = Mathf.RoundToInt(currentHealth).ToString();
    }
}