using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System.Collections;

namespace Suriyun.PetZoo {

    class PetZooEditor : EditorWindow {

        GameObject agent;
        GameObject animal;

        [MenuItem("Window/Suriyun/PetZoo Controller/Agent Maker")]
        public static void ShowWindow() {
            EditorWindow.GetWindow(typeof(PetZooEditor));
        }

        void OnGUI() {

            GUILayout.Label("Pet/Zoo Agent Maker", EditorStyles.boldLabel);
            agent = (GameObject)EditorGUILayout.ObjectField("Agent:", agent, typeof(GameObject), true);
            animal = (GameObject)EditorGUILayout.ObjectField("Animal:", animal, typeof(GameObject), true);

            if (GUILayout.Button("Make Pet Agent")) {
                MakePetAgent();
            }
            if (GUILayout.Button("Make Zoo Agent")) {
                MakeZooAgent();
            }
            if (GUILayout.Button("Make Zoo2 Agent")) {
                MakeZoo2Agent();
            }
            if (GUILayout.Button("Make Zoo3 Agent")) {
                MakeZoo3Agent();
            }
            if (GUILayout.Button("Make Zoo4 Agent")) {
                MakeZoo4Agent();
            }

        }

        protected void MakePetAgent() {
            GameObject agent = Instantiate(this.agent);
            agent.name = this.agent.name + this.animal.name;
            GameObject animal = Instantiate(this.animal);
            animal.name = this.animal.name;
            animal.transform.SetParent(agent.transform);
            animal.transform.localPosition = Vector3.zero;
            animal.transform.localScale = this.animal.transform.localScale * 3f;

            Animator animal_a = animal.GetComponent<Animator>();
            string ctrl_type = animal_a.runtimeAnimatorController.name.Remove(0, 4);
            RuntimeAnimatorController ctrl =
                (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                    "Assets/Suriyun/Addon-PetZoo/Mecanim/Controller" + ctrl_type + ".controller",
                    typeof(RuntimeAnimatorController));
            animal_a.runtimeAnimatorController = ctrl;
            agent.GetComponent<ControllerPetZoo>().mecanim = animal_a;

            NavMeshAgent navmesh_agent = agent.GetComponent<NavMeshAgent>();
            navmesh_agent.radius = animal.transform.localScale.x / 9f;
            navmesh_agent.height = animal.transform.localScale.y / 5f;

            BoxCollider col = agent.GetComponent<BoxCollider>();
            col.center = Vector3.up * navmesh_agent.radius;
            col.size = new Vector3(navmesh_agent.radius * 2f, navmesh_agent.radius * 2f, navmesh_agent.radius * 2f);

            PrefabUtility.SaveAsPrefabAsset(agent, "Assets/Suriyun/Addon-PetZoo/Prefab/Pet/" + agent.name + ".prefab");

            DestroyImmediate(agent);
        }

        protected void MakeZooAgent() {
            GameObject agent = Instantiate(this.agent);
            agent.name = this.agent.name + this.animal.name;
            GameObject animal = Instantiate(this.animal);
            animal.name = this.animal.name;
            animal.transform.SetParent(agent.transform);
            animal.transform.localPosition = Vector3.zero;
            animal.transform.localScale = this.animal.transform.localScale * 3f;

            Animator animal_a = animal.GetComponent<Animator>();
            string ctrl_type = animal_a.runtimeAnimatorController.name.Remove(0, 4);
            RuntimeAnimatorController ctrl =
                (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                    "Assets/Suriyun/Addon-PetZoo/Mecanim/Controller" + ctrl_type + ".controller",
                    typeof(RuntimeAnimatorController));
            animal_a.runtimeAnimatorController = ctrl;
            agent.GetComponent<ControllerPetZoo>().mecanim = animal_a;

            NavMeshAgent navmesh_agent = agent.GetComponent<NavMeshAgent>();
            navmesh_agent.radius = animal.transform.localScale.x / 9f;
            navmesh_agent.height = animal.transform.localScale.y / 5f;

            BoxCollider col = agent.GetComponent<BoxCollider>();
            col.center = Vector3.up * navmesh_agent.radius;
            col.size = new Vector3(navmesh_agent.radius * 2f, navmesh_agent.radius * 2f, navmesh_agent.radius * 2f);

            PrefabUtility.SaveAsPrefabAsset(agent, "Assets/Suriyun/Addon-PetZoo/Prefab/Zoo/" + agent.name + ".prefab");

            DestroyImmediate(agent);
        }

