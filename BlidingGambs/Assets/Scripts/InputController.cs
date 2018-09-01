using UnityEngine;

public class InputController : MonoBehaviour
{
    public static System.Action OnActionKeyPressed;

    [SerializeField] private KeyCode _actionKey;

    private void Update()
    {
        if (Input.GetKeyDown(_actionKey))
        {
            if (OnActionKeyPressed != null)
                OnActionKeyPressed();
        }
    }
}