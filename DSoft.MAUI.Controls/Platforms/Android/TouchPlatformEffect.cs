﻿using Android.Views;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using System.ComponentModel;
using Point = Microsoft.Maui.Graphics.Point;
using Rect = Microsoft.Maui.Graphics.Rect;

namespace DSoft.Maui.Controls.TouchTracking
{
	// All the code in this file is only included on Android.
	internal class TouchPlatformEffect : PlatformEffect
	{
		Android.Views.View view;
		Element _formsElement;
		TouchTracking.TouchEffect _pclTouchEffect;

		bool _capture;
		Func<double, double> _fromPixels;
		int[] twoIntArray = new int[2];

		static Dictionary<Android.Views.View, TouchPlatformEffect> viewDictionary = new Dictionary<Android.Views.View, TouchPlatformEffect>();
		static Dictionary<int, TouchPlatformEffect> idToEffectDictionary = new Dictionary<int, TouchPlatformEffect>();

		protected override void OnAttached()
		{
			// Get the Android View corresponding to the Element that the effect is attached to
			view = Control == null ? Container : Control;

			// Get access to the TouchEffect class in the PCL
			TouchTracking.TouchEffect touchEffect =
				(TouchTracking.TouchEffect)Element.Effects.
					FirstOrDefault(e => e is TouchTracking.TouchEffect);

			if (touchEffect != null && view != null)
			{
				viewDictionary.Add(view, this);

				_formsElement = Element;

				_pclTouchEffect = touchEffect;

				// Save fromPixels function
				_fromPixels = view.Context.FromPixels;

				// Set event handler on View
				view.Touch += OnTouch;
			}
		}

		protected override void OnDetached()
		{
			try
			{
				if (viewDictionary.ContainsKey(view))
				{
					viewDictionary.Remove(view);
					view.Touch -= OnTouch;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		void OnTouch(object sender, Android.Views.View.TouchEventArgs args)
		{
			// Two object common to all the events
			Android.Views.View senderView = sender as Android.Views.View;
			MotionEvent motionEvent = args.Event;

			// Get the pointer index
			int pointerIndex = motionEvent.ActionIndex;

			// Get the id that identifies a finger over the course of its progress
			int id = motionEvent.GetPointerId(pointerIndex);


			senderView.GetLocationOnScreen(twoIntArray);
			Point screenPointerCoords = new Point((int)(twoIntArray[0] + motionEvent.GetX(pointerIndex)),(int)
				(twoIntArray[1] + motionEvent.GetY(pointerIndex)));


			// Use ActionMasked here rather than Action to reduce the number of possibilities
			switch (args.Event.ActionMasked)
			{
				case MotionEventActions.Down:
				case MotionEventActions.PointerDown:
					FireEvent(this, id, TouchActionType.Pressed, screenPointerCoords, true);

					idToEffectDictionary.Add(id, this);

					_capture = _pclTouchEffect.Capture;
					break;

				case MotionEventActions.Move:
					// Multiple Move events are bundled, so handle them in a loop
					for (pointerIndex = 0; pointerIndex < motionEvent.PointerCount; pointerIndex++)
					{
						id = motionEvent.GetPointerId(pointerIndex);

						if (_capture)
						{
							senderView.GetLocationOnScreen(twoIntArray);

							screenPointerCoords = new Point(twoIntArray[0] + motionEvent.GetX(pointerIndex),
								twoIntArray[1] + motionEvent.GetY(pointerIndex));

							FireEvent(this, id, TouchActionType.Moved, screenPointerCoords, true);
						}
						else
						{
							CheckForBoundaryHop(id, screenPointerCoords);

							if (idToEffectDictionary[id] != null)
							{
								FireEvent(idToEffectDictionary[id], id, TouchActionType.Moved, screenPointerCoords, true);
							}
						}
					}
					break;

				case MotionEventActions.Up:
				case MotionEventActions.Pointer1Up:
					if (_capture)
					{
						FireEvent(this, id, TouchActionType.Released, screenPointerCoords, false);
					}
					else
					{
						CheckForBoundaryHop(id, screenPointerCoords);

						if (idToEffectDictionary[id] != null)
						{
							FireEvent(idToEffectDictionary[id], id, TouchActionType.Released, screenPointerCoords, false);
						}
					}
					idToEffectDictionary.Remove(id);
					break;

				case MotionEventActions.Cancel:
					if (_capture)
					{
						FireEvent(this, id, TouchActionType.Cancelled, screenPointerCoords, false);
					}
					else
					{
						if (idToEffectDictionary[id] != null)
						{
							FireEvent(idToEffectDictionary[id], id, TouchActionType.Cancelled, screenPointerCoords, false);
						}
					}
					idToEffectDictionary.Remove(id);
					break;
			}
		}

		void CheckForBoundaryHop(int id, Point pointerLocation)
		{
			TouchPlatformEffect touchEffectHit = null;

			foreach (Android.Views.View view in viewDictionary.Keys)
			{
				// Get the view rectangle
				try
				{
					view.GetLocationOnScreen(twoIntArray);
				}
				catch // System.ObjectDisposedException: Cannot access a disposed object.
				{
					continue;
				}
				Rect viewRect = new Rect(twoIntArray[0], twoIntArray[1], view.Width, view.Height);

				if (viewRect.Contains(pointerLocation))
				{
					touchEffectHit = viewDictionary[view];
				}
			}

			if (touchEffectHit != idToEffectDictionary[id])
			{
				if (idToEffectDictionary[id] != null)
				{
					FireEvent(idToEffectDictionary[id], id, TouchActionType.Exited, pointerLocation, true);
				}
				if (touchEffectHit != null)
				{
					FireEvent(touchEffectHit, id, TouchActionType.Entered, pointerLocation, true);
				}
				idToEffectDictionary[id] = touchEffectHit;
			}
		}

		void FireEvent(TouchPlatformEffect touchEffect, int id, TouchActionType actionType, Point pointerLocation, bool isInContact)
		{
			// Get the method to call for firing events
			Action<Element, TouchActionEventArgs> onTouchAction = touchEffect._pclTouchEffect.OnTouchAction;

			// Get the location of the pointer within the view
			touchEffect.view.GetLocationOnScreen(twoIntArray);
			double x = pointerLocation.X - twoIntArray[0];
			double y = pointerLocation.Y - twoIntArray[1];
			Point point = new Point(_fromPixels(x), _fromPixels(y));

			// Call the method
			onTouchAction(touchEffect._formsElement,
				new TouchActionEventArgs(id, actionType, point, isInContact));
		}
	}
}