using UnityEngine;

public class BuildingOpacityChange : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private SpriteRenderer sprite;

    private Color originalColor;

    private void Start()
    {
        originalColor = sprite.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Alpha = 200 מתוך 255
        sprite.color = new Color(
            originalColor.r,
            originalColor.g,
            originalColor.b,
            200f / 255f
        );
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        // מחזיר לאטימות מלאה
        sprite.color = originalColor;
    }
}