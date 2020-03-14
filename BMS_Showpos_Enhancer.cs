using LiveSplit.ComponentUtil;
using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components {
	class BMS_Showpos_Enhancer: IComponent {
		public string ComponentName {
			get { return "Black Mesa Showpos Enhancer"; }
		}

		public IDictionary<string, Action> ContextMenuControls { get; protected set; }
		private LiveSplitState _state;

		private bool isHooked = false;
		private Process Game;
		IntPtr velocityTextPTR;
		DeepPointer xVelocityDP = new DeepPointer("server.dll", 0x890BFC);
		DeepPointer yVelocityDP = new DeepPointer("server.dll", 0x890C00);
		DeepPointer zVelocityDP = new DeepPointer("server.dll", 0x890C04);
		DeepPointer velocityTextDP = new DeepPointer("client.dll", 0x4432CC);

		public BMS_Showpos_Enhancer(LiveSplitState state) {
			_state = state;
			_state.OnReset += state_OnReset;
		}

		public void Dispose() {
			byte[] velocityArray = Encoding.ASCII.GetBytes("vel:  %.2f");
			byte[] outputArray = new byte[32];
			for (int i = 0; i < velocityArray.Length; i++) {
				outputArray[i] = velocityArray[i];
			}

			Game.WriteBytes(velocityTextPTR, outputArray);
			_state.OnReset -= state_OnReset;
		}

		public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) {

			if (Game == null) {
				isHooked = false;
			} else if (Game.HasExited) {
				Game = null;
				isHooked = false;
			}else{
				isHooked = xVelocityDP.Deref<float>(Game, out var ignore);
			}

			if (!isHooked) {
				List<Process> GameProcesses = Process.GetProcesses().ToList().FindAll(x => x.ProcessName.StartsWith("bms"));
				if (GameProcesses.Count > 0) {
					Game = GameProcesses.First();
					velocityTextDP.DerefOffsets(Game, out velocityTextPTR);
					Game.VirtualProtect(velocityTextPTR, 32, MemPageProtect.PAGE_EXECUTE_READWRITE);
					isHooked = xVelocityDP.Deref<float>(Game, out var ignore);
				}
			} else {
				float xVel = xVelocityDP.Deref<float>(Game);
				float yVel = yVelocityDP.Deref<float>(Game);
				float zVel = zVelocityDP.Deref<float>(Game);
				double horizontalVelocity = Math.Sqrt(Math.Pow(xVel, 2) + Math.Pow(yVel, 2));

				byte[] velocityArray = Encoding.ASCII.GetBytes("hvel: " + horizontalVelocity.ToString("0.00")+ "  zvel:  "+zVel.ToString("0.00"));
				byte[] outputArray = new byte[32];
				for (int i = 0; i < velocityArray.Length; i++) {
					outputArray[i] = velocityArray[i];
				}

				Game.WriteBytes(velocityTextPTR, outputArray);
			}
				
		}

		public void DrawVertical(Graphics g, LiveSplitState state, float width, Region region) {
			this.PrepareDraw(state);
		}

		public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region region) {
			this.PrepareDraw(state);
		}

		void PrepareDraw(LiveSplitState state) {
		}

		void state_OnReset(object sender, TimerPhase t) {
		}

		public XmlNode GetSettings(XmlDocument document) { return document.CreateElement("Settings"); }
		public Control GetSettingsControl(LayoutMode mode) { return null; }
		public void SetSettings(XmlNode settings) { }
		public void RenameComparison(string oldName, string newName) { }
		public float MinimumWidth { get { return 0; } }
		public float MinimumHeight { get { return 0; } }
		public float VerticalHeight { get { return 0; } }
		public float HorizontalWidth { get { return 0; } }
		public float PaddingLeft { get { return 0; } }
		public float PaddingRight { get { return 0; } }
		public float PaddingTop { get { return 0; } }
		public float PaddingBottom { get { return 0; } }


	}

}

