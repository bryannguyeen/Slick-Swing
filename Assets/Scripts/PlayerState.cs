using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public const int BEGINNING = 0;
    public const int FREEFALL = 1;
    public const int CASTING = 2;
    public const int SWINGING = 3;

    private static int state;
    public static bool canBoost;


    void Start()
    {
        state = BEGINNING;
        canBoost = true;
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
