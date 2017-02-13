using System;
using System.Timers;
using System.Collections.Generic;

public class Rhythm_System
{
	private static System.Timers.Timer aTimer;
	static int beat = 0;
	static Boolean accepted = true;

	public static void Main()
	{
		SetTimer();

		Console.ReadLine();
		aTimer.Stop();
		aTimer.Dispose();

		Console.WriteLine("Terminating the application...");
	}

	private static void SetTimer()
	{
		//divides 120 beats into eighths to have the buffer
		aTimer = new System.Timers.Timer(125/2);
		// Hook up the Elapsed event for the timer. 
		aTimer.Elapsed += OnTimedEvent;
		aTimer.AutoReset = true;
		aTimer.Enabled = true;
	}

	//where all the actual action happens
	private static void OnTimedEvent(Object source, ElapsedEventArgs e)
	{
		if (beat == 7) {
			accepted = true;
		}
		if (beat == 8) {
			beat = 0;
		}
		if (beat == 2) {
			accepted = false;
		}

		//just as a visual for how the time works
		if (accepted) {
			Console.WriteLine ("double yay");
		} else {
			Console.WriteLine ("no yay");
		}

		beat++;
	}
}