using System;
using UnityEngine;
using UnityEngine.UI;

public class BoostIndicator : MonoBehaviour
{
    public Sprite enabledSprite;
    public Sprite disabledSprite;

    Transform arrowheadTf;
    Transform guyTf;

    SpriteRenderer indicatorSprite;
    void Awake()
    {
        Transform[] transforms = GetComponentsInParent<Transform>();
        arrowheadTf = Array.Find(transforms, tf => tf.name == "Boost Indicator");
        guyTf = Array.Find(transforms, tf => tf.name == "Guy");

        indicatorSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        indicatorSprite.enabled = GameState.state == GameState.GAMEPLAY && PlayerState.IsSwinging() && PlayerState.DistanceFromMouseclick() > PlayerState.minFingerBoostDistance;
        if (!indicatorSprite.enabled)
            return;

        if (PlayerState.CanBoost())
            indicatorSprite.sprite = enabledSprite;
        else
            indicatorSprite.sprite = disabledSprite;

        Vector2 position = guyTf.position + ((Vector3)PlayerState.BoostDirection()) * Mathf.Min(PlayerState.DistanceFromMouseclick() * 0.025f, 3f);

        Vector2 direction = PlayerState.BoostDirection();
        float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        if (direction.x > 0)
            angle = -angle;

        arrowheadTf.position = position;
        arrowheadTf.eulerAngles = new Vector3(0, 0, angle);
    }
}
