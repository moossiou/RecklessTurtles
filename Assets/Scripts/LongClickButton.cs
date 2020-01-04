using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClickButton : MonoBehaviour , IPointerDownHandler ,IPointerUpHandler {

    private bool pointerDown;
    private float pointerDownTimer;
    public float requireHoldTime;
    public UnityEvent onLongClick;

    [SerializeField]
    private Image fillImage;
    

    public void OnPointerDown (PointerEventData eventdata)
    {   
        pointerDown = true;     
    }

    public void OnPointerUp (PointerEventData eventdata)
    {
        Reset();
    }

	void Update () {

        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer > requireHoldTime)
            {
                if (onLongClick != null)
                {
                    onLongClick.Invoke();
                    Reset();
                }
            }
            
            fillImage.fillAmount = 1 - pointerDownTimer / requireHoldTime;
        }else
        {          
            pointerDownTimer -= Time.deltaTime;
            fillImage.fillAmount = 1 - pointerDownTimer / requireHoldTime;
        }

	}

    private void Reset()
    {
        pointerDown = false;
        pointerDownTimer -= Time.deltaTime;
           fillImage.fillAmount = 1 - pointerDownTimer / requireHoldTime;
        fillImage.fillAmount = pointerDownTimer / requireHoldTime;    
    }

   
}
 