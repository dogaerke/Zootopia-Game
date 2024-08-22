using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Suriyun.FramerateGrapher
{
	public class FramerateGrapher : MonoBehaviour
	{

		public Text fps_text;
		public RectTransform top;
		public RectTransform bottom;
		public RectTransform grapher;

		protected float top_pos;
		protected float bottom_pos;
		protected float magnitude;

		protected float offset_pos;

		public float max_fps = 100;
		public float fps;

		Vector3 offset;


		void Start ()
		{
			this.top_pos = top.position.y;
			this.bottom_pos = bottom.position.y;
			this.magnitude = top_pos - bottom_pos;

			this.max_fps = 100;

			offset = new Vector3 ();
			offset.x = bottom.position.x;
			offset.y = bottom.position.y;
			offset.z = grapher.position.z;

		}

		void Update ()
		{
			fps = 1f / Time.deltaTime;
			fps_text.text = "" + (int)(fps * 100f) / 100;

			offset.y = bottom_pos + Mathf.Clamp (fps / max_fps * magnitude, 0, magnitude);
			grapher.position = offset; 
		}

	}
}