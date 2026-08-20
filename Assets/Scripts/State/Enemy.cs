using UnityEngine;
using System.Collections;

namespace StatePattern {
	public abstract class Enemy {
		
		protected Transform enemy;
		protected Transform BulletEmiter;

		int attackRepeatTime = 1;
		private float attackTime = Time.time;
	
		protected int type;
		public virtual int getType() {
			return type;
		}

		protected enum EnemyStates {
			Stay,
			Chase,
			Attack,
			Retreat,
			Find,
			Bezier
		}

		public Transform getEnemy() {
			return enemy;
		}

		public float moveSpeed = 5f;
		public float Damping = 6f;

		public virtual void UpdateEnemy(Transform player, float health) { }

		protected void DoAction(Transform player, EnemyStates enemyMode) {

			float retreatSpeed = 5f;
			float chaseSpeed = 1f;
			float attackSpeed = 5f;

			switch (enemyMode) {
				
			case EnemyStates.Attack: 
				{
					//Attack player
					if (Time.time > attackTime) {
						/* Debug.Log ("The enemy is attacking"); */
						attackTime = Time.time + attackRepeatTime;

						BulletEmiter = enemy.FindChild ("BulletEmitter").transform;
						float Bullet_Forward_Force = 1000f;
						GameObject obj = ObjectPoolerScript.current.GetPooledObject ();
						if (obj == null)
							return;
						obj.transform.position = BulletEmiter.position;
						obj.transform.rotation = BulletEmiter.rotation;
						obj.transform.Rotate (Vector3.left * 90);
						obj.SetActive (true);
						GameObject TemporaryBulletHandler = obj;
						Rigidbody Temporary_Rigidbody;
						Temporary_Rigidbody = TemporaryBulletHandler.GetComponent<Rigidbody> ();
						Temporary_Rigidbody.AddForce (BulletEmiter.forward * Bullet_Forward_Force);
					}
					break;
				}
			case EnemyStates.Retreat: //Move away from player
				{
					/* Debug.Log ("The enemy is retreating"); */
					//Look in the opposite direction
					enemy.rotation = Quaternion.LookRotation(enemy.position - player.position);
					enemy.Translate(enemy.forward * retreatSpeed * Time.deltaTime);
					break;
				}
			case EnemyStates.Stay:
				{
					enemy.rotation = Quaternion.LookRotation(enemy.position - player.position);
					enemy.Translate(new Vector3(0,0,0));
					/* Debug.Log ("The enemy is staying"); */
					break;
				}
			case EnemyStates.Chase:
				{
					/* Debug.Log ("The enemy is chasing"); */
					Quaternion rotation = Quaternion.LookRotation (player.position - enemy.position);
					enemy.rotation = Quaternion.Slerp (enemy.rotation, rotation, Time.deltaTime * Damping);
					enemy.position += enemy.forward * moveSpeed * Time.deltaTime;
					break;
				}
			case EnemyStates.Find:
				{
					/* Debug.Log ("The enemy is trying to find me"); */
					break;
				}
			case EnemyStates.Bezier:
				{
					/* Debug.Log ("The enemy is trying to Bezier me"); */
					break;
				}

			}
		
		}



	}
}
