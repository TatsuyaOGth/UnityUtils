using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ogsn.Utils.Components
{
    public class Gizmo : MonoBehaviour
    {
        public bool Show = true;
        public bool SelectedOnly;

        public enum GizmoShape { Cube, Sphere, WireCube, WireSphere }
        public GizmoShape Shape = GizmoShape.Cube;

        public float Scale = 1;
        public Color Color = Color.green;

        void OnDrawGizmos()
        {
            if (SelectedOnly == false && Show)
            {
                Draw();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (SelectedOnly && Show)
            {
                Draw();
            }
        }

        void Draw()
        {
            Gizmos.color = Color;

            var curMat = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            switch (Shape)
            {
                case GizmoShape.Cube:
                    Gizmos.DrawCube(Vector3.zero, transform.localScale * Scale);
                    break;
                case GizmoShape.Sphere:
                    Gizmos.DrawSphere(Vector3.zero, transform.localScale.x * Scale * 0.5f);
                    break;
                case GizmoShape.WireCube:
                    Gizmos.DrawWireCube(Vector3.zero, transform.localScale * Scale);
                    break;
                case GizmoShape.WireSphere:
                    Gizmos.DrawWireSphere(Vector3.zero, transform.localScale.x * Scale * 0.5f);
                    break;
            }

            Gizmos.matrix = curMat;
        }
    }
}
