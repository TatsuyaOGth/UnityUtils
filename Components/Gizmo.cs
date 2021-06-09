using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ogsn.Utils.Components
{
    public class Gizmo : MonoBehaviour
    {
        public bool Show = true;
        public bool SelectedOnly;

        public enum GizmoShape { Cube, Sphere, WireCube, WireSphere, Frustum }
        public GizmoShape Shape = GizmoShape.Cube;

        public float Scale = 1;
        public Color Color = Color.green;

        Camera _camera;
        Camera Camera
        {
            get
            {
                if (!_camera) _camera = GetComponent<Camera>();
                if (!_camera) _camera = Camera.main;
                return _camera;
            }
        }

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
                case GizmoShape.Frustum:
                    DrawFrustum();
                    break;
            }

            Gizmos.matrix = curMat;
        }

        void DrawFrustum()
        {
            if (!Camera)
                return;

            float fov = Camera.fieldOfView;
            float size = Camera.orthographicSize;
            float max = Camera.farClipPlane;
            float min = Camera.nearClipPlane;
            float aspect = Camera.aspect;

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, new Vector3(aspect, 1.0f, 1.0f));
            
            if (Camera.orthographic)
            {
                Gizmos.DrawWireCube(new Vector3(0.0f, 0.0f, ((max - min) / 2.0f) + min), new Vector3(size * 2.0f, size * 2.0f, max - min));
            }
            else
            {
                Gizmos.DrawFrustum(Vector3.zero, fov, max, min, 1.0f);
            }
        }
    }
}
