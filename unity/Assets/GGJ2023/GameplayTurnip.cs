using System.Collections.Generic;
using UnityEngine;

namespace GGJ2023
{
    public class GameplayTurnip : MonoBehaviour
    {
        [SerializeField]
        private new Rigidbody rigidbody;
        [SerializeField]
        private Transform height;

        private int stepsRequired;
        private bool listenForInput = false;
        private int stepsSoFar = -1;

        private void Start()
        {
            rigidbody.isKinematic = true;
            rigidbody.angularDrag = 5;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        }

        public void Initialise(int stepsRequired)
        {
            this.stepsRequired = stepsRequired;
        }

        private void Update()
        {
            Debug.Log(Input.touchCount);
            //if (listenForInput)
            {
                if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    stepsSoFar++;

                    rigidbody.isKinematic = false;
                    rigidbody.AddForce(new Vector3(Random.Range(-2, 2), Random.Range(2, 4), Random.Range(-2, 2)), ForceMode.Impulse);
                    rigidbody.AddTorque(new Vector3(Random.Range(-2, 2), Random.Range(2, 2), Random.Range(-2, 2)), ForceMode.Impulse);
                }
            }
        }

        public void ListenForInput()
        {
            listenForInput = true;
        }
    }
}
