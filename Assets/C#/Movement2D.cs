using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       


    }

    // Update is called once per frame
    void Update()
    {
        //위쪽 화살표가 눌렸을 때
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, 0.01f, 0);    //위로 [3] 움직인다.
        }
        //아래쪽 화살표가 눌렸을 때
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, -0.01f, 0);   //아래로 [3] 움직인다.
        }



        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        //화면 밖으로 나가지않는다.
        if (pos.x < 0.04f) pos.x = 0.04f;
        if (pos.x > 0.96f) pos.x = 0.96f;
        if (pos.y < 0.15f) pos.y = 0.15f;
        if (pos.y > 0.9f) pos.y = 0.9f;

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
