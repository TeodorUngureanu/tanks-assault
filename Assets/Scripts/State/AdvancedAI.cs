using UnityEngine;
using System.Collections;

namespace StatePattern {
	public class AdvancedAI : Enemy {

		EnemyStates advancedAImode = EnemyStates.Bezier;

		public float findRange = 50f;
		public float chaseRange = 30f;
		public float attackRange = 20f;
		public float retreatHealth = 20f;
		public float minimumHealth = 60f;

		public AdvancedAI (Transform advancedAI) {
			base.enemy = advancedAI;
		}

		public override int getType() { 
			return 4;
		}

		public override void UpdateEnemy(Transform player, float health) {

			if (base.enemy == null) {
				return;
			}

			float distance = Vector3.Distance(base.enemy.position, player.position);

			switch (advancedAImode) {

			case EnemyStates.Attack:
				{
					if (health < retreatHealth) {
						advancedAImode = EnemyStates.Retreat;
					} else if (distance > 2f) {
						advancedAImode = EnemyStates.Chase;
					}
					break;
				}

			case EnemyStates.Retreat:
				{
					if (health > minimumHealth) {
						advancedAImode = EnemyStates.Stay;
					}
					break;
				}

			case EnemyStates.Stay:
				{
					if (distance < chaseRange) {
						advancedAImode = EnemyStates.Chase;
					} else if (distance < findRange) {
						advancedAImode = EnemyStates.Find;
					}
					break;
				}

			case EnemyStates.Chase:
				{
					if (distance < attackRange) {
						advancedAImode = EnemyStates.Attack;
					} else if (distance > chaseRange) {
						advancedAImode = EnemyStates.Stay;
					}
					break;
				}

			case EnemyStates.Find:
				{
					int randomBezier = Random.Range (0, 2);
					if (randomBezier == 1) {
						advancedAImode = EnemyStates.Bezier;
					} else {
						if (distance < attackRange) {
							advancedAImode = EnemyStates.Attack;
						} else if (distance < chaseRange) {
							advancedAImode = EnemyStates.Chase;
						} else if (distance > findRange) {
							advancedAImode = EnemyStates.Stay;
						}
					}
					break;
				}
			case EnemyStates.Bezier:
				{
					int randomBezier = Random.Range (0, 2);
					if (randomBezier == 1) {
						advancedAImode = EnemyStates.Find;
					}
					break;
				}

			}

			DoAction(player, advancedAImode);

		}

	}
}