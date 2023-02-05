using UnityEngine;

namespace GGJ2023
{
    public class TurnipEyes : MonoBehaviour
    {
        public enum EyeStyle {
            Default,
            Close,
            O,
        }

        [SerializeField]
        private GameObject heightPivot;
        [SerializeField]
        private GameObject leftAnglePivot;
        [SerializeField]
        private GameObject leftRadiusPivot;
        [SerializeField]
        private GameObject leftScalePivot;
        [SerializeField]
        private GameObject leftEyeRotationPivot;
        [SerializeField]
        private GameObject rightAnglePivot;
        [SerializeField]
        private GameObject rightRadiusPivot;
        [SerializeField]
        private GameObject rightScalePivot;
        [SerializeField]
        private GameObject rightEyeRotationPivot;
        [SerializeField]
        private float height;
        [SerializeField]
        private float radius;
        [SerializeField]
        private float angleBetweenEyes;
        [SerializeField]
        private float scale;
        [SerializeField]
        private float eyeRotation;
        [SerializeField]
        private Texture2D textureDefault;
        [SerializeField]
        private Texture2D textureClose;
        [SerializeField]
        private Texture2D textureO;

        private void OnValidate()
        {
            PlaceEyes();
        }

        private void PlaceEyes()
        {
            heightPivot.transform.localPosition = new Vector3(0, height, 0);

            leftAnglePivot.transform.localRotation = Quaternion.Euler(0, angleBetweenEyes / 2, 0);
            leftRadiusPivot.transform.localPosition = new Vector3(0, 0, -radius);
            leftScalePivot.transform.localScale = new Vector3(scale, scale, scale);
            leftEyeRotationPivot.transform.localRotation = Quaternion.Euler(0, 0, -eyeRotation);

            rightAnglePivot.transform.localRotation = Quaternion.Euler(0, -angleBetweenEyes / 2, 0);
            rightRadiusPivot.transform.localPosition = new Vector3(0, 0, -radius);
            rightScalePivot.transform.localScale = new Vector3(scale, scale, scale);
            rightEyeRotationPivot.transform.localRotation = Quaternion.Euler(0, 0, 180 + eyeRotation);
        }

        public void SetEyeStyle(EyeStyle style)
        {
            foreach(var mrs in GetComponentsInChildren<MeshRenderer>())
            {
                var texture = textureDefault;
                switch(style)
                {
                    case EyeStyle.Close:
                        texture = textureClose;
                        break;
                    case EyeStyle.O:
                        texture = textureO;
                        break;
                }
                mrs.material.SetTexture("Texture", texture);
            }
        }
    }
}
