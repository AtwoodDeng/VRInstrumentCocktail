using UnityEngine;
using System.Collections;

public class LogicManager : MonoBehaviour {
	
	public LogicManager() { s_Instance = this; }
	public static LogicManager Instance { get { return s_Instance; } }
	private static LogicManager s_Instance;

	void LateUpdate() {
		Cardboard.SDK.UpdateState();
		if (Cardboard.SDK.BackButtonPressed) {
			Application.Quit();
		}
	}

	public void ToggleVRMode() {
		Cardboard.SDK.VRModeEnabled = !Cardboard.SDK.VRModeEnabled;
	}

	public void ToggleDistortionCorrection() {
		switch(Cardboard.SDK.DistortionCorrection) {
		case Cardboard.DistortionCorrectionMethod.Unity:
			Cardboard.SDK.DistortionCorrection = Cardboard.DistortionCorrectionMethod.Native;
			break;
		case Cardboard.DistortionCorrectionMethod.Native:
			Cardboard.SDK.DistortionCorrection = Cardboard.DistortionCorrectionMethod.None;
			break;
		case Cardboard.DistortionCorrectionMethod.None:
		default:
			Cardboard.SDK.DistortionCorrection = Cardboard.DistortionCorrectionMethod.Unity;
			break;
		}
	}

	public void ToggleDirectRender() {
		Cardboard.Controller.directRender = !Cardboard.Controller.directRender;
	}

	public void TeleportRandomly() {
		Vector3 direction = Random.onUnitSphere;
		direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
		float distance = 2 * Random.value + 1.5f;
		transform.localPosition = direction * distance;
	}

}
