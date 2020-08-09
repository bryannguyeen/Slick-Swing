using UnityEngine;

public class BlipPlayer : MonoBehaviour
{
    public void Play(Vector2 position)
    {
        transform.position = position;
        GetComponent<Animator>().SetTrigger("playBlip");
    }
}
