using UnityEngine;

public class Ant_YH : MonoBehaviour
{
    Animator anim;
    public float speed = 3.0f;//מהירות הנמלה
    public bool way;//הכיון שהנמלה זזה אליו
    public string ant;

    string currentAnimation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        ChangeAnimationState(ant);
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
        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            way = !way;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;
        anim.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
