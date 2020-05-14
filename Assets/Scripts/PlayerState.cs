using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public const int BEGINNING = 0;
    public const int FREEFALL = 1;
    public const int CASTING = 2;
    public const int SWINGING = 3;

    private static int state;
    public static bool canBoost;

    public static bool mouseClick;    // is true only if the mouse is clicked on current frame
    public static bool mouseRelease;  // is true only if the mouse is released on current frame
    public static bool mouseHold;     // is true as long as the mouse is pressed, false otherwise


    void Start()
    {
        state = BEGINNING;
        canBoost = true;

        mouseClick = false;
        mouseRelease = false;
        mouseHold = false;
    }

    void Update()
    {
        if (GameState.state == GameState.GAMEPLAY)
        {
            CheckForMouseClick();
            CheckForMouseRelease();
        }
    }

    void CheckForMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseClick = true;
            mouseHold = true;
        }
    }

    void CheckForMouseRelease()
    {
        if (Input.GetMouseButtonUp(0) && mouseHold)
        {
            mouseRelease = true;
            mouseHold = false;
        }
    }

    public static int getState()
    {
        return state;
    }

    public static bool isStarting()
    {
        return state == BEGINNING;
    }

    public static bool isFreefall()
    {
        return state == FREEFALL;
    }

    public static bool isCasting()
    {
        return state == CASTING;
    }

    public static bool isSwinging()
    {
        return state == SWINGING;
    }

    public static void setToFreefall()
    {
        state = FREEFALL;
    }

    public static void setToCasting()
    {
        state = CASTING;
    }

    public static void setToSwinging()
    {
        state = SWINGING;
    }
}
