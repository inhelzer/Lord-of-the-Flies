using UnityEngine;

public class PerorCodeYH : MonoBehaviour
{
    [SerializeField] private int piecesX = 5;
    [SerializeField] private int piecesY = 5;
    [SerializeField] private float pieceExplosionForce = 6f;
    [SerializeField] private float pieceTorque = 200f;
    [SerializeField] private float pieceLifetime = 2.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        ExplodeSpriteToPieces();
        Destroy(gameObject);
    }

    private void ExplodeSpriteToPieces()
    {
        SpriteRenderer sourceRenderer = GetComponent<SpriteRenderer>();
        if (sourceRenderer == null || sourceRenderer.sprite == null)
        {
            return;
        }

        Sprite sourceSprite = sourceRenderer.sprite;
        Texture2D texture = sourceSprite.texture;
        Rect spriteRect = sourceSprite.rect;

        int safePiecesX = Mathf.Max(1, piecesX);
        int safePiecesY = Mathf.Max(1, piecesY);
        float cellWidth = spriteRect.width / safePiecesX;
        float cellHeight = spriteRect.height / safePiecesY;

        Vector2 pivotPixels = sourceSprite.pivot;
        float ppu = sourceSprite.pixelsPerUnit;

        for (int y = 0; y < safePiecesY; y++)
        {
            for (int x = 0; x < safePiecesX; x++)
            {
                Rect pieceRect = new Rect(
                    spriteRect.x + x * cellWidth,
                    spriteRect.y + y * cellHeight,
                    cellWidth,
                    cellHeight
                );

                Sprite pieceSprite = Sprite.Create(
                    texture,
                    pieceRect,
                    new Vector2(0.5f, 0.5f),
                    ppu
                );

                GameObject piece = new GameObject("SpritePiece");
                piece.transform.position = GetPieceWorldPosition(x, y, cellWidth, cellHeight, pivotPixels, ppu);
                piece.transform.rotation = transform.rotation;
                piece.transform.localScale = transform.lossyScale;

                SpriteRenderer pieceRenderer = piece.AddComponent<SpriteRenderer>();
                pieceRenderer.sprite = pieceSprite;
                pieceRenderer.color = sourceRenderer.color;
                pieceRenderer.sortingLayerID = sourceRenderer.sortingLayerID;
                pieceRenderer.sortingOrder = sourceRenderer.sortingOrder;

                Rigidbody2D rb = piece.AddComponent<Rigidbody2D>();
                rb.gravityScale = 1f;

                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                rb.AddForce(randomDirection * pieceExplosionForce, ForceMode2D.Impulse);
                rb.AddTorque(Random.Range(-pieceTorque, pieceTorque), ForceMode2D.Impulse);

                Destroy(piece, pieceLifetime);
            }
        }
    }

    private Vector3 GetPieceWorldPosition(int x, int y, float cellWidth, float cellHeight, Vector2 pivotPixels, float ppu)
    {
        float centerX = (x + 0.5f) * cellWidth;
        float centerY = (y + 0.5f) * cellHeight;

        Vector2 localOffset = new Vector2(
            (centerX - pivotPixels.x) / ppu,
            (centerY - pivotPixels.y) / ppu
        );

        return transform.TransformPoint(localOffset);
    }
}
