using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer buttonRenderer;
    public Sprite defaultSprite;
    public Sprite pressedSprite;

    public KeyCode assignedKey;

    // Start is called before the first frame update
    void Start()
    {
        buttonRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(assignedKey))
        {
            buttonRenderer.sprite = pressedSprite;
        }

        if (Input.GetKeyUp(assignedKey))
        {
            buttonRenderer.sprite = defaultSprite;
        }
    }
}
