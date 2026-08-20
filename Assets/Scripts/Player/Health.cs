using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace StatePattern {

	public class Health : MonoBehaviour {

		ConcreteSubject s;

		public Slider healthUI;

		public float startingHealth = 100f;
		public float currentHealth;
		private bool dead;


		void Start() {
			if (this.tag == "Player") {
				startingHealth = MainGameManager._currentHealth;
			}
			currentHealth = startingHealth;
			if (healthUI != null) {
				healthUI.value = startingHealth;
			}
		}

		private void OnCollisionEnter(Collision collision) {

			bool forwardBulletMovement = true;
			if (Vector3.Angle(transform.forward, collision.transform.position - transform.position) > 10f) {
				forwardBulletMovement = false;
			}

			if ((collision.transform.tag == "Projectile") && !(forwardBulletMovement)) {
				collision.transform.gameObject.SetActive (false);
				currentHealth -= 20f;
				if (healthUI != null) {
					healthUI.value = currentHealth;
				}
				MainGameManager.getInstance ().AdjustEnemyXP (50);
				if (currentHealth <= 0f && !dead) {
					OnDeath ();
				}
			}

			if ((collision.transform.tag == "Bullet") && !(forwardBulletMovement)) {
				if (transform.tag == "Player") {
					currentHealth -= 10f;
					if (healthUI != null) {
						healthUI.value = currentHealth;
					}
					MainGameManager.getInstance ().AdjustEnemyXP (10);
					MainGameManager.getInstance ().AdjustHealth (10);
				} else {
					currentHealth -= 10f;
					s.setSubjectState(currentHealth);
					s.Notify();
				}
				if (currentHealth <= 0f && !dead) {
					OnDeath ();
					if (transform.tag == "Enemy") {
						CancelInvoke ("SetPassedFalse");
					}
				}

			}
		}

		private void OnEnable() {
			currentHealth = startingHealth;
			dead = false;
			s = new ConcreteSubject();
			s.Attach(new ObserverManager(s, "HealthObserver", this.transform));
			IncreaseHealth ();
		}

		bool passed = true;

		void Update() {
			if (transform.tag == "Enemy") {
				if (passed == false) {
					IncreaseHealth();
					passed = true;
				}
			}
		}

		private void IncreaseHealth() {
			if (currentHealth < 95f && currentHealth >= 5f) {
				currentHealth += 5f;
			} else {
				currentHealth = 100f;
			}
			s.setSubjectState(currentHealth);
			s.Notify();
			Invoke ("SetPassedFalse", 5f);
		}

		private void SetPassedFalse() {
			passed = false;
		}

		private void OnDeath() {
			dead = true;
			gameObject.SetActive (false);
			if (gameObject.transform.tag == "Player") {
				LevelManager.setGameOverToTrue = true;
			}
		}

		void OnGUI() {
			if (this.tag == "Enemy" && GameObject.FindGameObjectWithTag("Player")!=null) {
				Vector3 coords = Camera.main.WorldToScreenPoint (transform.position);
				GUIStyle style = new GUIStyle (GUI.skin.textArea);
				style.fontSize = 12;
				style.alignment = TextAnchor.MiddleCenter;
				style.normal.textColor = Color.white;
				style.normal.background = new Texture2D (1, 1);
				GUI.backgroundColor = Color.cyan;
				if (currentHealth <= 10) {
					GUI.backgroundColor = Color.red;
				}
				GUI.TextArea (new Rect (coords.x - 30, Screen.height - coords.y - 40, 60, 30), currentHealth + "/" + startingHealth, style);
			}
		}

	}

}