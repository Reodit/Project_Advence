using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities
{
    public static class UIToolkit
    {
        /// <summary>
        /// canvas를 갖고 있는 게임 오브젝트 활성화
        /// </summary>
        /// <param name="canvasList"></param>
        public static void ShowUI(List<Canvas> canvasList)
        {
            foreach (var canvas in canvasList.Where(canvas => canvas != null))
            {
                canvas.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// canvas를 갖고 있는 게임 오브젝트 비활성화
        /// </summary>
        /// <param name="canvasList"></param>
        public static void HideUI(List<Canvas> canvasList)
        {
            foreach (var canvas in canvasList.Where(canvas => canvas != null))
            {
                canvas.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// RectTransform을 갖고 있는 게임 오브젝트 비활성화
        /// </summary>
        /// <param name="rectTransformList"></param>
        public static void HideUI(List<RectTransform> rectTransformList)
        {
            foreach (var rectTransform in rectTransformList.Where(rectTransform => rectTransform != null))
            {
                rectTransform.gameObject.SetActive(!rectTransform.gameObject.activeSelf);
            }
        }

        /// <summary>
        /// canvas를 갖고 있는 게임 오브젝트가 활성화 되어있다면, 비활성화
        /// 비활성화 되어있다면 활성화
        /// </summary>
        /// <param name="canvasList"></param>
        public static void ToggleUI(List<Canvas> canvasList)
        {
            foreach (var canvas in canvasList.Where(canvas => canvas != null))
            {
                canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
            }
        }

        /// <summary>
        /// RectTransform을 갖고 있는 게임 오브젝트가 활성화 되어있다면, 비활성화
        /// 비활성화 되어있다면 활성화
        /// </summary>  
        /// <param name="rectTransformList"></param>
        public static void ToggleUI(List<RectTransform> rectTransformList)
        {
            foreach (var rectTransform in rectTransformList.Where(rectTransform => rectTransform != null))
            {
                rectTransform.gameObject.SetActive(!rectTransform.gameObject.activeSelf);
            }
        }
    }
}
