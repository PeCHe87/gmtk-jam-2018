using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ButtonListeners : MonoBehaviour
{
    public Button _continue;
    public Button _link;
    public Image splash;

    void Start()
    {
        _continue.onClick.AddListener(Continue);
        _link.onClick.AddListener(OpenLink);
    }

    void Continue()
    {
        splash.enabled = true;

        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Main");
    }

    void OpenLink()
    {
        Application.OpenURL("https://www.facebook.com/SomosRANDOMGames/");
    }
}