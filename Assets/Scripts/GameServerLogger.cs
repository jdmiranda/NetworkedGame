using UnityEngine;
using System.Collections;

public class GameServerLogger : MonoBehaviour {
	public string output = "";
	public string stack = "";
	void OnEnable() {
		Application.RegisterLogCallback(HandleLog);
	}
	void OnDisable() {
		Application.RegisterLogCallback(null);
	}
	void HandleLog(string logString, string stackTrace, LogType type) {
		output = logString;
		stack = stackTrace;
		System.Console.WriteLine(output);
		System.Console.WriteLine(stack);
	}
}
