using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public string doorID;
    public string targetSceneName;
    public string targetDoorID;
    public Transform spawnPoint;

    [Header("Visual Feedback")]
    public GameObject interactionHint;

    private bool playerInRange = false;

    void Start()
    {
        Debug.Log($"🚪 Дверь {doorID} инициализирована на сцене {gameObject.scene.name}");

        if (spawnPoint == null)
        {
            spawnPoint = transform.Find("SpawnPoint");
            if (spawnPoint == null)
            {
                GameObject spawnObj = new GameObject("SpawnPoint");
                spawnPoint = spawnObj.transform;
                spawnPoint.SetParent(transform);
                spawnPoint.localPosition = new Vector3(1.5f, 0, 0);
            }
        }

        if (interactionHint != null)
            interactionHint.SetActive(false);
    }

    void Update()
    {
        // ТОЛЬКО новая Input System
        if (playerInRange && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("🎮 F нажата через новую Input System");
            Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log($"✅ Игрок в зоне двери {doorID}");
            if (interactionHint != null)
                interactionHint.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log($"❌ Игрок вышел из зоны двери {doorID}");
            if (interactionHint != null)
                interactionHint.SetActive(false);
        }
    }

    void Interact()
    {
        Debug.Log($"🎯 Активация двери {doorID} -> {targetSceneName}");

        if (SceneTransitionManager.Instance != null)
        {
            Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
            SceneTransitionManager.Instance.TransitionToScene(targetSceneName, targetDoorID, spawnPosition);
        }
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPoint != null ? spawnPoint.position : transform.position;
    }
}