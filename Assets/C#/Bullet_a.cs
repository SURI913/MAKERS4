using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_a : MonoBehaviour
{


    public ParticleSystem explosionParticle;
    public float moveSpeed = 5.0f;

    private Rigidbody2D rigid;
    public float BulletSpeed;

    private void Start()
    {
       
        rigid = GetComponent<Rigidbody2D>();
        var rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(Vector2.right * BulletSpeed, ForceMode2D.Impulse);
    }
    private void Update()
    {
       float moveX = moveSpeed * Time.deltaTime;
        transform.Translate(moveX, 0, 0);
    }



    private void OnCollisionEnter2D(Collision2D other) //���� �۵��� ����
    {
        if(other.gameObject.name == "wall")
        {
            //Set Active off
            Destroy(gameObject);
        }
        if (other.gameObject.name == "monster")
        {


            //Take Damage:�Ŀ� �߰�

            //Set Active off
            Destroy(GameObject.Find("bullet1(Clone)"));
        }
    }

}


