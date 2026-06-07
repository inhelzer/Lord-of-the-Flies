using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending_KF : MonoBehaviour
{
    [SerializeField] private float secondsBeforeNextScene = 5f;
    [SerializeField] private string nextSceneName = "V2";
    [SerializeField] private AudioSource music;
    [SerializeField] private Light sceneLight;
    [SerializeField] private Color startCameraColor = new Color(0.02f, 0.02f, 0.05f);
    [SerializeField] private Color endCameraColor = new Color(0.25f, 0.06f, 0.02f);

    private Camera sceneCamera;
    private float timer;
    private bool hasLoadedNextScene;

    private void Start()
    {
        sceneCamera = GetComponent<Camera>();

        if (music == null)
        {
            music = GetComponent<AudioSource>();
        }

        if (sceneLight == null)
        {
            sceneLight = GetComponent<Light>();
        }

        if (sceneCamera != null)
        {
            sceneCamera.backgroundColor = startCameraColor;
        }

        if (music != null && !music.isPlaying)
        {
            music.Play();
        }
    }

    private void Update()
    {
        if (hasLoadedNextScene)
        {
            return;
        }

        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / secondsBeforeNextScene);

        if (sceneCamera != null)
        {
            sceneCamera.backgroundColor = Color.Lerp(startCameraColor, endCameraColor, progress);
        }

        if (sceneLight != null)
        {
            sceneLight.intensity = 1f + Mathf.PingPong(Time.time * 3f, 1.5f);
            sceneLight.transform.Rotate(Vector3.up, 30f * Time.deltaTime);
        }

        if (timer >= secondsBeforeNextScene)
        {
            hasLoadedNextScene = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
