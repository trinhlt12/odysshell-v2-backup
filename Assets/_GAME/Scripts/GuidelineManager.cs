namespace _GAME.Scripts
{
    using UnityEngine;
    using TMPro;
    using System.Collections.Generic;
    using DG.Tweening;

    public class GuidelineManager : MonoBehaviour
    {
        public static GuidelineManager Instance { get; private set; }

        [Header("UI References")] public TextMeshProUGUI guidelineTextUI;

        [Header("Settings")] public float fadeDuration = 1.0f;

        [Header("Guideline Steps")] public List<GuidelineStep> guidelineSteps;

        private int currentStepIndex = -1;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            ShowNextGuideline();
        }

        public void ShowNextGuideline()
        {
            currentStepIndex++;

            if (currentStepIndex >= guidelineSteps.Count)
            {
                Debug.Log("Đã hoàn thành tất cả các chỉ dẫn.");
                guidelineTextUI.DOFade(0, fadeDuration);
                return;
            }

            StartCoroutine(FadeAndUpdateText());
        }

        private System.Collections.IEnumerator FadeAndUpdateText()
        {
            yield return guidelineTextUI.DOFade(0, fadeDuration).WaitForCompletion();

            guidelineTextUI.text = guidelineSteps[currentStepIndex].guidelineText;

            yield return guidelineTextUI.DOFade(1, fadeDuration).WaitForCompletion();
        }
    }
}