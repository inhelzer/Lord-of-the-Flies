using UnityEngine;

public class IH_rock : MonoBehaviour
{
    [SerializeField] Sprite[] looks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = looks[Random.Range(0,looks.Length)];
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
