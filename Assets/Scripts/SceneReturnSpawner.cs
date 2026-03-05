using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneReturnSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // Scene 1 only
    public GameObject player;       // Scene 1 only
    public float transitionDelay = 2f;

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // ===== SCENE 2 =====
        if (sceneName == "Scene2")
        {
            StartCoroutine(ReturnToSceneOne());
            return;
        }

        // ===== SCENE 1 =====
        if (sceneName == "Scene1" && ExitDoorTransition.hasTransitioned)
        {
            StartCoroutine(MovePlayerAfterFrame());
        }
    }

    IEnumerator ReturnToSceneOne()
    {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene("Scene1");
    }

    IEnumerator MovePlayerAfterFrame()
    {
        // wait ONE frame so CharacterController finishes setup
        yield return null;

        if (ExitDoorTransition.nextSpawnIndex < spawnPoints.Length)
        {
            player.transform.position =
                spawnPoints[ExitDoorTransition.nextSpawnIndex].position;
        }
    }
}
