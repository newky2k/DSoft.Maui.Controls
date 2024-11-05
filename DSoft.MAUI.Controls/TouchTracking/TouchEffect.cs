using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Maui.Controls.TouchTracking
{
	public class TouchEffect : RoutingEffect
	{
		public event TouchActionEventHandler TouchAction;

		public TouchEffect() : base() { }

		public bool Capture { set; get; }

		public void OnTouchAction(Element element, TouchActionEventArgs args)
		{
			TouchAction?.Invoke(element, args);
		}
	}
}
