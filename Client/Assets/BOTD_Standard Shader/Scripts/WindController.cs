using UnityEngine;

namespace InfinityPBR.BOTD
{
    public static class BOTDShaderID
    {
        public static readonly int m_windConfiguration;
        public static readonly int m_windNoiseTexture;
        public static readonly string m_windNoiseTextureName = "Wind Noise Mask.png";

        static BOTDShaderID()
        {
            //X = Speed Y = Scale Z = Tiling W = Power
            m_windConfiguration = Shader.PropertyToID("WindConfiguration");
            m_windNoiseTexture = Shader.PropertyToID("WindNoise");
        }
    }

    [ExecuteAlways]
    public class WindController : MonoBehaviour
    {
        #region Variables

        public WindZone m_windZone;

        public float WindSpeed
        {
            get { return m_windSpeed; }
            set
            {
                if (m_windSpeed != value)
                {
                    m_windSpeed = value;
                    UpdateWindValues();
                }
            }
        }

        [SerializeField] private float m_windSpeed = 0.05f;

        public float WindScale
        {
            get { return m_windScale; }
            set
            {
                if (m_windScale != value)
                {
                    m_windScale = value;
                    UpdateWindValues();
                }
            }
        }

        [SerializeField] private float m_windScale = 6f;

        public float WindTiling
        {
            get { return m_windTiling; }
            set
            {
                if (m_windTiling != value)
                {
                    m_windTiling = value;
                    UpdateWindValues();
                }
            }
        }

        [SerializeField] private float m_windTiling = 0.5f;

        public float WindPower
        {
            get { return m_windPower; }
            set
            {
                if (m_windPower != value)
                {
                    m_windPower = value;
                    UpdateWindValues();
                }
            }
        }

        [SerializeField] private float m_windPower = 0.5f;

        public Texture2D WindNoiseTexture
        {
            get { return m_windNoiseTexture; }
            set
            {
                if (m_windNoiseTexture != value)
                {
                    m_windNoiseTexture = value;
                    UpdateWindValues();
                }
            }
        }

        [SerializeField] private Texture2D m_windNoiseTexture;

        [SerializeField] private Vector4 m_windConfiguration;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            UpdateWindValues();
        }
        private void Update()
        {
            if (m_windZone == null)
            {
                m_windZone = FindObjectOfType<WindZone>();
                return;
            }

            WindSpeed = m_windZone.windMain;
            WindScale = m_windZone.windPulseMagnitude;
            WindTiling = m_windZone.windPulseFrequency;
            WindPower = m_windZone.windTurbulence;
        }

        #endregion

        #region Functions

        private void UpdateWindValues()
        {
            m_windConfiguration.x = WindSpeed;
            m_windConfiguration.y = WindScale;
            m_windConfiguration.z = WindTiling;
            m_windConfiguration.w = WindPower;

            UpdateWindNoiseTexture();
            UpdateWindSettings();
        }
        private void UpdateWindSettings()
        {
            Shader.SetGlobalVector(BOTDShaderID.m_windConfiguration, m_windConfiguration);
        }
        private void UpdateWindNoiseTexture()
        {
            Shader.SetGlobalTexture(BOTDShaderID.m_windNoiseTexture, WindNoiseTexture);
        }
        public void RevertToDefaults()
        {
            if (m_windZone != null)
            {
                m_windZone.windMain = 0.5f;
                m_windZone.windTurbulence = 0.2f;
                m_windZone.windPulseMagnitude = 0.3f;
                m_windZone.windPulseFrequency = 0.2f;
            }
        }

        #endregion
    }
}