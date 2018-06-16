using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace DogHouse.Jalloo.Splash
{
    /// <summary>
    /// The Splash Screen Player will play a series of 
    /// splash images in order and invoke an event when
    /// all the images are finished playing.
    /// </summary>
    public class SplashScreenPlayer : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        private Camera m_camera;

        [SerializeField]
        private Image m_image;

        [SerializeField]
        private SplashScreen[] m_splashImages;

        [SerializeField]
        private UnityEvent m_onFinishedSplashImages;

        private Color m_backgroundColor = Color.black;
        private const float BACKGROUND_LERP_TIME = 1f;
        private const float IMAGE_LERP_TIME = 0.5f;
        #endregion

        #region Main Methods
        private void Start() => StartCoroutine(_PlaySequence());
        #endregion

        #region Utility Methods
        private IEnumerator _PlaySequence()
        {
            float tValue = 0f;
            Color startColor = m_backgroundColor;

            m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, 0f);

            for (int i = 0; i < m_splashImages.Length; i++)
            {
                do
                {
                    LerpBackgroundColor(ref tValue, startColor, m_splashImages[i].BackgroundColor);
                    yield return null;
                } while (tValue < 1f);

                m_image.overrideSprite = m_splashImages[i].Image;
                m_image.rectTransform.sizeDelta = m_splashImages[i].m_imageSize;
                tValue = 0f;

                do
                {
                    LerpImageIn(ref tValue);
                    yield return null;
                } while (tValue < 1f);

                yield return new WaitForSeconds(m_splashImages[i].TimeOnScreen);

                tValue = 0f;

                do
                {
                    LerpImageOut(ref tValue);
                    yield return null;
                } while (tValue < 1f);

                startColor = m_camera.backgroundColor;
                tValue = 0f;
            }

            do
            {
                LerpBackgroundColor(ref tValue, startColor, Color.black);
                yield return null;
            } while (tValue < 1f);
                

            m_onFinishedSplashImages?.Invoke();
        }

        private void LerpBackgroundColor(ref float TValue, Color StartColor, Color TargetColor)
        {
            TValue += Time.deltaTime / BACKGROUND_LERP_TIME;
            m_backgroundColor = Color.Lerp(StartColor, TargetColor, TValue);
            m_camera.backgroundColor = m_backgroundColor;
        }

        private void LerpImageOut(ref float TValue)
        {
            TValue += Time.deltaTime / IMAGE_LERP_TIME;
            m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, 1f - TValue);
        }

        private void LerpImageIn(ref float TValue)
        {
            TValue += Time.deltaTime / IMAGE_LERP_TIME;
            m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, TValue);
        }
        #endregion
    }
}
