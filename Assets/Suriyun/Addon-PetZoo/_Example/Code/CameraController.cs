using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Suriyun.PetZoo
{
	public class CameraController : MonoBehaviour
	{
		public static CameraController instance;

		public InputManager input_manager;

		public Transform target;
		public float camera_speed = 3.33f;
		protected Transform transform_cached;
		protected Vector3 pos_offset;
		protected Camera cam;
		protected float fov_original;

		protected Vector3 herd_center;
		protected bool is_zoomed = false;

		protected Transform animal;

		protected Transform stabilizer;

		void Awake ()
		{
			instance = this;
			transform_cached = GetComponent<Transform> ();
			pos_offset = transform_cached.position - target.position;

			//stabilizer = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//stabilizer.GetComponent<MeshFilter>()
			stabilizer = new GameObject ("Stabilizer").transform;

		}

		protected virtual void Start ()
		{
			cam = GetComponent<Camera> ();
			fov_original = cam.fieldOfView;

			if (input_manager == null) {
				Debug.LogWarning ("CameraController : 'input_manager' not set.");
				input_manager = GameObject.FindObjectOfType<InputManager> ();
			}

		}

		protected virtual void Update ()
		{
			

			if (this.animal == null) {
				this.UnZoom ();
			}

			if (!is_zoomed) {
				this.CalculateHerdCenter ();
				this.target.position = Vector3.Lerp (target.position, herd_center, camera_speed * Time.deltaTime);
				this.cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, fov_original, camera_speed * Time.deltaTime);
			} else {
				this.target.position = Vector3.Lerp (target.position, animal.position, camera_speed * Time.deltaTime);
				this.cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, fov_original / 3f, camera_speed * Time.deltaTime);
			}

			transform_cached.position = Vector3.Lerp (transform_cached.position, target.position + pos_offset, camera_speed * Time.deltaTime);
		
			stabilizer.position = Vector3.Lerp (stabilizer.position, transform_cached.position, camera_speed * Time.deltaTime);
			stabilizer.LookAt (target);

			transform_cached.rotation = Quaternion.Lerp (transform_cached.rotation, stabilizer.rotation, camera_speed * Time.deltaTime);
		}

		protected Vector3 tmp;

		protected virtual void CalculateHerdCenter ()
		{
			tmp = Vector3.zero;
			foreach (ControllerPetZoo c in input_manager.controllers) {
				tmp += c.transform_cached.position;
			}
			if (input_manager.controllers.Count > 0) {
				herd_center = tmp / input_manager.controllers.Count;
			}
		}

		public virtual void ZoomAt (Transform t)
		{
			is_zoomed = true;
			this.animal = t;
		}

		public virtual void UnZoom ()
		{
			is_zoomed = false;
		}
	}


}