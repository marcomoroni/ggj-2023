using UnityEngine;

namespace GGJ2023
{
    public class GameplayTurnip : MonoBehaviour
    {
        [SerializeField]
        private new Rigidbody rigidbody;
        [SerializeField]
        private Transform height;
        [SerializeField]
        private new Animation animation;
        [SerializeField]
        private MeshRenderer bodyMR;
        [SerializeField]
        private ColourCombinations colourCombinations;

        private int stepsRequired;
        private bool listenForInput = false;
        private int stepsSoFar = 0;
        private System.Action onDone;
        private System.Action onFail;
        private bool done;
        private float timeSinceDone;

        private void Start()
        {
            rigidbody.isKinematic = true;
            rigidbody.angularDrag = 5;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
            //rigidbody.useGravity = false;

            ApplyRandomColorCombination();
        }

        public void Initialise(int stepsRequired, System.Action onDone, System.Action onFail)
        {
            this.stepsRequired = stepsRequired;
            this.onDone = onDone;
            this.onFail = onFail;
        }

        public void ListenForInput()
        {
            listenForInput = true;
        }

        public void ForceDone()
        {
            done = true;
        }

        private void ApplyRandomColorCombination()
        {
            var randomCombination = colourCombinations.list[Random.Range(0, colourCombinations.list.Count)];
            bodyMR.material.SetColor("Color1", randomCombination.color1);
            bodyMR.material.SetColor("Color2", randomCombination.color2);
        }

        private void Update()
        {
            // ----- wait for one frame!

            if (listenForInput && !done)
            {
                if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    stepsSoFar++;

                    if (FindObjectOfType<Bar>().ArrowIsInValidArea)
                    {
                        if (stepsSoFar < stepsRequired)
                        {

                            animation.Play("Turnip up");

                            StartCoroutine(EyesClose());

                            transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);

                            FindObjectOfType<Bar>().Highlight();
                        }
                        else
                        {
                            transform.position = new Vector3(transform.position.x, -height.localPosition.y + 0.3f, transform.position.z);

                            rigidbody.isKinematic = false;
                            rigidbody.AddForce(new Vector3(Random.Range(-3, 3), Random.Range(4, 8), 0), ForceMode.Impulse);
                            rigidbody.AddTorque(new Vector3(Random.Range(-6, 6), Random.Range(-6, 6), Random.Range(-6, 6)), ForceMode.Impulse);
                            //rigidbody.AddForce(new Vector3(Random.Range(-1,1), Random.Range(1, 1), 0), ForceMode.Impulse);
                            //rigidbody.AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)), ForceMode.Impulse);

                            StartCoroutine(EyesO());

                            onDone?.Invoke();

                            done = true;
                        }
                    }
                    else
                    {
                        onFail?.Invoke();
                    }
                }
            }

            // --- they get destoyed anyway
            //if(done)
            //{
            //    timeSinceDone += Time.deltaTime;
            //    if (timeSinceDone > 20)
            //    {
            //        Destroy(gameObject);
            //    }
            //}
        }

        System.Collections.IEnumerator EyesClose()
        {
            GetComponentInChildren<TurnipEyes>().SetEyeStyle(TurnipEyes.EyeStyle.Close);
            yield return new WaitForSeconds(0.4f);
            GetComponentInChildren<TurnipEyes>().SetEyeStyle(TurnipEyes.EyeStyle.Default);
        }

        System.Collections.IEnumerator EyesO()
        {
            GetComponentInChildren<TurnipEyes>().SetEyeStyle(TurnipEyes.EyeStyle.O);
            yield return new WaitForSeconds(1f);
            GetComponentInChildren<TurnipEyes>().SetEyeStyle(TurnipEyes.EyeStyle.Default);
        }
    }
}
