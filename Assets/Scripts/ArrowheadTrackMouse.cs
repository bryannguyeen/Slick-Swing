using UnityEngine;
using UnityEngine.UI;

public class ArrowheadTrackMouse : MonoBehaviour
{
    RectTransform rt;
    public RectTransform arrowbodyRT;
    Image imageSprite;
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        imageSprite = GetComponent<Image>();
    }

    void Update()
    {
        imageSprite.enabled = GameState.state == GameState.GAMEPLAY && PlayerState.BoostInput();
        if (!imageSprite.enabled)
            return;

        rt.position = PlayerState.clickPosition + (Vector3) PlayerState.BoostDirection() * Mathf.Min(PlayerState.DistanceFromMouseclick(), 80f);
        arrowbodyRT.sizeDelta = new Vector2(5f, Mathf.Min(PlayerState.DistanceFromMouseclick(), 80f));
        Debug.Log(GetComponentInChildren<Image>());

        Vector2 direction = PlayerState.BoostDirection();
        float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        if (direction.x > 0)
            angle = -angle;
        rt.eulerAngles = new Vector3(0, 0, angle);
    }
}
