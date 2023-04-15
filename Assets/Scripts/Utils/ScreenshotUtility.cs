using System.IO;
using UnityEngine;

public class ScreenshotUtility : MonoBehaviour
{
    #if UNITY_EDITOR

    private static string folderPath => Path.Combine(Application.persistentDataPath, "Screenshots");
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Screenshot();
        }
    }

    [UnityEditor.MenuItem("Tools/Game/Screenshot")]
    private static void Screenshot()
    {
        Debug.Log(folderPath);
        CheckIfFolderExists();
        int index = Directory.GetFiles(folderPath).Length;
        while (File.Exists(ScreenshotPath(index)))
        {
            index++;
        }
        Debug.Log($"Capturing screenshot to {ScreenshotPath(index)}");
        ScreenCapture.CaptureScreenshot(ScreenshotPath(index));
    }

    private static string ScreenshotPath(int index)
    {
        return $"{folderPath}/screen{index}.png";
    }

    private static void CheckIfFolderExists()
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

#endif
}
