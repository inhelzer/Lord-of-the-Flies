using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrinkMe : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private Spider_YH spider;
    [SerializeField] private GameObject bottlePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool spawnWhenSpiderDies = true;

    [Header("End")]
    [SerializeField] private string nextScene = "Reut_E.B";
    [SerializeField] private float sceneLoadDelay = 1f;

    private bool spawnedBottle;
    private bool endingStarted;

    void Start()
    {
        if (spider == null)
        {
            spider = FindFirstObjectByType<Spider_YH>();
        }
    }

    void Update()
    {
        if (!spawnWhenSpiderDies || spawnedBottle || bottlePrefab == null || spider == null || !spider.IsDead)
        {
            return;
        }

        SpawnBottle();
    }

    void SpawnBottle()
    {
        spawnedBottle = true;

        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
        GameObject bottle = Instantiate(bottlePrefab, spawnPosition, Quaternion.identity);

        DrinkMe bottleDrinkMe = bottle.GetComponent<DrinkMe>();
        if (bottleDrinkMe == null)
        {
            bottleDrinkMe = bottle.AddComponent<DrinkMe>();
        }

        bottleDrinkMe.spider = spider;
        bottleDrinkMe.spawnWhenSpiderDies = false;
        bottleDrinkMe.nextScene = nextScene;
        bottleDrinkMe.sceneLoadDelay = sceneLoadDelay;

        CreateMagicEffect(spawnPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartEnding();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartEnding();
        }
    }

    void StartEnding()
    {
        if (endingStarted || spawnWhenSpiderDies)
        {
            return;
        }

        endingStarted = true;
        CreateMagicEffect(transform.position);
        HideBottle();
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(nextScene);
    }

    void HideBottle()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in renderers)
        {
            sr.enabled = false;
        }

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D bottleCollider in colliders)
        {
            bottleCollider.enabled = false;
        }
    }

    void CreateMagicEffect(Vector3 position)
    {
        GameObject effect = new GameObject("DrinkMe_MagicEffect");
        effect.transform.position = position;

        ParticleSystem particles = effect.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.duration = 0.8f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.35f, 0.9f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(1.8f, 4.5f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.08f, 0.22f);
        main.startColor = new ParticleSystem.MinMaxGradient(
            new Color(0.25f, 0.95f, 1f, 1f),
            new Color(1f, 0.45f, 1f, 1f));
        main.gravityModifier = -0.15f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0f, 45),
            new ParticleSystem.Burst(0.15f, 25)
        });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.7f;

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(new Color(0.35f, 1f, 1f), 0f),
                new GradientColorKey(new Color(1f, 0.55f, 1f), 0.55f),
                new GradientColorKey(Color.white, 1f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0.7f, 0.6f),
                new GradientAlphaKey(0f, 1f)
            });
        colorOverLifetime.color = gradient;

        particles.Play();
        Destroy(effect, 2f);
    }
}
