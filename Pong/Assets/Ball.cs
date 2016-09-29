using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
    private Rigidbody rigid;
    private GameObject paddle;
    private MovePaddle paddleScript;
    [HideInInspector]
    public int maxSpeed = 8;
    private UIHandler UIhandler;

    public bool canMove = false;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
        paddle = GameObject.Find("Paddle");
        paddleScript = paddle.GetComponent<MovePaddle>();
        UIhandler = GameObject.Find("UICanvas").GetComponent<UIHandler>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = gameObject.transform.position;
        if (pos.z >= 50) {
            UIhandler.givePoint();
        } else if (pos.x >= 12 || pos.x <= -12 || pos.y >= 22 || pos.y <= -2 || pos.z <= -12) {
            UIhandler.resetBall();
        }
    }

    void FixedUpdate() {
        if (canMove) {
            Vector3 velocity = rigid.velocity;
            if (velocity.z < 0 && velocity.z > -maxSpeed)
                rigid.AddForce(new Vector3(0, 0, -maxSpeed));
            else if (velocity.z >= 0 && velocity.z < maxSpeed)
                rigid.AddForce(new Vector3(0, 0, maxSpeed));
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "BackWall") {
            UIhandler.playerOne = !UIhandler.playerOne;
            paddleScript.switchedPlayer = false;
        }
    }

}
