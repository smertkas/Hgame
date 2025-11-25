using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    private Vector3 playerPosition;
    private string targetDoorID;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        MakeFadeImageTransparent();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MakeFadeImageTransparent();
        PlacePlayerAtDoor();
    }

    void MakeFadeImageTransparent()
    {
        Image fadeImage = GameObject.Find("FadeImage")?.GetComponent<Image>();
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    public void TransitionToScene(string sceneName, string doorID, Vector3 position)
    {
        targetDoorID = doorID;
        playerPosition = position;
        StartCoroutine(TransitionCoroutine(sceneName));
    }

    private IEnumerator TransitionCoroutine(string sceneName)
    {
        // Фаза 1: Затемнение
        Image fadeImage = GameObject.Find("FadeImage")?.GetComponent<Image>();

        if (fadeImage != null)
        {
            float timer = 0;
            while (timer < 0.8f)
            {
                timer += Time.deltaTime;
                fadeImage.color = new Color(0, 0, 0, timer / 0.8f);
                yield return null;
            }
            fadeImage.color = Color.black;
        }

        // Фаза 2: Загрузка сцены
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone) yield return null;

        yield return new WaitForSeconds(0.1f);

        // Фаза 3: Появление
        fadeImage = GameObject.Find("FadeImage")?.GetComponent<Image>();

        if (fadeImage != null)
        {
            float timer = 0;
            while (timer < 0.8f)
            {
                timer += Time.deltaTime;
                fadeImage.color = new Color(0, 0, 0, 1 - (timer / 0.8f));
                yield return null;
            }
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    private void PlacePlayerAtDoor()
    {
        Door[] allDoors = FindObjectsOfType<Door>();

        foreach (Door door in allDoors)
        {
            if (door.doorID == targetDoorID)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = door.GetSpawnPosition();
                }
                break;
            }
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}