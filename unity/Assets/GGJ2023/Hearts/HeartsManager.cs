using System.Collections.Generic;
using UnityEngine;

namespace GGJ2023
{
    public class HeartsManager : MonoBehaviour
    {
        [SerializeField]
        private Color colorOn;
        [SerializeField]
        private Color colorOff;
        [SerializeField]
        private float gap;
        [SerializeField]
        private GameObject heartPrefab;
        [SerializeField]
        private Transform spawnPoint;

        private List<MeshRenderer> heartsMRs = new List<MeshRenderer>();

        public void Initialise(int lives)
        {
            var totLenght = (lives - 1) * gap;
            var startX = spawnPoint.position.x - (totLenght / 2);
            var endX = startX * -1;
            for (int i = 0; i < lives; i++)
            {
                var heart = Instantiate(heartPrefab);
                heart.transform.position = new Vector3(Mathf.Lerp(startX, endX, (float)i / (float)(lives-1)), spawnPoint.position.y, spawnPoint.position.z);
                var meshRenderer = heart.GetComponentInChildren<MeshRenderer>();
                meshRenderer.material.SetColor("Tint", colorOn);
                heartsMRs.Add(meshRenderer);
            }
        }

        public void UpdateHearts(int countOn)
        {
            for (int i = 0; i < heartsMRs.Count; i++)
            {
                var color = i < countOn ? colorOn : colorOff;
                heartsMRs[i].material.SetColor("Tint", color);
            }
        }
    }
}
