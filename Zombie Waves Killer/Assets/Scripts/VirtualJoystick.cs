using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {
	private Image backgroundImage;
	private Image joystickImage;
	private Vector3 inputVector;
    // flag for run code once in update function
    private bool isPlayerDead = true;
	public static bool isJoystickPressed = false;

	private void Start(){
		backgroundImage = GetComponent<Image> ();
		joystickImage = transform.GetChild (0).GetComponent<Image> ();
        isPlayerDead = true;
	}

    private void Update()
    {
        if (ZombieController.IsPlayerDead && isPlayerDead) {
            inputVector = Vector3.zero;
            joystickImage.rectTransform.anchoredPosition = Vector3.zero;

            isJoystickPressed = false;
            isPlayerDead = false;
        }
    }

    public virtual void OnDrag (PointerEventData eventData)
	{
		Vector2 pos;
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImage.rectTransform, eventData.position, eventData.pressEventCamera, out pos)){
			pos.x = (pos.x / backgroundImage.rectTransform.sizeDelta.x);
			pos.y = (pos.y / backgroundImage.rectTransform.sizeDelta.y);

			//inputVector = new Vector3 (pos.x*2, 0, pos.y*2);
			inputVector = new Vector3((pos.x - 0.5f) * 2.0f, 0, (pos.y - 0.5f) * 2.0f);
			inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

			joystickImage.rectTransform.anchoredPosition = new Vector3 (inputVector.x * (backgroundImage.rectTransform.sizeDelta.x / 3), inputVector.z * (backgroundImage.rectTransform.sizeDelta.y / 3));

			isJoystickPressed = true;
		}
	}

	public virtual void OnPointerUp (PointerEventData eventData)
	{
		inputVector = Vector3.zero;
		joystickImage.rectTransform.anchoredPosition = Vector3.zero;

		isJoystickPressed = false;
	}

	public virtual void OnPointerDown (PointerEventData eventData)
	{
		//AudioManager.instance.PlaySound ("Player running", transform.position);
		OnDrag (eventData);
		isJoystickPressed = true;
	}

	public float Horizontal(){
		if (inputVector.x != 0)
			return inputVector.x;
		else
			return Input.GetAxis ("Horizontal");
	}

	public float Vertical(){
		if (inputVector.z != 0)
			return inputVector.z;
		else
			return Input.GetAxis ("Vertical");
	}
}
