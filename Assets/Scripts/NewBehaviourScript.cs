using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    private float RotateSpeed = 1f;
    private float Radius = 10f;

    private Vector2 _centre;
    private float _angle;

    private void Start()
    {
        _centre = transform.position;
    }

    private void Update()
    {

        _angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
        transform.position = _centre + offset;
    }

    /*public void SetPoint()
    {
        Vector2 posPoto = new Vector2 (transform.localPosition.x + xPoint, transform.localPosition.y + yPoint);
        Debug.Log("lama " + posPoto);
        Vector2 center = new Vector2(transform.localPosition.x , transform.localPosition.y );
        Debug.Log("alpaca " + center);

        float angle = Vector2.Angle(transform.localPosition, posPoto);
       // angle = angle * Mathf.Rad2Deg;
        Debug.Log("lamalpaca " + angle);

        pointToCircle.transform.localPosition = new Vector2(Mathf.Sin(angle) , Mathf.Cos(angle)) * 10f;
        //pointToCircle.transform.localPosition = new Vector2((xPoint * 10f), (yPoint * 10f));
    }*/
}
