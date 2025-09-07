using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


class Sine : Ease {

    public static double EaseIn(double t, double b, double c, double d)
    {
		return -c * Math.Cos(t/d * HALF_PI) + c + b;
	}
    public static double EaseOut(double t, double b, double c, double d)
    {
		return c * Math.Sin(t/d * HALF_PI) + b;
	}
    public static float EaseInOut(float t, float b, float c, float d)
    {
		return -c/2 * (float)(Math.Cos(Math.PI*t/d) - 1) + b;
	}
}
