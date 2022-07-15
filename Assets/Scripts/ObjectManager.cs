//오브젝트 풀 관리

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {
	public Transform monsterGroup;
	public GameObject monsterS1Prefab_A;
	public GameObject monsterS1Prefab_B;
	public GameObject monsterS1Prefab_Boss;
	GameObject[] monsterS1_A;
	GameObject[] monsterS1_B;
	GameObject[] monsterS1_Boss;
	GameObject[] targetPool;

	void Awake() {
		monsterS1_A = new GameObject[20];
		monsterS1_B = new GameObject[15];
		monsterS1_Boss = new GameObject[2];

		Generate();
	}

	void Generate() {
		//적 생성 및 비활성화
		for (int index = 0; index < monsterS1_A.Length; index++) {
			monsterS1_A[index] = Instantiate(monsterS1Prefab_A, monsterGroup);
			monsterS1_A[index].SetActive(false);
		}
		for (int index = 0; index < monsterS1_B.Length; index++) {
			monsterS1_B[index] = Instantiate(monsterS1Prefab_B, monsterGroup);
			monsterS1_B[index].SetActive(false);
		}
		for (int index = 0; index < monsterS1_Boss.Length; index++) {
			monsterS1_Boss[index] = Instantiate(monsterS1Prefab_Boss, monsterGroup);
			monsterS1_Boss[index].SetActive(false);
		}

	}

	public GameObject MakeMonster(string type) {
		//생성 몬스터 반환 및 활성화
		switch (type) {
			case "MonsterS1_A":
				targetPool = monsterS1_A;
				break;
			case "MonsterS1_B":
				targetPool = monsterS1_B;
				break;
			case "MonsterS1_Boss":
				targetPool = monsterS1_Boss;
				break;
		}

		for (int index = 0; index < targetPool.Length; index++)
			if (!targetPool[index].activeSelf) {
				targetPool[index].SetActive(true);
				return targetPool[index];
			}

		return null;
	}

	public GameObject[] GetPool(string type) {
		//생성 몬스터 반환 및 활성화
		switch (type) {
			case "MonsterS1_A":
				targetPool = monsterS1_A;
				break;
			case "MonsterS1_B":
				targetPool = monsterS1_B;
				break;
			case "MonsterS1_Boss":
				targetPool = monsterS1_Boss;
				break;
		}

		return targetPool;
	}
}