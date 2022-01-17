using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace PaperPlaneTools.AR {
	using OpenCvSharp;

	using UnityEngine;
	using System.Collections;
	using System.Runtime.InteropServices;
	using System;
	using System.Collections.Generic;
	using UnityEngine.UI;
	
	public class MarkerManager : WebCamera {
	

		/// <summary>
		/// Arma controlada por el marcador
		/// set in Unity Inspector
		/// </summary>
		public GameObject pistola;

		private GameObject auxiliarGO;
		[Range(0.0f,1.0f)]
		public float weight = 0.01f;
		
		private Vector3 RotationOffset;
		private Vector3 old_forward3, old_right3;
		/// <summary>
		/// Marcador que corresponde con el arma
		/// </summary>
		public int markerId;
		
		/// <summary>
		/// The marker detector
		/// </summary>
		private MarkerDetector markerDetector;
		

		void Start ()
		{
			
			auxiliarGO = new GameObject();
			auxiliarGO.transform.position = pistola.transform.position;
			auxiliarGO.transform.rotation = pistola.transform.rotation;
			auxiliarGO.transform.parent = pistola.transform.parent;
			
			old_forward3 = auxiliarGO.transform.forward;
			old_right3 = auxiliarGO.transform.right;
			
			int cameraIndex = -1;
			for (int i = 0; i < WebCamTexture.devices.Length; i++) {
				WebCamDevice webCamDevice = WebCamTexture.devices [i];
				if (webCamDevice.isFrontFacing == false) {
					cameraIndex = i;
					break;
				}
				if (cameraIndex < 0) {
					cameraIndex = i;
				}
			}

			if (cameraIndex >= 0) {
				DeviceName = WebCamTexture.devices [cameraIndex].name;
				//webCamDevice = WebCamTexture.devices [cameraIndex];
			}
			
			Debug.Log(cameraIndex);

			markerDetector = new MarkerDetector ();
		}
		
		private void FixedUpdate()
		{
			// obj3's orientation is a weighted average of obj2's current orientation and obj3's old orientation
			Vector3 new_forward3 = ((weight)*auxiliarGO.transform.forward + (1-weight)*pistola.transform.forward);
			Vector3 new_right3 = ((weight)*auxiliarGO.transform.right + (1-weight)*pistola.transform.right);

			//Debug.Log(pistola.transform.forward + " " + auxiliarGO.transform.forward + " " + new_forward3 + " " + old_forward3);
			
			// Use a cross-product to makes sure new_up3 is perpendicular to new_forward3.
			Vector3 new_up3 = Vector3.Cross( new_forward3, new_right3 );
			//Debug.Log(new_forward3 + " / "  + new_right3 + " / " + new_up3);
			pistola.transform.rotation = Quaternion.LookRotation(new_forward3.normalized, new_up3.normalized);
			
			//old_forward3 = pistola.transform.forward;
			//old_right3 = pistola.transform.right;
		}

		protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output) {
			Mat img = Unity.TextureToMat (input, TextureParameters);
			ProcessFrame(img, img.Cols, img.Rows);
			output = Unity.MatToTexture(img, output);
			return true;
		}

		private void ProcessFrame (Mat mat, int width, int height) {
			List<int> markerIds = markerDetector.Detect (mat, width, height);

			for (int i = 0; i < markerIds.Count; i++)
			{
				if (markerIds[i] == markerId)
				{
					
					Matrix4x4 transforMatrix = markerDetector.TransfromMatrixForIndex(i);
					UpdateGun(transforMatrix);
					break;
				}
			}
			
			/*
			int count = 0;
			foreach (MarkerObject markerObject in markers) {
				List<int> foundedMarkers = new List<int>();
				for (int i=0; i<markerIds.Count; i++) {
					if (markerIds[i] == markerObject.markerId) {
						foundedMarkers.Add(i);
						count++;
					}
				}

				ProcessMarkesWithSameId(markerObject, gameObjects[markerObject.markerId], foundedMarkers);
			}*/
		}

		private void UpdateGun(Matrix4x4 transformMatrix)
		{
			//var localRotation = pistola.transform.localRotation;
			
			Matrix4x4 matrixY = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, -1, 1));
			Matrix4x4 matrixZ = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, 1, -1));
			Matrix4x4 matrix = matrixY * transformMatrix * matrixZ;

			Vector3 newPos = MatrixHelper.GetPosition (matrix);
			//newPos.z = pistola.transform.localPosition.z;
			//pistola.transform.localPosition = new Vector3(Mathf.Clamp(newPos.x, minX, maxX), Mathf.Clamp(newPos.y, minY, maxY), Mathf.Clamp(newPos.z, minZ, maxZ));
			pistola.transform.localPosition = newPos;
			Quaternion target = MatrixHelper.GetQuaternion (matrix);
			//target = Quaternion.Inverse(target);
			//target.eulerAngles += RotationOffset;
			//pistola.transform.localRotation = target;
			auxiliarGO.transform.localRotation = target;


			//Debug.Log(newPos);
			//Debug.Log(target.eulerAngles);
			//localRotation = Quaternion.Inverse(target);
			//localRotation = Quaternion.Euler(new Vector3(localRotation.eulerAngles.x,
			//	localRotation.eulerAngles.y, localRotation.eulerAngles.z));
			//pistola.transform.localRotation = localRotation;
			//pistola.transform.rotation = Quaternion.Euler(pistola.transform.rotation.eulerAngles + RotationOffset);

			//gameObject.transform.localScale = MatrixHelper.GetScale (matrix);
		}
		/*
		private void ProcessMarkesWithSameId(MarkerObject markerObject, List<MarkerOnScene> gameObjects, List<int> foundedMarkers) {
			int index = 0;
		
			index = gameObjects.Count - 1;
			while (index >= 0) {
				MarkerOnScene markerOnScene = gameObjects[index];
				markerOnScene.bestMatchIndex = -1;
				if (markerOnScene.destroyAt > 0 && markerOnScene.destroyAt < Time.fixedTime) {
					Destroy(markerOnScene.gameObject);
					gameObjects.RemoveAt(index);
				}
				--index;
			}

			index = foundedMarkers.Count - 1;

			// Match markers with existing gameObjects
			while (index >= 0) {
				int markerIndex = foundedMarkers[index];
				Matrix4x4 transforMatrix = markerDetector.TransfromMatrixForIndex(markerIndex);
				Vector3 position = MatrixHelper.GetPosition(transforMatrix);

				float minDistance = float.MaxValue;
				int bestMatch = -1;
				for (int i=0; i<gameObjects.Count; i++) {
					MarkerOnScene markerOnScene = gameObjects [i];
					if (markerOnScene.bestMatchIndex >= 0) {
						continue;
					}
					float distance = Vector3.Distance(markerOnScene.gameObject.transform.position, position);
					if (distance<minDistance) {
						bestMatch = i;
					}
				}

				if (bestMatch >=0) {
					gameObjects[bestMatch].bestMatchIndex = markerIndex;
					foundedMarkers.RemoveAt(index);
				} 
				--index;
			}

			//Destroy excessive objects
			index = gameObjects.Count - 1;
			while (index >= 0) {
				MarkerOnScene markerOnScene = gameObjects[index];
				if (markerOnScene.bestMatchIndex < 0) {
					if (markerOnScene.destroyAt < 0) {
						markerOnScene.destroyAt = Time.fixedTime + 0.2f;
					}
				} else {
					markerOnScene.destroyAt = -1f;
					int markerIndex = markerOnScene.bestMatchIndex;
					Matrix4x4 transforMatrix = markerDetector.TransfromMatrixForIndex(markerIndex);
					PositionObject(markerOnScene.gameObject, transforMatrix);
				}
				index--;
			}

			//Create objects for markers not matched with any game object
			foreach (int markerIndex in foundedMarkers) {
				GameObject gameObject = Instantiate(markerObject.markerPrefab);
				MarkerOnScene markerOnScene = new MarkerOnScene() {
					gameObject = gameObject
				};
				gameObjects.Add(markerOnScene);

				Matrix4x4 transforMatrix = markerDetector.TransfromMatrixForIndex(markerIndex);
				PositionObject(markerOnScene.gameObject, transforMatrix);
			}
		}

		private void PositionObject(GameObject gameObject, Matrix4x4 transformMatrix) {
			Matrix4x4 matrixY = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, -1, 1));
			Matrix4x4 matrixZ = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, 1, -1));
			Matrix4x4 matrix = matrixY * transformMatrix * matrixZ;

			gameObject.transform.localPosition = MatrixHelper.GetPosition (matrix);
			gameObject.transform.localRotation = MatrixHelper.GetQuaternion (matrix);
			gameObject.transform.localScale = MatrixHelper.GetScale (matrix);
		}*/
	}
}
