using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Suriyun.PetZoo
{
    public class InputManager : MonoBehaviour
    {
        public Camera main_camera;
        public Transform destination;

        public List<ControllerPetZoo> controllers;

        public bool use_dynamic_stopping_distance = false;
        protected float dynamic_stopping_distance = 2f;

        protected virtual void Start()
        {
            if (destination == null)
            {
                Debug.LogWarning("InputHandler.destination not set.");
                GameObject g = new GameObject("Destination");
                destination = g.transform;
            }
            if (main_camera == null)
            {
                Debug.LogWarning("InputHandler.main_camera not set.");
                main_camera = Camera.main;
            }
            if (controllers.Count == 0)
            {
                Debug.LogWarning("InputHandler.controllers is empty.");
            }

            this.AddAllController();

        }

        protected virtual void Update()
        {
            this.HandleMouse0();

            if (use_dynamic_stopping_distance)
            {
                this.dynamic_stopping_distance = 0.5f + Mathf.Sqrt((float)controllers.Count / Mathf.PI);
                foreach (ControllerPetZoo c in controllers)
                {
                    c.agent.stoppingDistance = this.dynamic_stopping_distance;
                }
            }
        }


        public virtual void SetUseDynamicStoppingDistance(bool use)
        {
            this.use_dynamic_stopping_distance = use;

        }

        protected RaycastHit[] hits;
        protected Ray ray;
        protected bool done = false;
        private float tmp_distance;
        private Vector3 tmp_point;

        protected virtual void HandleMouse0()
        {
            if (Input.GetMouseButton(0) && (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()))
            {
                done = false;
                tmp_distance = 0f;
                ray = main_camera.ScreenPointToRay(Input.mousePosition);
                hits = Physics.RaycastAll(ray);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.tag == "Ground")
                    {
                        if (tmp_distance == 0f)
                        {
                            tmp_distance = Vector3.Distance(hits[i].point, main_camera.transform.position);
                            tmp_point = hits[i].point;
                        }
                        else if(Vector3.Distance(hits[i].point, main_camera.transform.position) < tmp_distance)
                        {
                            tmp_distance = Vector3.Distance(hits[i].point, main_camera.transform.position);
                            tmp_point = hits[i].point;
                        }
                    }
                }
                destination.position = tmp_point;
                foreach (ControllerPetZoo c in controllers)
                {
                    c.SetDestination(tmp_point);
                }
            }
        }

        public virtual void AddAllController()
        {
            ControllerPetZoo[] tmp = GameObject.FindObjectsOfType<ControllerPetZoo>();
            foreach (ControllerPetZoo c in tmp)
            {
                this.AddController(c);
            }
        }

        public virtual void AddController(ControllerPetZoo ctrl)
        {
            ctrl.SetDestination(destination.position);
            if (!controllers.Contains(ctrl))
            {
                controllers.Add(ctrl);
            }
        }

        public virtual void AddControllerAndSpawn(GameObject prefab, Vector3 position)
        {
            if (prefab.GetComponent<ControllerPetZoo>() == null)
            {
                Debug.LogError("AddController : GameObject does not have 'ControllerPetZoo' component.");
            }
            else
            {
                GameObject tmp = Instantiate(prefab, position, prefab.transform.rotation);
                controllers.Add(tmp.GetComponent<ControllerPetZoo>());
            }

        }

        public virtual void RemoveController(int index)
        {
            if (index > controllers.Count - 1)
            {
                Debug.LogError("RemoveController : 'index' out of range.");
            }
            else
            {
                controllers.RemoveAt(index);
            }
        }

        public virtual void RemoveController(ControllerPetZoo ctrl)
        {
            if (!controllers.Contains(ctrl))
            {
                Debug.LogError("RemoveController : no object in the list.");
            }
            else
            {
                controllers.Remove(ctrl);
            }
        }

        public virtual void RemoveControllerByAmount(int amount)
        {
            if (amount > controllers.Count)
            {
                Debug.LogError("RemoveControllerByAmount : 'amount' exeeds maximum count.");
            }
            else
            {
                controllers.RemoveRange(0, amount);
            }

        }
    }
}