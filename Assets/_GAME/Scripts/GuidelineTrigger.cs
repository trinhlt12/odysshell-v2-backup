namespace _GAME.Scripts
{
    using UnityEngine;

    public class GuidelineTrigger : MonoBehaviour
    {
        public bool alsoStartsDialogue = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player đã kích hoạt Guideline Trigger!");

                GuidelineManager.Instance.ShowNextGuideline();

                if (alsoStartsDialogue)
                {
                }

                gameObject.SetActive(false);
            }
        }
    }
}