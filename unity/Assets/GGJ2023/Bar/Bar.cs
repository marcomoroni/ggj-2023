using System.Collections.Generic;
using UnityEngine;

namespace GGJ2023
{
    public class Bar : MonoBehaviour
    {
        [SerializeField]
        private Transform l;
        [SerializeField]
        private Transform r;
        [SerializeField]
        private GameObject arrowToMove;
        [SerializeField]
        private List<Transform> arrowsScalePivots = new List<Transform>();
        [SerializeField]
        private List<MeshRenderer> arrowsMRs = new List<MeshRenderer>();
        [SerializeField]
        private float arrowSpeed = 0.8f;
        [SerializeField]
        private MeshRenderer barMR;
        [SerializeField]
        private Color colorAreaIn;
        [SerializeField]
        private Color colorAreaOut;
        [SerializeField]
        private Color colorAreaOutHighlight;
        [SerializeField]
        private float highlightDuration = 1;
        [SerializeField]
        private float distFromScreenEdge;

        private float validAreaFrom;
        private float validAreaTo;
        private bool arrowIsInValidArea;
        private float timeSinceArrowSpeedReset;
        private float timeElapsedHighlight = 300;

        public bool ArrowIsInValidArea => arrowIsInValidArea;

        private void Start()
        {
            foreach(var mr in arrowsMRs)
            {
                UpdateArrowStyle(false);
            }
            MoveArrows(0);

            SetBarValidArea(0.5f, 0.9f);
        }

        public void Highlight()
        {
            timeElapsedHighlight = 0;
        }

        private void MoveArrows(float percent)
        {
            var x = Mathf.Lerp(l.localPosition.x, r.localPosition.x, percent);
            arrowToMove.transform.localPosition = new Vector3(x, 0, 0);
        }

        private void UpdateArrowStyle(bool isInValidPosition)
        {
            var scale = isInValidPosition ? 0.15292f : 0.09108526f;
            var color = isInValidPosition ? colorAreaIn : colorAreaOut;

            foreach (var mr in arrowsMRs)
            {
                mr.material.SetColor("Tint", color);
            }
            foreach (var s in arrowsScalePivots)
            {
                //s.localScale = new Vector3(scale, scale, scale);
            }
        }

        public void RandomizeValidArea(float lenght)
        {
            var from = Random.Range(0, 1 - lenght);
            var to = from + lenght;
            SetBarValidArea(from, to);
        }

        private void SetBarValidArea(float from, float to)
        {
            validAreaFrom = from;
            validAreaTo = to;
            barMR.material.SetFloat("N1", validAreaFrom);
            barMR.material.SetFloat("N2", validAreaTo);
        }

        public void ResetArrowSpeed()
        {
            timeSinceArrowSpeedReset = 0;
        }

        private void Update()
        {
            timeSinceArrowSpeedReset += Time.deltaTime;
            //var caluclateArrowSpeed = (Time.timeSinceLevelLoad * arrowSpeed) + (timeSinceArrowSpeedReset * 2f);
            //caluclateArrowSpeed = Mathf.Min(caluclateArrowSpeed, 10f);
            //Debug.Log(caluclateArrowSpeed);
            float percent = Mathf.Sin(Time.timeSinceLevelLoad * arrowSpeed).Remap(-1, 1, 0, 1);
            MoveArrows(percent);
            arrowIsInValidArea = percent >= validAreaFrom && percent <= validAreaTo;
            UpdateArrowStyle(arrowIsInValidArea);

            timeElapsedHighlight += Time.deltaTime;
            var backColor = Color.Lerp(colorAreaOutHighlight, colorAreaOut, timeElapsedHighlight.Remap(0, highlightDuration, 0, 1));
            barMR.material.SetColor("Color1", backColor);

        }

        //private void LateUpdate()
        //{
        //    var y = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 5));

        //    float OrthoHeight = Camera.main.orthographicSize * Camera.main.aspect;
        //    transform.position = new Vector3(transform.position.x, -OrthoHeight + distFromScreenEdge, transform.position.z);
        //}
    }
}

public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}