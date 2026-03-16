using UnityEngine;

public class BuildingOpacityChange : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private SpriteRenderer sprite;
    private Color originalColor;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

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

        sprite.color = originalColor;
    }
}