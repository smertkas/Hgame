using UnityEngine;

public class FrameSwitch : MonoBehaviour
{
    public GameObject activeFrame;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plat"))
        {
            activeFrame.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Plat"))
        {
            activeFrame.SetActive(false);
        }
    }
}
