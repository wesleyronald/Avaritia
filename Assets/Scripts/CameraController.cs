using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using UnityEngine.UI;

public class CameraController : MonoBehaviour 
{
    [Range (0f, 20f)]
    public float cameraSpeed = 3f;
    public Vector2 targetResolution;
    public float heightInWorldUnits = 9f, widthInWorldUnits = 16f;
    public Vector3 cameraOffset;

    #if UNITY_WEBGL
    private bool fullScreen = false;
    #endif
    private Camera mainCamera;
    private GGEZ.PerfectPixelCamera perfectPixelCamera;
    private float xCameraExtent, yCameraExtent, aspectRatio, startingOrthographicSize, currentWidth, currentHeight, currentAspect, targetAspect;
    private Vector3 newPos;
    public GameObject player;

    /*[MenuItem("Screenshot/Take screenshot")]
    static void Screenshot()
    {
        ScreenCapture.CaptureScreenshot("test.png");
    }*/

	// Use this for initialization
	void Start () 
    {
        mainCamera = GetComponent<Camera>();
        perfectPixelCamera = GetComponent<GGEZ.PerfectPixelCamera>();
        #if UNITY_WEBGL
        fullScreen = Screen.fullScreen;
        #endif
        startingOrthographicSize = mainCamera.orthographicSize;
        targetAspect = targetResolution.x / targetResolution.y;
        ResizeCamera();
	}
	
	// Update is called once per frame
	void Update () 
    {
        #if UNITY_WEBGL
        if (fullScreen != Screen.fullScreen)
        {
            fullScreen = Screen.fullScreen;
            ResizeCamera();
        }
        #endif
        if (player == null || Time.timeScale == 0f)
        {
            return;
        }
		newPos = new Vector3(player.transform.position.x, player.transform.position.y, 0f) + cameraOffset;
      	transform.position = Vector3.MoveTowards(transform.position, newPos, cameraSpeed * Time.deltaTime);
	}

    public void ResizeCamera()
    {
        #if UNITY_WEBGL
        if (fullScreen)
        {
            currentWidth = (float)Screen.currentResolution.width;
            currentHeight = (float)Screen.currentResolution.height;
        }else
        {
            currentWidth = (float)mainCamera.pixelWidth;
            currentHeight = (float)mainCamera.pixelHeight;
        }
        #else
        currentWidth = (float)mainCamera.pixelWidth;
        currentHeight = (float)mainCamera.pixelHeight;
        #endif
        currentAspect = currentWidth / currentHeight;
        if (mainCamera.aspect >= targetAspect)
        {
            perfectPixelCamera.TexturePixelsPerWorldUnit = Mathf.CeilToInt(currentHeight / heightInWorldUnits);
        }
        else
        {
			//remember to anchor and pivot UI Elements at middle and set Canvas to Expand mode
            perfectPixelCamera.TexturePixelsPerWorldUnit = Mathf.CeilToInt(currentWidth / widthInWorldUnits);
			mainCamera.orthographicSize = startingOrthographicSize * targetAspect / currentAspect;
        }
    }
}
