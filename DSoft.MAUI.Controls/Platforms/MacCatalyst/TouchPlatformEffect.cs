﻿using Microsoft.Maui.Controls.Platform;
using System.ComponentModel;
using UIKit;

namespace DSoft.Maui.Controls.TouchTracking
{
	// All the code in this file is only included on Mac Catalyst.
	internal class TouchPlatformEffect : PlatformEffect
	{
		UIView view;
		TouchRecognizer touchRecognizer;

		protected override void OnAttached()
		{
			// Get the iOS UIView corresponding to the Element that the effect is attached to
			view = Control ?? Container;

			// Get access to the TouchEffect class in the PCL
			TouchTracking.TouchEffect effect = (TouchTracking.TouchEffect)Element.Effects.FirstOrDefault(e => e is TouchTracking.TouchEffect);

			if (effect != null && view != null)
			{
				// Create a TouchRecognizer for this UIView
				touchRecognizer = new TouchRecognizer(Element, view, effect);
				view.AddGestureRecognizer(touchRecognizer);
			}
		}

		protected override void OnDetached()
		{
			if (touchRecognizer != null)
			{
				// Clean up the TouchRecognizer object
				touchRecognizer.Detach();

				// Remove the TouchRecognizer from the UIView
				view.RemoveGestureRecognizer(touchRecognizer);
			}
		}
	}
}