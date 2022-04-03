using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    public float rotationSpeed;
    private Vector2 direction;
    public float moveSpeed;
    [SerializeField] Transform pos;

    private void Update()
    {
        direction =  /*Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;*/ pos.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        Vector2 cursorpos = /*Camera.main.ScreenToWorldPoint(Input.mousePosition);*/ pos.position;
        transform.position = Vector2.MoveTowards(transform.position, cursorpos, moveSpeed * Time.deltaTime);
    }
}
