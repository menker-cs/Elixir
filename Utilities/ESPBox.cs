using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Elixir.Utilities
{
    public class HollowBox : MonoBehaviour
    {
        private void LateUpdate()
        {
            base.transform.LookAt(base.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
    public class ESPBox : MonoBehaviour
    {
        private void Start()
        {
            this.hollowBoxGO = new GameObject("HollowBoxHollow");
            this.hollowBoxGO.transform.SetParent(base.transform);
            this.topSide = GameObject.CreatePrimitive(PrimitiveType.Cube);
            this.bottomSide = GameObject.CreatePrimitive(PrimitiveType.Cube);
            this.leftSide = GameObject.CreatePrimitive(PrimitiveType.Cube);
            this.rightSide = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(this.topSide.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(this.bottomSide.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(this.leftSide.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(this.rightSide.GetComponent<BoxCollider>());
            this.topSide.transform.SetParent(this.hollowBoxGO.transform);
            this.topSide.transform.localPosition = new Vector3(0f, this.boxHeight / 2f - boxThickness / 2f, 0f);
            this.topSide.transform.localScale = new Vector3(this.boxWidth, boxThickness, boxThickness);
            this.bottomSide.transform.SetParent(this.hollowBoxGO.transform);
            this.bottomSide.transform.localPosition = new Vector3(0f, -this.boxHeight / 2f + boxThickness / 2f, 0f);
            this.bottomSide.transform.localScale = new Vector3(this.boxWidth, boxThickness, boxThickness);
            this.leftSide.transform.SetParent(this.hollowBoxGO.transform);
            this.leftSide.transform.localPosition = new Vector3(-this.boxWidth / 2f + boxThickness / 2f, 0f, 0f);
            this.leftSide.transform.localScale = new Vector3(boxThickness, this.boxHeight, boxThickness);
            this.rightSide.transform.SetParent(this.hollowBoxGO.transform);
            this.rightSide.transform.localPosition = new Vector3(this.boxWidth / 2f - boxThickness / 2f, 0f, 0f);
            this.rightSide.transform.localScale = new Vector3(boxThickness, this.boxHeight, boxThickness);
            this.hollowBoxGO.transform.localPosition = Vector3.zero;
            this.hollowBoxGO.transform.localRotation = Quaternion.identity;
            this.hollowBoxGO.AddComponent<HollowBox>();
            this.topSide.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
            this.bottomSide.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
            this.leftSide.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
            this.rightSide.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
        }


        public float boxWidth = 1f;


        public float boxHeight = 1f;

        public static void IncreaseboxThickness()
        {

            boxThickness++;

        }
        public static void DecreaseboxThickness()
        {

            boxThickness--;

        }
        public static void ResetBoxThickness()
        {
            boxThickness = 0.03305f;
        }
        public static float boxThickness = 0.03305f;


        public GameObject topSide;


        public GameObject bottomSide;


        public GameObject leftSide;


        public GameObject rightSide;


        private GameObject hollowBoxGO;
    }
}
