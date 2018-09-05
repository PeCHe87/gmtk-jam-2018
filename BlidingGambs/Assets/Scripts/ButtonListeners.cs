using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ButtonListeners : MonoBehaviour
{
    public Button _continue;
    public Button _link;
    public Image _splash;

    void Start()
    {
        _continue.onClick.AddListener(Continue);
        _continue.gameObject.SetActive(false);
        _link.onClick.AddListener(OpenLink);
    }

    void Continue()
    {
        _continue.GetComponent<Image>().enabled = false;
        _link.GetComponent<Image>().enabled = false;
        _splash.enabled = true;

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