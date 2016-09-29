using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConsoleHandler : MonoBehaviour {

    private Canvas canvas;
    private InputField consoleInput;
    private Text consoleText;
    private GameObject consoleBackground;
    private Canvas UICanvas;


    // Use this for initialization
    void Start () {
        canvas = gameObject.GetComponent<Canvas>();// GameObject.Find("ConsoleCanvas").GetComponent<Canvas>();
        consoleInput = GameObject.Find("ConsoleInput").GetComponent<InputField>();
        consoleText = GameObject.Find("ConsoleText").GetComponent<Text>();
        consoleBackground = GameObject.Find("ConsoleBackground");
        UICanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Return) && consoleInput.text != "") {
            string val = consoleInput.text;
            consoleText.text += "\n> " + val;
            consoleInput.text = "";
            processCommand(val);
            consoleInput.ActivateInputField();
        }
    }

    void processCommand(string c) {
        string cmd = c.ToLower();
        string[] cmds = cmd.Split(' ');
        if (cmds[0].Equals("exit")) {
            canvas.gameObject.SetActive(false);
        } else if (cmds[0].Equals("help")) {
            printHelp();
        } else if (cmds[0].Equals("background")) {
            if (cmds.Length == 2) {
                char[] hexs = cmds[1].ToCharArray();
                if (hexs.Length == 6 && isHexChars(hexs)) {
                    int val1 = System.Int32.Parse(new string(new char[] { hexs[0], hexs[1] }), System.Globalization.NumberStyles.AllowHexSpecifier);
                    int val2 = System.Int32.Parse(new string(new char[] { hexs[2], hexs[3] }), System.Globalization.NumberStyles.AllowHexSpecifier);
                    int val3 = System.Int32.Parse(new string(new char[] { hexs[4], hexs[5] }), System.Globalization.NumberStyles.AllowHexSpecifier);
                    consoleBackground.GetComponent<Image>().color = new Color32((byte)val1, (byte)val2, (byte)val3, 200);
                } else {
                    printError("Wrong amount of HEX chars! Must be exactly 6: \"451278\" or \"F15A28\".");
                }
            } else {
                printError("Wrong number of arguments!");
            }
        } else if (cmds[0].Equals("pointstowin")) {
            if (cmds.Length == 2) {
                int number = System.Int32.Parse(cmds[1]);
                GameObject.Find("UICanvas").GetComponent<UIHandler>().pointsToWin = number;
                printMessage("Max points to win changed to " + number);
            }
        } else if (cmds[0].Equals("ballspeed")) {
            int number = System.Int32.Parse(cmds[1]);
            GameObject.Find("Ball").GetComponent<Ball>().maxSpeed = number;
            printMessage("Speed changed to " + number);
        }
    }

    void printMessage(string s) {
        consoleText.text += "\n  " + s;
    }

    void printError(string err) {
        consoleText.text += "\n  <color=red>" + err + "</color>";
    }

    void printHelp() {
        consoleText.text += "\n     Commands:";
        consoleText.text += "\n     Help: This list.";
        consoleText.text += "\n     Exit: Exit the console.";
        consoleText.text += "\n     Background [HEX color value]: Change the background color of the command window.";
        consoleText.text += "\n     Pointstowin [points]: Changes the amount of points required to win a game, default is 10.";
        consoleText.text += "\n     Ballspeed [speed]: Changes the speed of the ball, default is 8.";
    }

    private bool isHexChars(char[] c) {
        for (int i = 0; i < c.Length; i++) {
            if (!isHex(c[i]))
                return false;
        }
        return true;
    }

    private bool isHex(char c) {
        return (c >= '0' && c <= '9') ||
                 (c >= 'a' && c <= 'f') ||
                 (c >= 'A' && c <= 'F');
    }
}