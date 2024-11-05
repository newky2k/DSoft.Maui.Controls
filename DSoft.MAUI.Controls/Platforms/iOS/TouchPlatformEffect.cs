using Microsoft.Maui.Controls.Platform;
using UIKit;

namespace DSoft.Maui.Controls.TouchTracking
{
	internal class TouchPlatformEffect : PlatformEffect
	{
		private UIView _view;
		private TouchRecognizer _touchRecognizer;

		protected override void OnAttached()
		{
			// Get the iOS UIView corresponding to the Element that the effect is attached to
			_view = Control as UIView ?? Container as UIView;

			// Get access to the TouchEffect class in the PCL
			var effect = (TouchTracking.TouchEffect)Element.Effects.FirstOrDefault(e => e is TouchEffect);

			if (effect != null && _view != null)
			{
				// Create a TouchRecognizer for this UIView
				_touchRecognizer = new TouchRecognizer(Element, _view, effect);
				_view.AddGestureRecognizer(_touchRecognizer);
			}
		}

		protected override void OnDetached()
		{
			if (_touchRecognizer != null)
			{
				// Clean up the TouchRecognizer object
				_touchRecognizer.Detach();

				// Remove the TouchRecognizer from the UIView
				_view.RemoveGestureRecognizer(_touchRecognizer);
			}
		}
	}
}
