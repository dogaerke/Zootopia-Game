using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace Suriyun.PetZoo
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager instance;
		public InputManager input_handler;

		public List<GameObject> animal_prefabs;


		protected virtual void Awake ()
		{
			GameManager.instance = this;
		}

		protected virtual void Start ()
		{
			
			foreach (GameObject g in animal_prefabs) {
				if (g == null) {
					animal_prefabs.Remove (g);
				}
			}

			foreach (GameObject g in animal_prefabs) {
				if (UnityEngine.Random.Range (0f, 100f) < 27f) {
					Spawn (g);
				}
			}

			ui.selected = UnityEngine.Random.Range (0, animal_prefabs.Count);
			/*
			Spawn (animal_prefabs [6]);
			Spawn (animal_prefabs [6]);
			Spawn (animal_prefabs [6]);
			Spawn (animal_prefabs [6]);
			Spawn (animal_prefabs [6]);
			Spawn (animal_prefabs [6]);
			*/
		}

		protected virtual void Update ()
		{
			ui.Update ();
		}



		#region Core

		public virtual void Spawn (GameObject prefab)
		{
			if (prefab.GetComponent<ControllerPetZoo> () == null) {
				Debug.LogError ("GameManager.Spawn (GameObject prefab) : prefab does not have 'ControllerPetZoo' component.");
			} else {
				GameObject g = Instantiate (prefab) as GameObject;
				g.transform.position = new Vector3 (-2, 0, -2);
				ControllerPetZoo ctrl = g.GetComponent<ControllerPetZoo> ();
				input_handler.AddController (ctrl);

				ui.AddItemList (ctrl);
			}
		}

		public virtual void DeSpawn (ControllerPetZoo ctrl)
		{
			input_handler.RemoveController (ctrl);
			Destroy (ctrl.gameObject);
		}

		public virtual void DeSpawn (GameObject g)
		{
			if (g.GetComponent<ControllerPetZoo> () == null) {
				Debug.LogError ("GameManager.DeSpawn(GameObject g) : g does not contain 'ControllerPetZoo' component.");
			} else {
				this.DeSpawn (g.GetComponent<ControllerPetZoo> ());
			}
		}

		#endregion

		#region UI

		public GameUI ui;

		public virtual void SelectRight ()
		{
			ui.selected++;
			ui.selected = ui.selected % GameManager.instance.animal_prefabs.Count;
		}

		public virtual void SelectLeft ()
		{
			ui.selected--;
			if (ui.selected < 0) {
				ui.selected = GameManager.instance.animal_prefabs.Count - 1;
			}
		}

		public virtual void SpawnSelected ()
		{
			this.Spawn (GameManager.instance.animal_prefabs [ui.selected]);
		}

		public virtual void DeSpawnOne ()
		{
			ControllerPetZoo ctrl = GameObject.FindObjectOfType<ControllerPetZoo> ();
			if (ctrl) {
				this.DeSpawn (ctrl);
			}
		}

		public virtual void Reset ()
		{
			SceneManager.LoadScene ("example_pet");
		}

		public virtual void PresetOne ()
		{
			this.ClearAllAnimals ();
			this.Spawn (animal_prefabs [22]);
		}

		public virtual void PresetTwo ()
		{
			this.ClearAllAnimals ();
			for (int i = 0; i < 10; i++) {
				this.Spawn (animal_prefabs [19]);
			}
		}

		public virtual void PresetThree ()
		{
			this.ClearAllAnimals ();
			for (int i = 0; i < 50; i++) {
				this.Spawn (animal_prefabs [9]);
			}
		}

		protected virtual void ClearAllAnimals ()
		{
			
			ControllerPetZoo[] ctrls = GameObject.FindObjectsOfType<ControllerPetZoo> ();
			foreach (ControllerPetZoo c in ctrls) {
				this.DeSpawn (c);
			}
		}

		#endregion
	}

	[Serializable]
	public class GameUI
	{
		public Transform list_parent;
		public GameObject list_item_prefab;
		public Text t_count;
		public Text t_current_selected;
		public int selected = 0;

		public virtual void AddItemList (ControllerPetZoo ctrl)
		{
			GameObject l = GameObject.Instantiate (this.list_item_prefab) as GameObject;
			l.transform.SetParent (this.list_parent);
			l.transform.localScale = this.list_parent.transform.localScale;
			l.transform.position = this.list_parent.transform.position;
			l.GetComponent<UIItemList> ().Init (ctrl);
		}

		string tmp_string;

		public virtual void Update ()
		{
			tmp_string = GameManager.instance.input_handler.controllers.Count.ToString ();
			if (this.t_count.text != tmp_string) {
				this.t_count.text = tmp_string;
			}

			tmp_string = GameManager.instance.animal_prefabs [this.selected].name.Remove (0, 6);
			if (this.t_current_selected.text != tmp_string) {
				this.t_current_selected.text = tmp_string;
			}
		}

	}

}


