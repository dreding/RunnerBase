using System;
using UnityEngine;

//Author : https://gist.github.com/alialacan/1eddcd107f4a48a46dea17695ca151f2
namespace CustomInput
{
	public class SwipeDetector
	{
		private Vector2 fingerDownPos;
		private Vector2 fingerUpPos;

		public bool detectSwipeAfterRelease = false;

		public float SWIPE_THRESHOLD = 20f;

		public Action OnSwipeLeft;
		public Action OnSwipeRight;

		// Update is called once per frame
		public void OnUpdate()
		{
			foreach (Touch touch in UnityEngine.Input.touches)
			{
				if (touch.phase == TouchPhase.Began)
				{
					fingerUpPos = touch.position;
					fingerDownPos = touch.position;
				}

				//Detects Swipe while finger is still moving on screen
				if (touch.phase == TouchPhase.Moved)
				{
					if (!detectSwipeAfterRelease)
					{
						fingerDownPos = touch.position;
						//DetectSwipe();
					}
				}

				//Detects swipe after finger is released from screen
				if (touch.phase == TouchPhase.Ended)
				{
					fingerDownPos = touch.position;
					DetectSwipe();
				}
			}
		}

		void DetectSwipe()
		{
			if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue())
			{
				fingerUpPos = fingerDownPos;
			}
			else if (HorizontalMoveValue() > SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue())
			{
				Debug.Log("Horizontal Swipe Detected!");
				if (fingerDownPos.x - fingerUpPos.x > 0)
				{
					OnSwipeRight();
				}
				else if (fingerDownPos.x - fingerUpPos.x < 0)
				{
					OnSwipeLeft();
				}

				fingerUpPos = fingerDownPos;

			}
			else
			{
				Debug.Log("No Swipe Detected!");
			}
		}

		float VerticalMoveValue ()
		{
			return Mathf.Abs (fingerDownPos.y - fingerUpPos.y);
		}

		float HorizontalMoveValue ()
		{
			return Mathf.Abs (fingerDownPos.x - fingerUpPos.x);
		}
	}
}