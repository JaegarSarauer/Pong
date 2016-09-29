using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    private Text playerText;
    private const string playerTextConst = "Player: ";
    private Canvas console;

    private Text winText;

    private Text scoresText;

    private int p1Score = 0;
    private int p2Score = 0;
    
    public bool playerOne;
    public int pointsToWin = 10;

    private Button restart2PButton;
    private Button restartNPCButton;
    private Button startGamePointButton;
    private Button endGameButton;

    GameObject ballObject;
    Rigidbody ballRigidbody;
    Vector3 ballStartPosition;

    GameObject paddleObject;
    
    public bool canMove = false;

    // Use this for initialization
    void Start () {
        playerText = GameObject.Find("PlayerText").GetComponent<Text>();
        scoresText = GameObject.Find("ScoresText").GetComponent<Text>();
        winText = GameObject.Find("WinText").GetComponent<Text>();
        console = GameObject.Find("ConsoleCanvas").GetComponent<Canvas>();
        console.gameObject.SetActive(false);

        restart2PButton = GameObject.Find("Restart2PButton").GetComponent<Button>();
        restart2PButton.onClick.AddListener(delegate () { restart2PButtonOnClick(); });
        restart2PButton.gameObject.SetActive(false);

        restartNPCButton = GameObject.Find("RestartNPCButton").GetComponent<Button>();
        restartNPCButton.onClick.AddListener(delegate () { restartNPCButtonOnClick(); });
        restartNPCButton.gameObject.SetActive(false);

        endGameButton = GameObject.Find("EndGameButton").GetComponent<Button>();
        endGameButton.onClick.AddListener(delegate () { endGameButtonOnClick(); });
        endGameButton.gameObject.SetActive(false);

        startGamePointButton = GameObject.Find("StartGamePointButton").GetComponent<Button>();
        startGamePointButton.onClick.AddListener(delegate () { startGamePointButtonOnClick(); });
        startGamePointButton.gameObject.SetActive(false);

        winText.enabled = false;
        ballObject = GameObject.Find("Ball");
        ballRigidbody = ballObject.GetComponent<Rigidbody>();
        ballStartPosition = new Vector3(0, 10, 10);

        paddleObject = GameObject.Find("Paddle");

        resetBall();
        showMenu();
    }
	
	// Update is called once per frame
	void Update () {
        scoresText.text = "Red: " + p1Score + "\nBlue: " + p2Score;
        if (Input.GetKeyUp(KeyCode.C) && !console.gameObject.activeSelf && !canMove) {
            console.gameObject.SetActive(true);
            GameObject.Find("ConsoleInput").GetComponent<InputField>().ActivateInputField();
        }
        if (console.gameObject.activeSelf ||
                restart2PButton.gameObject.activeSelf ||
                restartNPCButton.gameObject.activeSelf ||
                startGamePointButton.gameObject.activeSelf ||
                endGameButton.gameObject.activeSelf)
            canMove = false;
        else
            canMove = true;
	}

    public void changePlayer() {
        string v = (playerOne) ? "Blue (WASD)" : "Red (Arrow Keys)";
        playerText.text = playerTextConst + v;
    }

    public void givePoint() {
        if (playerOne) {
            p1Score++;
        } else {
            p2Score++;
        }
        resetBall();
        if (!checkWinner()) {
            startGamePointButton.gameObject.SetActive(true);
        } 
    }

    private void showMenu() {
        winText.text = "Choose an Option";
        winText.enabled = true;
        restart2PButton.gameObject.SetActive(true);
        restartNPCButton.gameObject.SetActive(true);
    }

    private void hideMenu() {
        winText.enabled = false;
        restart2PButton.gameObject.SetActive(false);
        restartNPCButton.gameObject.SetActive(false);
    }

    private void showEndGame(string msg) {
        winText.text = msg;
        winText.enabled = true;
        endGameButton.gameObject.SetActive(true);
    }

    private void hideEndGame() {
        winText.enabled = false;
        endGameButton.gameObject.SetActive(false);
    }

    //returns true if there was a winner, false if no winner
    private bool checkWinner() {
        bool win = false;
        if (p1Score >= pointsToWin) {
            showEndGame("Red Wins!");
            win = true;
        } else if (p2Score >= pointsToWin) {
            showEndGame("Blue Wins!");
            win = true;
        }
        if (!win)
            return false;
        return true;
    }

    public void resetBall() {
        ballObject.transform.position = ballStartPosition;
        ballObject.gameObject.GetComponent<Ball>().canMove = false;
        ballObject.GetComponent<Rigidbody>().useGravity = false;
        ballObject.GetComponent<Rigidbody>().isKinematic = true;
        ballRigidbody.velocity = Vector3.zero;
    }

    public void startBall() {
        resetBall();
        ballObject.GetComponent<Rigidbody>().useGravity = true;
        ballObject.GetComponent<Rigidbody>().isKinematic = false;
        ballRigidbody.velocity = randomStartVelocity();
        ballObject.gameObject.GetComponent<Ball>().canMove = true;
    }

    private Vector3 randomStartVelocity() {
        return new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
    }

    public void restart2PButtonOnClick() {
        paddleObject.GetComponent<MovePaddle>().isAI = false;
        hideMenu();
        startGamePointButton.gameObject.SetActive(true);
    }

    public void restartNPCButtonOnClick() {
        paddleObject.GetComponent<MovePaddle>().isAI = true;
        hideMenu();
        startGamePointButton.gameObject.SetActive(true);
    }

    public void startGamePointButtonOnClick() {
        startBall();
        startGamePointButton.gameObject.SetActive(false);
    }

    private void endGameButtonOnClick() {
        hideEndGame();
        resetBall();
        p1Score = 0;
        p2Score = 0;
        showMenu();
    }

}
