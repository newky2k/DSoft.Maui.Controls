using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.Maui.Controls
{
	public class SimpleDonutGaugeView : SimpleRadialGaugeView
	{
		public SimpleDonutGaugeView() : base()
		{
			StartAngle = 0;
			EndAngle = 360;
			Rotation = 0;
		}
	}
}
