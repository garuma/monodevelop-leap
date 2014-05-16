using System;
using MonoDevelop.Components.Commands;

namespace LeapAddin
{
	public class LeapStarter : CommandHandler
	{
		LeapListener listener;
	
		protected override void Run ()
		{
			if (listener == null)
				listener = new LeapListener ();
		}
	}
}

