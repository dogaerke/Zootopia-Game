using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Suriyun
{
	public class ControllerPetZoo : MonoBehaviour
	{
		public Animator mecanim;
		public NavMeshAgent agent;

		public float jump_power = 20f;
		public float gravity_multiplier = 1f;

		protected AgentLinkMover agent_link_mover;
		protected float base_height;
		protected float base_offset;
		protected float jump_cooldown = 0;

		protected bool eating = false;
		protected bool resting = false;

		protected int param_jump;
		protected int param_speed;
		protected int param_eating;
		protected int param_resting;

		protected string current_state;
		protected Dictionary<int,string> states;

		public Transform transform_cached;


		protected virtual void OnEnable ()
		{
			transform_cached = transform;
		}

		protected virtual void Start ()
		{
			if (agent_link_mover == null) {
				agent_link_mover = GetComponent<AgentLinkMover> ();
			}
			base_height = 0;
			base_offset = agent.baseOffset;

			states = new Dictionary<int, string> ();
			states.Add (Animator.StringToHash ("Base.Idle"), "Idle");
			states.Add (Animator.StringToHash ("Base.Eat"), "Eat");
			states.Add (Animator.StringToHash ("Base.Rest"), "Rest");
			states.Add (Animator.StringToHash ("Base.Move"), "Move");
			states.Add (Animator.StringToHash ("Base.Jump"), "Jump");

			this.param_jump = Animator.StringToHash ("jump");
			this.param_speed = Animator.StringToHash ("speed");
			this.param_resting = Animator.StringToHash ("resting");
			this.param_eating = Animator.StringToHash ("eating");
		}

		Vector3 speed;

		protected virtual void Update ()
		{
			speed = agent.velocity;
			speed.y = 0;
			agent.baseOffset = base_height + base_offset;

			mecanim.SetFloat (param_speed, speed.sqrMagnitude);
			jump_cooldown -= Time.deltaTime;

			current_state = states [mecanim.GetCurrentAnimatorStateInfo (0).fullPathHash];
		}

		protected virtual void OnTriggerEnter (Collider c)
		{
			if (c.tag == "Fence" && jump_cooldown <= 0) {
				StartCoroutine (JumpOverFence (jump_power));
			} else if (c.tag == "Eat") {
				mecanim.SetBool (param_eating, true);

			} else if (c.tag == "Rest") {
				mecanim.SetBool (param_resting, true);
			}
		}

		protected virtual void OnTriggerExit (Collider c)
		{
			if (c.tag == "Eat") {
				mecanim.SetBool (param_eating, false);
			} else if (c.tag == "Rest") {
				mecanim.SetBool (param_resting, false);
			}
		}

		protected virtual IEnumerator JumpOverFence (float jump_power)
		{
			this.Jump ();
			jump_cooldown = 1f;
			base_height = 0.1f;
			while (base_height > 0f) {
				jump_power = Mathf.Lerp (jump_power, 0, jump_power * 0.2f * Time.deltaTime);
				jump_power += Physics.gravity.y * gravity_multiplier * Time.deltaTime;
				base_height += jump_power * Time.deltaTime;
				base_height += Physics.gravity.y * gravity_multiplier * Time.deltaTime;
				yield return 0;
			}
			base_height = 0;
		}

		public virtual void Jump ()
		{
			mecanim.SetTrigger (param_jump);
		}

		public virtual void SetDestination (Vector3 pos)
		{
			agent.SetDestination (pos);
		}

		public virtual string GetCurrentState ()
		{
			return current_state;
		}

	}
}