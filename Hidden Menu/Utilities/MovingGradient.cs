using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hidden.Utilities
{
    public class MovingGradient : MonoBehaviour
    {
        public void Start()
        {
            Mesher = GameObject.Instantiate<Mesh>(GetComponent<MeshFilter>().mesh);
            Material material = new Material(Shader.Find("Sprites/Default"));
            material.SetInt("_ZTest", 4);
            material.SetInt("_ZWrite", 1);
            gameObject.GetComponent<MeshRenderer>().material = material;
            if (!Mesher)
            {
                Debug.Log("no no work ):");
            }
            GetComponent<MeshFilter>().mesh = Mesher;
        }

        public void Update()
        {
            int num = Mesher.vertices.Length;
            colors = new Color[num];
            HueShft += Time.deltaTime * Speed;
            for (int i = 0; i < num; i++)
            {
                Vector3 vector = transform.TransformPoint(Mesher.vertices[i]);
                transform.rotation = gameObject.transform.parent.rotation;
                float num2 = Mathf.Repeat(HueShft + (vector.x + vector.y) * 0.25f, 1f);
                colors[i] = Color.HSVToRGB(num2, 1f, 1f);
            }
            Mesher.colors = colors;
        }

        public float Speed = 0.3f;

        public Mesh Mesher;

        private Mesh tempMesh;

        private Color[] colors;

        private float HueShft;
    }
}