using System;
using UnityEngine;
using UnityEngine.UI;

public class ArrowheadTrackMouse : MonoBehaviour
{
    RectTransform arrowheadRT;
    RectTransform arrowbodyRT;

    Image arrowheadImage;
    Image arrowbodyImage;

    void Awake()
    {
        RectTransform[] rects = GetComponentsInChildren<RectTransform>();
        arrowheadRT = Array.Find(rects, rect => rect.name == "Arrowhead");
        arrowbodyRT = Array.Find(rects, rect => rect.name == "Arrowbody");

        Image[] images = GetComponentsInChildren<Image>();
        arrowheadImage = Array.Find(images, image => image.name == "Arrowhead");
        arrowbodyImage = Array.Find(images, image => image.name == "Arrowbody");
    }

    void Update()
    {
        arrowheadImage.enabled = arrowbodyImage.enabled = GameState.state == GameState.GAMEPLAY && PlayerState.BoostInput();
        if (!arrowheadImage.enabled)
            return;

        arrowheadRT.position = PlayerState.clickPosition + (Vector3) PlayerState.BoostDirection() * Mathf.Min(PlayerState.DistanceFromMouseclick(), 80f);
        arrowbodyRT.sizeDelta = new Vector2(10f, Mathf.Min(PlayerState.DistanceFromMouseclick(), 80f));

        Vector2 direction = PlayerState.BoostDirection();
        float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        if (direction.x > 0)
            angle = -angle;
        arrowheadRT.eulerAngles = new Vector3(0, 0, angle);
    }
}