        protected void MakeZoo2Agent() {
            GameObject agent = Instantiate(this.agent);
            agent.name = this.agent.name + this.animal.name;
            GameObject animal = Instantiate(this.animal);
            animal.name = this.animal.name;
            animal.transform.SetParent(agent.transform);
            animal.transform.localPosition = Vector3.zero;
            animal.transform.localScale = this.animal.transform.localScale * 3f;

            Animator animal_a = animal.GetComponent<Animator>();
            string ctrl_type = animal_a.runtimeAnimatorController.name.Remove(0, 4);
            RuntimeAnimatorController ctrl =
                (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                    "Assets/Suriyun/Addon-PetZoo/Mecanim/Controller" + ctrl_type + ".controller",
                    typeof(RuntimeAnimatorController));
            animal_a.runtimeAnimatorController = ctrl;
            agent.GetComponent<ControllerPetZoo>().mecanim = animal_a;

            NavMeshAgent navmesh_agent = agent.GetComponent<NavMeshAgent>();
            navmesh_agent.radius = animal.transform.localScale.x / 9f;
            navmesh_agent.height = animal.transform.localScale.y / 5f;

            BoxCollider col = agent.GetComponent<BoxCollider>();
            col.center = Vector3.up * navmesh_agent.radius;
            col.size = new Vector3(navmesh_agent.radius * 2f, navmesh_agent.radius * 2f, navmesh_agent.radius * 2f);

            PrefabUtility.SaveAsPrefabAsset(agent, "Assets/Suriyun/Addon-PetZoo/Prefab/Zoo2/" + agent.name + ".prefab");

            DestroyImmediate(agent);
        }

        protected void MakeZoo3Agent() {
            GameObject agent = Instantiate(this.agent);
            agent.name = this.agent.name + this.animal.name;
            GameObject animal = Instantiate(this.animal);
            animal.name = this.animal.name;
            animal.transform.SetParent(agent.transform);
            animal.transform.localPosition = Vector3.zero;
            animal.transform.localScale = this.animal.transform.localScale * 3f;

            Animator animal_a = animal.GetComponent<Animator>();
            string ctrl_type = animal_a.runtimeAnimatorController.name.Remove(0, 4);
            RuntimeAnimatorController ctrl =
                (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                    "Assets/Suriyun/Addon-PetZoo/Mecanim/Controller" + ctrl_type + ".controller",
                    typeof(RuntimeAnimatorController));
            animal_a.runtimeAnimatorController = ctrl;
            agent.GetComponent<ControllerPetZoo>().mecanim = animal_a;

            NavMeshAgent navmesh_agent = agent.GetComponent<NavMeshAgent>();
            navmesh_agent.radius = animal.transform.localScale.x / 9f;
            navmesh_agent.height = animal.transform.localScale.y / 5f;

            BoxCollider col = agent.GetComponent<BoxCollider>();
            col.center = Vector3.up * navmesh_agent.radius;
            col.size = new Vector3(navmesh_agent.radius * 2f, navmesh_agent.radius * 2f, navmesh_agent.radius * 2f);

            PrefabUtility.SaveAsPrefabAsset(agent, "Assets/Suriyun/Addon-PetZoo/Prefab/Zoo3/" + agent.name + ".prefab");

            DestroyImmediate(agent);
        }

        protected void MakeZoo4Agent() {
            GameObject agent = Instantiate(this.agent);
            agent.name = this.agent.name + this.animal.name;
            GameObject animal = Instantiate(this.animal);
            animal.name = this.animal.name;
            animal.transform.SetParent(agent.transform);
            animal.transform.localPosition = Vector3.zero;
            animal.transform.localScale = this.animal.transform.localScale * 3f;

            Animator animal_a = animal.GetComponent<Animator>();
            string ctrl_type = animal_a.runtimeAnimatorController.name.Remove(0, 4);
            RuntimeAnimatorController ctrl =
                (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(
                    "Assets/Suriyun/Addon-PetZoo/Mecanim/Controller" + ctrl_type + ".controller",
                    typeof(RuntimeAnimatorController));
            animal_a.runtimeAnimatorController = ctrl;
            agent.GetComponent<ControllerPetZoo>().mecanim = animal_a;

            NavMeshAgent navmesh_agent = agent.GetComponent<NavMeshAgent>();
            navmesh_agent.radius = animal.transform.localScale.x / 9f;
            navmesh_agent.height = animal.transform.localScale.y / 5f;

            BoxCollider col = agent.GetComponent<BoxCollider>();
            col.center = Vector3.up * navmesh_agent.radius;
            col.size = new Vector3(navmesh_agent.radius * 2f, navmesh_agent.radius * 2f, navmesh_agent.radius * 2f);

            PrefabUtility.SaveAsPrefabAsset(agent, "Assets/Suriyun/Addon-PetZoo/Prefab/Zoo4/" + agent.name + ".prefab");

            DestroyImmediate(agent);
        }



    }
}