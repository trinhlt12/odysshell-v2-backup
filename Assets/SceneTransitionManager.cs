using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneTransitionManager : MonoBehaviour
{
    public Image fadeImage;
    public GameObject confirmationPanel;
    public string sceneToLoad;
    public float fadeDuration = 2f;

    void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.color = new Color(1f, 1f, 1f, 0f);
            fadeImage.transform.localScale = Vector3.zero;
            fadeImage.gameObject.SetActive(false);
        }
    }

    public void StartTransition()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
        }
        fadeImage.transform.DOScale(Vector3.one * 5f, fadeDuration).SetEase(Ease.OutQuad);

        fadeImage.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad)
            .OnComplete(() => {
                if (confirmationPanel != null)
                {
                    confirmationPanel.SetActive(true);
                }
            });
    }


    public void OnYesButtonClicked()
    {
        confirmationPanel.SetActive(false);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnNoButtonClicked()
    {
        Debug.Log("Application Quit!");
        Application.Quit();
    }
}