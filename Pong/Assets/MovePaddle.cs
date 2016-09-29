using UnityEngine;
using System.Collections;

public class MovePaddle : MonoBehaviour {
    private Rigidbody rigid;
    private Camera gameCamera;
    private GameObject ball;
    private Rigidbody ballRigid;
    public bool switchedPlayer = false;
    private UIHandler UIhandler;
    private ControllerInput osType;

    public bool isAI;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
        gameCamera = GameObject.Find("gameCamera").GetComponent<Camera>();
        Vector3 start = gameObject.transform.position;
        start.z += 10;
        gameCamera.transform.position = start;
        UIhandler = GameObject.Find("UICanvas").GetComponent<UIHandler>();
        ball = GameObject.Find("Ball");
        ballRigid = ball.GetComponent<Rigidbody>();
        osType = GetComponent<ControllerInput>();
    }

    Quaternion getCameraFacingAngle(Transform paddle, Transform camera) {
        Vector3 relativePos = paddle.position - camera.position;
        return Quaternion.LookRotation(relativePos);
    }
	
	// Update is called once per frame
	void Update () {
        if (!UIhandler.canMove)
            return;
        Quaternion rotate = getCameraFacingAngle(gameObject.transform, gameCamera.transform);
        gameCamera.transform.rotation = rotate;
        gameObject.transform.rotation = rotate;
        Vector3 move = rigid.velocity;
        Vector3 pos = gameObject.transform.position;
        if (!isAI || (isAI && !UIhandler.playerOne)) {
            if (osType.OS == ControllerInput.OSType.MOBILE) {
                Vector3 newPos = new Vector3(10, 0, 25);
                int xPart = Screen.width / 20;
                int yPart = Screen.height / 20;
                newPos.y += (Input.mousePosition.y / yPart);
                newPos.x -= (Input.mousePosition.x / xPart);
                newPos.x = Mathf.Clamp(newPos.x, -10, 10);
                newPos.y = Mathf.Clamp(newPos.y, 0, 20);
                Debug.Log((Screen.width) + " : " + (Screen.height));
                gameObject.transform.position = newPos;
            } else {
                move.y = 30 * Input.GetAxis("Vertical");
                move.x = 30 * Input.GetAxis("Horizontal") * -1;
            }
        } else if (isAI && UIhandler.playerOne) {//AI movement
            Debug.Log("ASDFSDFSGHDFHG");
            Vector3 bV = ball.transform.position;
            if (bV.z <= pos.z && ballRigid.velocity.z >= 0) {
                move.x = (bV.x - pos.x) * 3;
                move.y = (bV.y - pos.y) * 3;
            } else {
                move.x = move.y = 0;
            }
        }
        rigid.velocity = move;
        checkPlayer();
    }

    private void checkPlayer() {
        if (switchedPlayer)
            return;
        switchedPlayer = true;
        Color curColor = GetComponent<Renderer>().material.color;
        if (UIhandler.playerOne) {
            curColor = Color.blue;
            curColor.a = .5f;
            GetComponent<Renderer>().material.color = curColor;
        } else {
            curColor = Color.red;
            curColor.a = .5f;
            GetComponent<Renderer>().material.color = curColor;
        }
        UIhandler.changePlayer();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Ceiling") {
            rigid.velocity = Vector3.zero;
        }
    }
}