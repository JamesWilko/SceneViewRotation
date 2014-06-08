using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class SceneViewRotation {

	const float ROTATION_AMT = 5f;
	const float BUTTON_C_X = 60f;
	const float BUTTON_SPACING = 0f;
	const float BUTTON_HEIGHT_OFFSET = 106f;
	const float BUTTON_WIDTH = 22f;
	const float BUTTON_HEIGHT = 22f;
	const string PATH = "Assets/Plugins/SceneViewRotation/Resources/";
	const string ROTATION_TOOLTIP = "Rotate view {0} by {1}�";
	const string RESET_TOOLTIP = "Reset the camera rotation";

	static Texture2D rotateLeftIcon;
	static Texture2D RotateLeftIcon {
		get {
			if ( rotateLeftIcon == null ) {
				rotateLeftIcon = Resources.LoadAssetAtPath( PATH + "RotateLeftIcon.png", typeof( Texture2D ) ) as Texture2D;
			}
			return rotateLeftIcon;
		}
	}
	static GUIContent RotateLeftContent = null;

	static Texture2D rotateRightIcon;
	static Texture2D RotateRightIcon {
		get {
			if ( rotateRightIcon == null ) {
				rotateRightIcon = Resources.LoadAssetAtPath( PATH + "RotateRightIcon.png", typeof( Texture2D ) ) as Texture2D;
			}
			return rotateRightIcon;
		}
	}
	static GUIContent RotateRightContent = null;

	static Texture2D rotateResetIcon;
	static Texture2D RotateResetIcon {
		get {
			if ( rotateResetIcon == null ) {
				rotateResetIcon = Resources.LoadAssetAtPath( PATH + "RotateResetIcon.png", typeof( Texture2D ) ) as Texture2D;
			}
			return rotateResetIcon;
		}
	}
	static GUIContent RotateResetContent = null;

	static float cameraZRotation = 0f;
	static bool resetCameraRotation = false;
	static bool didUpdateRotation = false;

	static SceneViewRotation() {
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

	~SceneViewRotation(){
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}

	static void OnSceneGUI( SceneView sceneView ) {

		Handles.BeginGUI();

		if ( GUI.Button( new Rect( sceneView.position.width - BUTTON_C_X - BUTTON_SPACING - BUTTON_WIDTH, BUTTON_HEIGHT_OFFSET, BUTTON_WIDTH, BUTTON_HEIGHT ), GetRotateLeftContent() ) ) {
			RotateLeft();
		}

		if ( GUI.Button( new Rect( sceneView.position.width - BUTTON_C_X, BUTTON_HEIGHT_OFFSET, BUTTON_WIDTH, BUTTON_HEIGHT ), GetRotateResetContent() ) ) {
			RotateReset();
		}

		if ( GUI.Button( new Rect( sceneView.position.width - BUTTON_C_X + BUTTON_WIDTH + BUTTON_SPACING, BUTTON_HEIGHT_OFFSET, BUTTON_WIDTH, BUTTON_HEIGHT ), GetRotateRightContent() ) ) {
			RotateRight();
		}

		Handles.EndGUI();
		
		if( SceneView.lastActiveSceneView.camera ){

			Camera cam = SceneView.lastActiveSceneView.camera;
			Transform camTransform = cam.transform;
			Quaternion rotation = camTransform.localRotation;

			if ( resetCameraRotation ) {
				cam.transform.localRotation = Quaternion.identity;
				rotation = camTransform.localRotation;
				resetCameraRotation = false;
			}

			rotation = Quaternion.Euler( rotation.eulerAngles.x, rotation.eulerAngles.y, cameraZRotation );
			cam.transform.localRotation = rotation;

			if ( didUpdateRotation ) {
				didUpdateRotation = false;
				cam.Render();
			}

		}

	}

	static void RotateLeft() {
		cameraZRotation -= ROTATION_AMT;
		didUpdateRotation = true;
	}

	static void RotateRight() {
		cameraZRotation += ROTATION_AMT;
		didUpdateRotation = true;
	}

	static void RotateReset() {
		cameraZRotation = 0f;
		resetCameraRotation = true;
		didUpdateRotation = true;
	}

	static GUIContent GetRotateLeftContent() {
		if( RotateLeftContent == null ){
			string tooltip = string.Format( ROTATION_TOOLTIP, "left", ROTATION_AMT );
			if ( RotateLeftIcon ) {
				RotateLeftContent = new GUIContent( RotateLeftIcon, tooltip );
			} else {
				RotateLeftContent = new GUIContent( "<-", tooltip );
			}
		}
		return RotateLeftContent;
	}

	static GUIContent GetRotateRightContent() {
		if ( RotateRightContent == null ) {
			string tooltip = string.Format( ROTATION_TOOLTIP, "right", ROTATION_AMT );
			if ( RotateRightIcon ) {
				RotateRightContent = new GUIContent( RotateRightIcon, tooltip );
			} else {
				RotateRightContent = new GUIContent( "->", tooltip );
			}
		}
		return RotateRightContent;
	}

	static GUIContent GetRotateResetContent() {
		if ( RotateResetContent == null ) {
			if ( RotateResetIcon ) {
				RotateResetContent = new GUIContent( RotateResetIcon, RESET_TOOLTIP );
			} else {
				RotateResetContent = new GUIContent( "R", RESET_TOOLTIP );
			}
		}
		return RotateResetContent;
	}

}
