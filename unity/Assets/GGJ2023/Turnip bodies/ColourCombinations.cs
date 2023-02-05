using System.Collections.Generic;
using UnityEngine;

namespace GGJ2023
{
    [CreateAssetMenu(fileName = "ColourCombinations", menuName = "GGJ 2023/ColourCombinations")]
    public class ColourCombinations : ScriptableObject
    {
        [System.Serializable]
        public struct Combination
        {
            public Color color1;
            public Color color2;
        }

        public List<Combination> list = new List<Combination>();
    }
}