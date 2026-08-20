using UnityEngine;
using System.Collections;

namespace StatePattern {
	public class EasyAI : Enemy {

		EnemyStates easyAImode = EnemyStates.Stay;

		public float chaseRange = 30f;
		public float attackRange = 20f;
		public float retreatHealth = 20f;
		public float minimumHealth = 60f;

		public EasyAI (Transform easyAI) {
			base.enemy = easyAI;
		}

		public override int getType() { 
			return 1;
		}

		//Update the easyAI's state
		public override void UpdateEnemy(Transform player, float health) {

			if (base.enemy == null) {
				return;
			}

			float distance = Vector3.Distance(base.enemy.position, player.position);

			switch (easyAImode) {

			case EnemyStates.Attack:
				{
					if (health < retreatHealth) {
						easyAImode = EnemyStates.Retreat;
					} else if (distance > 2f) {
						easyAImode = EnemyStates.Chase;
					}
					break;
				}

			case EnemyStates.Retreat:
				{
					/* Debug.Log ("The Enemy is retreating " + health); */
					if (health > retreatHealth) {
						easyAImode = EnemyStates.Stay;
					}
					break;
				}

			case EnemyStates.Stay:
				{
					if (distance < chaseRange) {
						easyAImode = EnemyStates.Chase;
					}
					break;
				}

			case EnemyStates.Chase:
				{
					if (distance < attackRange) {
						easyAImode = EnemyStates.Attack;
					} else if (distance > chaseRange) {
						easyAImode = EnemyStates.Stay;
					}
					break;
				}

			}

			//Move the enemy based on a state
			DoAction(player, easyAImode);

		}

	}
}