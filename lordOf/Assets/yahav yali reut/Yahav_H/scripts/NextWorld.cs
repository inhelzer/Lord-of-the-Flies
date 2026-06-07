using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class NextWorld : MonoBehaviour
{
    private static readonly string[] defaultScenes = { "Yahav_H", "YZ_GAZA_OLD", "yarin1", "Exit_pyramid" };

    [SerializeField] private bool isOpeningScene;
    [SerializeField] string sceneName;
    static string[] scenes = { "Yahav_H", "YZ_GAZA_OLD", "yarin1", "Exit_pyramid" };
    [SerializeField] private VideoPlayer myVideoPlayer;


    private void Start()
    {
        if (!isOpeningScene)
        {
            RemoveCurrentSceneFromRandomList();
        }

        if (myVideoPlayer != null)
        {
            // ????? ???????? ?????? ???? ??????
            myVideoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    private void RemoveCurrentSceneFromRandomList()
    {
        int nulls = 0;

        for (int i = 0; i < scenes.Length; i++)
        {
            if (scenes[i] == sceneName)
            {
                scenes[i] = null;
            }
            if (scenes[i] == null)
            {
                nulls++;
            }
        }

        if (nulls == scenes.Length)
        {
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = defaultScenes[i];
            }
        }
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        int sceneIndex;
        do
        {
            sceneIndex = Random.Range(0, scenes.Length);
        }
        while (scenes[sceneIndex] == null);

        SceneManager.LoadScene(scenes[sceneIndex]);

        // ??? ??? ???? ?????? ???? ??? ?????, ????:
        // SceneManager.LoadScene("Level1");
    }

    private void OnDestroy()
    {
        if (myVideoPlayer != null)
        {
            // ????? ???? ???? ?? ?????? ????????? ?? ???????? ??? ????? ????? ??????
            myVideoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}
