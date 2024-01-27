using UnityEngine;

namespace Utilities
{
    public static class ScreenshotUtility
    {
        public static void TakeScreenshot()
        {
            string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string screenshotFileName = "Screenshot_" + timestamp + ".png";
            string filePath = Application.persistentDataPath + "/" + screenshotFileName;

            ScreenCapture.CaptureScreenshot(filePath);
            Debug.Log("Screenshot saved to: " + filePath);
        }
    }
}
