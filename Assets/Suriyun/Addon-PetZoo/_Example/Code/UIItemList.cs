using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Suriyun.PetZoo
{
	public class UIItemList : MonoBehaviour
	{

		public Text obj_name;
		public Text obj_state;
		public ControllerPetZoo ctrl;

		protected bool is_zoomed = false;

		public void Init (ControllerPetZoo ctrl)
		{
			this.ctrl = ctrl;
			obj_name.text = ctrl.gameObject.name;
			//StartCoroutine (PseudoUpdate ());
		}



		protected virtual void Update ()
		{
			if (obj_state.text != ctrl.GetCurrentState ()) {
				obj_state.text = ctrl.GetCurrentState ();
			}
			if (ctrl == null) {
				Destroy (this.gameObject);
			}
		}

		IEnumerator PseudoUpdate ()
		{
			WaitForSeconds wait = new WaitForSeconds (0.1f);
			while (ctrl != null) {
				if (obj_state.text != ctrl.GetCurrentState ()) {
					obj_state.text = ctrl.GetCurrentState ();
				}
				yield return wait;
			}
			Destroy (this.gameObject);
		}

		public void Zoom ()
		{
			CameraController.instance.ZoomAt (ctrl.transform_cached);
		}

		public void Kill ()
		{
			GameManager.instance.DeSpawn (ctrl);
			Destroy (this.gameObject);
		}


	}

}