using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public  SceneTransitionManager transitionManager;
    private bool                   hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            transitionManager.StartTransition();
        }
    }
}