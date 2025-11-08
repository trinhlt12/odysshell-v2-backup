using UnityEngine;
using DialogueEditor;

public class DialogueTrigger : MonoBehaviour
{
    public NPCConversation conversation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ConversationManager.Instance.StartConversation(conversation);

            gameObject.SetActive(false);
        }
    }
}