using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{

    private float m_fillAmount;

    [SerializeField]
    private float m_lerpSpeed;

    [SerializeField]
    private Image m_content;

    [SerializeField]
    private Color m_fullColor;

    [SerializeField]
    private Color m_lowColor;

    [SerializeField]
    private bool m_lerpColors;

    [SerializeField]
    private float m_maxValue;

    public float MaxValue
    {
        get { return m_maxValue; }
        set { m_maxValue = value; }
    }

    public float Value
    {
        set
        {
            m_fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        if (m_lerpColors)
        {
            m_content.color = m_fullColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleBarUpdate();
    }

    /// <summary>
    /// 设置人物的血条显示
    /// </summary>
    private void HandleBarUpdate()
    {
        if (m_fillAmount != m_content.fillAmount)
        {
            m_content.fillAmount = Mathf.Lerp(m_content.fillAmount, m_fillAmount, Time.deltaTime * m_lerpSpeed);
            //m_healthTxt.text = string.Format("{0:N0}", ().ToString()) + "%";
        }
        if (m_lerpColors)
        {
            m_content.color = Color.Lerp(m_lowColor, m_fullColor, m_fillAmount);
        }
    }
    public void Reset()
    {
        Value = MaxValue;
        m_content.fillAmount = 1;
    }

    /// <summary>
    /// 设置怪物当前血条显示
    /// </summary>
    /// <param name="value">当前血量</param>
    /// <param name="inMin">最小血量</param>
    /// <param name="inMax">最大血量</param>
    /// <param name="outMin">最小百分比</param>
    /// <param name="outMax">最大百分比</param>
    /// <returns></returns>
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        // (当前生命值[具体数值]) * (最大生命值[百分比]) / （ 最大生命值[具体数值] ）
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //(78 - 0) *(1-0)/(230-0)+0
        //  78 * 1 / 100 = 0.8
    }
}
