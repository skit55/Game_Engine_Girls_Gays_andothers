using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject landingScreen;
    [SerializeField] private GameObject introScreen;

    [Header("Intro UI")]
    [SerializeField] private TMP_Text introTitle;
    [SerializeField] private TMP_Text introBody;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMP_Text nextButtonLabel;

    [TextArea(5, 12)]
    [SerializeField] private string[] pages;

    private int pageIndex;

    private void Awake()
    {
        // Safety defaults
        if (introScreen != null) introScreen.SetActive(false);
        pageIndex = 0;

        // If you want, you can auto-fill a default pages array in Inspector instead.
        if (pages == null || pages.Length == 0)
        {
            pages = new string[]
            {
                "OBJECTIVE:\nRestore the broken world by completing 3 levels...",
                "CONTROLS:\nWASD to move...\nE to interact...",
                "TIPS:\nKeep substance above 50%...\nPractice rhythm timing early..."
            };
        }
    }

    public void OnStartClicked()
    {
        if (landingScreen == null || introScreen == null)
        {
            Debug.LogError("Assign landingScreen and introScreen in the Inspector.");
            return;
        }

        landingScreen.SetActive(false);
        introScreen.SetActive(true);

        pageIndex = 0;
        RefreshIntro();
    }

    public void OnNextClicked()
    {
        pageIndex++;

        if (pageIndex >= pages.Length)
        {
            // End of intro flow – choose what happens next
            Debug.Log("Intro finished – start game next");
            // Example actions:
            // LoadScene("GameScene");
            SceneManager.LoadScene("Core");
            // or introScreen.SetActive(false);
            // or keep intro visible and change UI
            return;
        }

        RefreshIntro();
    }

    public void OnBackFromIntro()
    {
        introScreen.SetActive(false);
        landingScreen.SetActive(true);
        pageIndex = 0;
    }

    private void RefreshIntro()
    {
        if (introBody != null)
            introBody.text = pages[pageIndex];

        // Optional: update title per page
        if (introTitle != null)
        {
            // You can customize titles per page if you want
            // introTitle.text = pageIndex == 0 ? "OBJECTIVE" : pageIndex == 1 ? "CONTROLS" : "TIPS";
        }

        // Button label changes on last pfage
        if (nextButtonLabel != null && pages.Length > 0)
        {
            bool lastPage = (pageIndex == pages.Length - 1);
            nextButtonLabel.text = lastPage ? "START GAME" : "NEXT";

        }
    }
}
