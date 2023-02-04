using UnityEngine;

namespace GGJ2023
{
    public class GameplayTurnip : MonoBehaviour
    {
        [SerializeField]
        private new Rigidbody rigidbody;
        [SerializeField]
        private int stepsToComplete = 1;

        private void Start()
        {
            rigidbody.isKinematic = true;
            rigidbody.angularDrag = 5;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.isKinematic = false;
                rigidbody.AddForce(new Vector3(Random.Range(-2, 2), Random.Range(2, 4), Random.Range(-2, 2)), ForceMode.Impulse);
                rigidbody.AddTorque(new Vector3(Random.Range(-2, 2), Random.Range(2, 2), Random.Range(-2, 2)), ForceMode.Impulse);
            }
        }
    }
}
