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
        //���� ȭ��ǥ�� ������ ��
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, 0.01f, 0);    //���� [3] �����δ�.
        }
        //�Ʒ��� ȭ��ǥ�� ������ ��
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, -0.01f, 0);   //�Ʒ��� [3] �����δ�.
        }



        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        //ȭ�� ������ �������ʴ´�.
        if (pos.x < 0.04f) pos.x = 0.04f;
        if (pos.x > 0.96f) pos.x = 0.96f;
        if (pos.y < 0.15f) pos.y = 0.15f;
        if (pos.y > 0.9f) pos.y = 0.9f;

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
