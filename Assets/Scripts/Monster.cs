//몬스터 정보 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
	public Player player;
	public GameManager gameManager;
	public ObjectManager objManager;
	public RectTransform monsterHealthGroupPrefab;
	RectTransform monsterHealthBar;

	public string monsterName;
	public float monsterSpeed;
	public int monsterMaxHealth;
	public int monsterCurHealth;
	public float maxShotDelay;
	public float curShotDelay;

	public int score;

	void OnEnable() {
		//활성화시 체력 초기화
		switch(monsterName) {
			case "S1_A":
				monsterCurHealth = 3;
				break;
			case "S1_B":
				monsterCurHealth = 10;
				break;
			case "S1_Boss":
				monsterCurHealth = 300;
				Invoke("BossStop", 7f);
				break;
		}
	}

	void BossStop() {
		Rigidbody2D rigid = GetComponent<Rigidbody2D>();
		rigid.velocity = Vector2.zero;
	}

	void Update() {
		Attack();
		Reload();
	}

	void Attack() {
		if (curShotDelay < maxShotDelay)
			return;

		if (monsterName == "S1_A") {
			//총알 발사 로직
		}

		curShotDelay = 0;
	}

	void Reload() {
		curShotDelay += Time.deltaTime;
	}

	public void OnDamaged(int dmg) {
		if (monsterCurHealth <= 0)	//이미 죽은 상태면 리턴
			return;

		monsterCurHealth -= dmg;
		
		if (monsterCurHealth <= 0) {   //피해를 받고 죽었다면
			gameObject.SetActive(false);
			transform.rotation = Quaternion.identity;

			if (monsterName == "S1_Boss") {
				gameManager.StageEnd();
				return;
			}

			if (gameManager.curKillNum < gameManager.maxKillNum)	//아직 스테이지 목표치를 못채웠다면
				gameManager.curKillNum++;	//처치 값 증가
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Border") {
			gameObject.SetActive(false);
			transform.rotation = Quaternion.identity;
		}
	}
}