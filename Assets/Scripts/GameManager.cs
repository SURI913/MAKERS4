//게임 진행 관리 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;	//파일 읽기 namespace

public class GameManager : MonoBehaviour {
	[Header("[ Core ]")]
	public ObjectManager objManager;
	public Player player;

	[Header("[ Spawn Monster ]")]
	public Transform[] spawnPoints; //생성 포인트
	public float nextSpawnDelay;	//다음 패턴 딜레이 값
	public float curSpawnDelay;	//현재 패턴 딜레이 값
	public List<Spawn> spawnList;	//생성 정보 리스트
	public int spawnIndex;	//생성 인덱스
	public bool spawnEnd;	//생성 종료 알림
	string[] monsterObjs;   //몬스터 종류 저장

	[Header("[ Stage ]")]
	public int stageNum;	//현재 스테이지 넘버
	public int maxKillNum;	//스테이지 목표 처치 수
	public int curKillNum;  //현재 처치 수
	bool isSpawnBoss;   //보스 소환 여부

	[Header("[ UI ]")]
	public RectTransform playerHealthGroup;
	public RectTransform playerHealthBar;
	public RectTransform bossHealthGroup;
	public RectTransform bossHealthBar;
	public RectTransform monsterHealthGroupPrefab;
	Monster bossLogic;

	void Awake() {
		monsterObjs = new string[] { "MonsterS1_A", "MonsterS1_B", "MonsterS1_Boss" };
		spawnList = new List<Spawn>();
		ReadSpawnFile();
		StageStart();
	}

	public void StageStart() {
		curKillNum = 0;

		switch(stageNum) {
			case 1:
				maxKillNum = 15;
				break;
			case 2:
				maxKillNum = 20;
				break;
			case 3:
				maxKillNum = 25;
				break;
			case 4:
				maxKillNum = 30;
				break;
			case 5:
				maxKillNum = 40;
				break;
		}
	}

	public void StageEnd() {
		if (stageNum < 5) {
			stageNum++;
			Invoke("StageStart", 8f);
			return;
		}

		Invoke("GameOver", 4f);
	}

	void GameOver() {
		//게임 종료 함수
	}

	void ReadSpawnFile() {
		//변수 초기화
		spawnList.Clear();
		spawnIndex = 0;
		spawnEnd = false;

		//텍스트 파일 읽기
		TextAsset textFile = Resources.Load("Stage 1") as TextAsset;
		StringReader strReader = new StringReader(textFile.text);
		while (strReader != null) {
			string line = strReader.ReadLine();

			if (line == null)
				break;

			//스폰 데이터 생성 및 저장
			Spawn spawnData = new Spawn();
			spawnData.delay = float.Parse(line.Split(',')[0]);
			spawnData.type = line.Split(',')[1];
			spawnData.point = int.Parse(line.Split(',')[2]);
			spawnList.Add(spawnData);
		}

		//텍스트 파일 닫기
		strReader.Close();

		//첫번째 스폰 딜레이 적용
		nextSpawnDelay = spawnList[0].delay;
	}

	void Update() {
		if (isSpawnBoss)	//보스가 생성되어있다면 딜레이 갱신 안함
			return;

		if (maxKillNum == curKillNum)   //목표치를 모두 채우고, 보스가 소환되지 않았다면
			DestroyMonsters();

		if (maxKillNum != curKillNum && spawnEnd)	//목표를 못채우고 스폰 패턴이 끝나면
			ReadSpawnFile();    //다시 패턴 시작

		//패턴 딜레이 갱신
		curSpawnDelay += Time.deltaTime;

		if (curSpawnDelay > nextSpawnDelay && !spawnEnd) {	//딜레이 시간이 만족되면
			SpawnMonster();	//몬스터 생성
			curSpawnDelay = 0;	//현재 딜레이 값 초기화
		}
	}
	
	void DestroyMonsters() {
		//남은 모든 적 처치
		GameObject[] monsterS1_A = objManager.GetPool("MonsterS1_A");
		GameObject[] monsterS1_B = objManager.GetPool("MonsterS1_B");
		for (int index = 0; index < monsterS1_A.Length; index++)
			if (monsterS1_A[index].activeSelf) {
				Monster monsterLogic = monsterS1_A[index].GetComponent<Monster>();
				monsterLogic.OnDamaged(1000);
			}
		for (int index = 0; index < monsterS1_B.Length; index++)
			if (monsterS1_B[index].activeSelf) {
				Monster monsterLogic = monsterS1_B[index].GetComponent<Monster>();
				monsterLogic.OnDamaged(1000);
			}

		//처치 후 보스 소환
		isSpawnBoss = true;
		SpawnBoss();
	}

	void SpawnMonster() {
		int monsterIndex = 0;	//몬스터 타입 지정
		switch(spawnList[spawnIndex].type) {
			case "S1_A":
				monsterIndex = 0;
				break;
			case "S1_B":
				monsterIndex = 1;
				break;
		}

		int spawnPoint = spawnList[spawnIndex].point;	//스폰 포인트 저장
		GameObject monster = objManager.MakeMonster(monsterObjs[monsterIndex]);	//몬스터 불러오기
		monster.transform.position = spawnPoints[spawnPoint].position;  //몬스터 생성 위치 설정
		Rigidbody2D rigid = monster.GetComponent<Rigidbody2D>();
		Monster monsterLogic = monster.GetComponent<Monster>();
		monsterLogic.player = player;   //플레이어 넘겨주기
		monsterLogic.objManager = objManager;   //오브젝트 매니저 넘겨주기(발사체 등)
		monsterLogic.gameManager = this;    //게임 매니저 넘겨주기
		monsterLogic.monsterHealthGroupPrefab = monsterHealthGroupPrefab;	//몬스터 체력바 넘겨주기

		rigid.velocity = new Vector2(monsterLogic.monsterSpeed * (-1), 0);  //몬스터 이동

		spawnIndex++;
		if (spawnIndex == spawnList.Count) {	//마지막 몬스터 스폰 후
			spawnEnd = true;	//스폰 종료 true로 변경
			return;
		}

		//다음 스폰 딜레이 갱신
		nextSpawnDelay = spawnList[spawnIndex].delay;
	}

	void SpawnBoss() {
		GameObject boss = objManager.MakeMonster("MonsterS1_Boss");
		boss.transform.position = spawnPoints[1].position;
		Rigidbody2D rigid = boss.GetComponent<Rigidbody2D>();
		bossLogic = boss.GetComponent<Monster>();
		bossLogic.player = player;
		bossLogic.objManager = objManager;
		bossLogic.gameManager = this;
		rigid.velocity = new Vector2(bossLogic.monsterSpeed * (-1), 0);
	}

	void LateUpdate() {
		playerHealthBar.localScale = new Vector3((float)player.curHealth / player.maxHealth, 1, 1);
		if (isSpawnBoss) {
			bossHealthGroup.anchoredPosition = Vector3.down * 10;
			bossHealthBar.localScale = new Vector3((float)bossLogic.monsterCurHealth / bossLogic.monsterMaxHealth, 1, 1);
		}
		else
			bossHealthGroup.anchoredPosition = Vector3.up * 200;
	}
}