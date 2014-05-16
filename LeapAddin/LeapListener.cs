using System;

using Gtk;
using GLib;
using Leap;

using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Debugger;

namespace LeapAddin
{
	public class LeapListener : Listener
	{
		Controller controller;
		
		public LeapListener ()
		{
			controller = new Controller ();
			controller.AddListener (this);
		}
		
		public override void OnConnect (Controller arg0)
		{
			base.OnConnect (arg0);
			controller.EnableGesture (Gesture.GestureType.TYPECIRCLE);
			controller.EnableGesture (Gesture.GestureType.TYPESWIPE);
		}
		
		public override void OnFrame (Controller arg0)
		{
			base.OnFrame (arg0);
			var gestures = controller.Frame ().Gestures ();
			for (int i = 0; i < gestures.Count; i++) {
				var gesture = gestures[i];
				switch (gesture.Type) {
				case Gesture.GestureType.TYPECIRCLE:
					var circleGesture = new CircleGesture (gesture);
					if (circleGesture.State == Gesture.GestureState.STATESTOP && circleGesture.Progress > 1)
						IdeApp.CommandService.DispatchCommand (ProjectCommands.BuildSolution);
					break;
				case Gesture.GestureType.TYPESWIPE:
					var swipeGesture = new SwipeGesture (gesture);
					if (swipeGesture.State == Gesture.GestureState.STATESTOP && Math.Abs (swipeGesture.Direction.x) > 0.5f) {
						var wasLeft = swipeGesture.Direction.x < 0;
						if (wasLeft)
							IdeApp.CommandService.DispatchCommand (ProjectCommands.Stop);
						else
							IdeApp.CommandService.DispatchCommand (DebugCommands.Debug);
					}
					break;
				}
			}
		}
	}
}

