using UnityEngine;
using System.Collections;

namespace StatePattern {
	public class IntermediateAI : Enemy {

		EnemyStates intermediatAImode = EnemyStates.Find;

		public float findRange = 500f;
		public float chaseRange = 30f;
		public float attackRange = 20f;
		public float retreatHealth = 20f;
		public float minimumHealth = 60f;

		public IntermediateAI (Transform intermediateAI) {
			base.enemy = intermediateAI;
		}

		public override int getType() { 
			return 2;
		}

		public override void UpdateEnemy(Transform player, float health) {
			//check if player exists
			if (base.enemy == null) {
				return;
			}

			float distance = Vector3.Distance(base.enemy.position, player.position);

			switch (intermediatAImode) {

			case EnemyStates.Attack:
				{
					if (health < retreatHealth) {
						intermediatAImode = EnemyStates.Retreat;
					} else if (distance > 2f) {
						intermediatAImode = EnemyStates.Chase;
					}
					break;
				}

			case EnemyStates.Retreat:
				{
					if (health > minimumHealth) {
						intermediatAImode = EnemyStates.Stay;
					}
					break;
				}

			case EnemyStates.Stay:
				{
					if (distance < chaseRange) {
						intermediatAImode = EnemyStates.Chase;
					} else if (distance < findRange) {
						intermediatAImode = EnemyStates.Find;
					}
					break;
				}

			case EnemyStates.Chase:
				{
					if (distance < attackRange) {
						intermediatAImode = EnemyStates.Attack;
					} else if (distance > chaseRange) {
						intermediatAImode = EnemyStates.Stay;
					}
					break;
				}

			case EnemyStates.Find:
				{
					if (distance < attackRange) {
						intermediatAImode = EnemyStates.Attack;
					} else if (distance < chaseRange) {
						intermediatAImode = EnemyStates.Chase;
					} else if (distance > findRange) {
						intermediatAImode = EnemyStates.Stay;
					}
					break;
				}

			}

			DoAction(player, intermediatAImode);

		}

	}
}