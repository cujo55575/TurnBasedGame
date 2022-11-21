using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedTrail : MonoBehaviour
{    // Start is called before the first frame update
    void Start()
    {
    }

    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector3 mousePos;
    private Vector3 objPos;
    private float angle;

    private void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane + 89.7f;
        //transform.position = Camera.main.ScreenToWorldPoint(mousePos);

        objPos = Camera.main.WorldToScreenPoint(spriteRenderer.transform.position);

        mousePos.x = mousePos.x - objPos.x;
        mousePos.y = mousePos.y - objPos.y;

        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        spriteRenderer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

        float scale = Mathf.Abs(Vector2.Distance(mousePos, this.transform.position));

        spriteRenderer.size = new Vector2(spriteRenderer.size.x, scale / 18);
    }
}
