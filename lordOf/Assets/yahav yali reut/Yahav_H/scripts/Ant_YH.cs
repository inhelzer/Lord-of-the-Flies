using UnityEngine;

public class Ant_YH : MonoBehaviour
{
    public GameObject body;
    Animator anim;
    public float speed = 3.0f;//מהירות הנמלה
    public bool way;//הכיון שהנמלה זזה אליו
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = body.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (way == true)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (way == false)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "wall")
        {
            way = !way;
        }

        if (other.gameObject.tag == "player")
        {
            Destroy(other.gameObject);
        }
    }
}
