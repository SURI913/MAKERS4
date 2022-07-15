using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public GameObject BulletPrefab;
    public double maxShotDelay;//제한 속도
    public float curShotDelay;//제한 속도
    public float BulletSpeed;
    private Rigidbody2D rigid;
	public int maxHealth;
	public int curHealth;

    void Start() {
        maxShotDelay = 0.7f;
        BulletSpeed = 0.2f;
    }
    
    void Update() {
        fire();
        Reload();
    }

    void fire() {
        if (curShotDelay < maxShotDelay)
            return;
        
        GameObject Bullet = Instantiate(BulletPrefab);
        Bullet.transform.position = transform.position;//총알의 위치를 플레이어와 함께 움직인다.
        Bullet.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        //Bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * BulletSpeed);
        Rigidbody2D rigid = Bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.right * 150);
        curShotDelay = 0;
    }

    void Reload() {
        curShotDelay += Time.deltaTime;
    }
}
