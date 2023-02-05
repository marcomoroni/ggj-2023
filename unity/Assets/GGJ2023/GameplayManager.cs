using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2023
{
    public class GameplayManager : MonoBehaviour
    {
        private abstract class State
        {
            protected System.Action<State> changeState;
            protected MonoBehaviour c; // for corutines

            public State(MonoBehaviour c, System.Action<State> changeState)
            {
                this.c = c;
                this.changeState = changeState;
            }
            public virtual void OnEnter() { }
            public virtual void OnUpdate() { }
            public virtual void OnExit() { }
        }

        // --- plying
        // --- menu
        // --- end game

        private class MenuState : State
        {
            private Data data;

            public MenuState(MonoBehaviour c, System.Action<State> changeState,Data data) : base(c, changeState)
            {
                this.data = data;
            }

            public override void OnEnter()
            {
                data.startText.gameObject.SetActive(true);
            }

            public override void OnUpdate()
            {
                if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    data.uiBgAnim.Play("UI Fadeout");

                    changeState(new PlayingState(c, changeState, data));
                }
            }

            public override void OnExit()
            {
                data.startText.gameObject.SetActive(false);
            }
        }

        private class PlayingState : State
        {
            private Data data;
            private int livesLeft;

            public PlayingState(MonoBehaviour c,System.Action<State> changeState, Data data):base(c,changeState)
            {
                this.data = data;
            }

            public override void OnEnter()
            {
                livesLeft = data.lives;
                data.points = 0;
                data.pointsTextMesh.text = data.points.ToString();
                data.hearsManager.UpdateHearts(livesLeft);
                SpawnTurnip();
                data.bar.RandomizeValidArea(0.4f);
            }

            private void SpawnTurnip()
            {
                var randomTurnip = data.gameplayTurnipPrefabs[Random.Range(0, data.gameplayTurnipPrefabs.Count)];
                var turnipGO = Instantiate(randomTurnip);
                turnipGO.transform.position = new Vector3(
                    data.turnipSpawnPoint.transform.position.x + Random.Range(-3, 3),
                    data.turnipSpawnPoint.transform.position.y,
                    data.turnipSpawnPoint.transform.position.z);
                var turnipLogic = turnipGO.GetComponentInChildren<GameplayTurnip>();
                var steps = Random.Range(1, 3 + 1);
                turnipLogic.Initialise(steps, OnTurnipDone, OnFail);
                turnipLogic.ListenForInput();
            }

            private void OnTurnipDone()
            {
                data.points++;
                data.pointsTextMesh.text = data.points.ToString();

                SpawnTurnip();

                var lenght = 0.4f;
                if (data.points >= 2) lenght = 0.3f;
                if (data.points >= 4) lenght = 0.25f;
                if (data.points >= 6) lenght = 0.2f;
                if (data.points >= 8) lenght = 0.17f;
                if (data.points >= 10) lenght = 0.15f;
                if (data.points >= 15) lenght = 0.11f;
                data.bar.RandomizeValidArea(lenght);
            }

            private void OnFail()
            {
                livesLeft--;
                data.hearsManager.UpdateHearts(livesLeft);
                if (livesLeft <= 0)
                {
                    Debug.Log("gameover");
                    foreach(var t in FindObjectsOfType<GameplayTurnip>())
                    {
                        t.ForceDone();
                    }
                    changeState(new GameOver(c, changeState, data));
                }
            }

            public override void OnExit()
            {
                c.StartCoroutine(DestroyAllTurnips());
            }

            IEnumerator DestroyAllTurnips()
            {
                yield return new WaitForSeconds(1f);
                foreach (var t in FindObjectsOfType<GameplayTurnip>())
                {
                    Destroy(t.gameObject);
                }
            }
        }

        private class GameOver : State
        {
            private Data data;

            public GameOver(MonoBehaviour c, System.Action<State> changeState, Data data) : base(c, changeState)
            {
                this.data = data;
            }

            public override void OnEnter()
            {
                data.uiBgAnim.Play("UI Reveal");

                var isNewHighScore = data.points > data.highScore;
                if (isNewHighScore)
                {
                    data.pointsTextMesh.text = $"{data.points}  •  NEW HIGH SCORE!";
                    data.highScore = data.points;
                }

                c.StartCoroutine(ChangeStateTimeout());
            }

            IEnumerator ChangeStateTimeout()
            {
                yield return new WaitForSeconds(2f);
                changeState(new MenuState(c, changeState, data));
            }
        }

        [System.Serializable]
        public class Data
        {
            public Animation uiBgAnim;
            public List<GameObject> gameplayTurnipPrefabs;
            public Transform turnipSpawnPoint;
            public int lives;
            public TextMesh pointsTextMesh;
            public TextMesh startText;
            public HeartsManager hearsManager;
            public Bar bar;

            public int points;
            public int highScore;
        }

        [SerializeField]
        private Data data;

        private State currentState;

        private void Start()
        {
            data.highScore = 0;
            data.hearsManager.Initialise(data.lives);
            data.pointsTextMesh.text = "";
            ChangeState(new MenuState(this, (newState) => ChangeState(newState), data));
        }

        private void ChangeState(State newState)
        {
            currentState?.OnExit();
            newState.OnEnter();
            currentState = newState;
        }

        private void Update()
        {
            currentState?.OnUpdate();
        }
    }
}
