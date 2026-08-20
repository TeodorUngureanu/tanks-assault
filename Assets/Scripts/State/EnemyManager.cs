using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StatePattern {

	public class EnemyManager : MonoBehaviour {
		public GameObject player;
		public GameObject enemyEasy;
		public GameObject enemyIntermediar;
		public GameObject enemyAdvanced;
		public Transform SpawnPoint;

		static List<Enemy> enemies = new List<Enemy> ();
		static List<Transform> enemiesTranform = new List<Transform> ();
		static List<float> enemiesHP = new List<float> ();

		public static EnemyManager instance;

		public static int numberOfEnemies = 0;
		public static int maxNumberOfEnemies = 1;

		private int maximumLevel;

		void Awake() {
			instance = this;
			if (player == null) {
				player = GameObject.FindGameObjectWithTag ("Player");
			}
			numberOfEnemies = 0;
		}

		private float enemyXP;
		enum EnemyType {
			Easy,
			Intermediar,
			Advanced
		}
		EnemyType enemyType = EnemyType.Easy;

		void ChooseEnemy() {
			int number = Random.Range (0, maximumLevel);
			number = 2;
			switch (number) {
			case 0:
				{
					enemyType = EnemyType.Easy;
					break;
				}
			case 1:
				{
					enemyType = EnemyType.Intermediar;
					break;
				}
			case 2:
				{
					enemyType = EnemyType.Advanced;
					break;
				}
			}

			AddEnemy ();
		}

		void AddEnemy() {
			if(numberOfEnemies < maxNumberOfEnemies) {
				numberOfEnemies++;
				switch (enemyType) {

				case EnemyType.Easy:
					{
						GameObject easyAI = Instantiate(enemyEasy, SpawnPoint.position, SpawnPoint.rotation) as GameObject;
						enemies.Add(new EasyAI(easyAI.transform));
						enemiesTranform.Add (easyAI.transform);
						enemiesHP.Add (100f);
						break;
					}

				case EnemyType.Intermediar:
					{
						GameObject intermediateAI = Instantiate(enemyIntermediar, SpawnPoint.position, SpawnPoint.rotation) as GameObject;
						enemies.Add(new IntermediateAI(intermediateAI.transform));
						enemiesTranform.Add (intermediateAI.transform);
						enemiesHP.Add (100f);
						break;
					}

				case EnemyType.Advanced:
					{
						GameObject advancedAI = Instantiate(enemyAdvanced, SpawnPoint.position, SpawnPoint.rotation) as GameObject;
						enemies.Add(new AdvancedAI(advancedAI.transform));
						enemiesTranform.Add (advancedAI.transform);
						enemiesHP.Add (100f);
						break;
					}
				}

			}

			Invoke("ChooseEnemy", 5f);
		}

		void Start() {
			maximumLevel = MainGameManager._maximumLevel;
			maximumLevel++;
			if (player != null) {
				ChooseEnemy ();
			} else {
				MethodToInvokeWhenNoPlayerWasInTheScene ();
			}
		}

		public void MethodToInvokeWhenNoPlayerWasInTheScene () {
			Invoke("TryToAddEnemyInScene", 5f);
		}

		public void TryToAddEnemyInScene() {
			if (player != null) {
				ChooseEnemy ();
			}
		}

		void Update() {
			//Update all enemies to see if they should change state
			if (player != null) {
				for (int i = 0; i < enemies.Count; i++) {
					enemies[i].UpdateEnemy(player.transform, enemiesHP[i]);
				}
			}
		}

		public static void changedHealthStatus(Transform obj, float health) {
			int enemyIndex = enemiesTranform.IndexOf (obj);
			if (enemyIndex >= 0) {
				if (health.Equals(0)) {
					int score = 100 * enemies[enemyIndex].getType();
					enemiesTranform.RemoveAt (enemyIndex);
					enemies.RemoveAt (enemyIndex);
					enemiesHP.RemoveAt (enemyIndex);
					numberOfEnemies--;

					MainGameManager.getInstance().AdjustScore(score);
				} else {
					enemiesHP.RemoveAt (enemyIndex);
					enemiesHP.Insert (enemyIndex, health);
				}
			}
		}

	}
}