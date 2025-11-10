using _GAME.Scripts;
using UnityEngine;
using DialogueEditor;

public class GuidelineAndDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [Tooltip("Kéo GameObject chứa hội thoại của Chi vào đây.")]
    public NPCConversation conversationToStart;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;

            Debug.Log("Trigger cho Chi đã được kích hoạt!");
            GuidelineManager.Instance.ShowNextGuideline();

            if (conversationToStart != null)
            {
                ConversationManager.Instance.StartConversation(conversationToStart);
            }
            else
            {
                Debug.LogWarning("Chưa gán hội thoại (Conversation) cho trigger này!", this.gameObject);
            }

            gameObject.SetActive(false);
        }
    }
}