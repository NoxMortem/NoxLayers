using System;
using Infrastructure.NoxLayers;

// ReSharper disable CheckNamespace
namespace Layers
{
	public static class Enums
	{
		/// <summary>
		/// Returns the string representation of the enum variable. Useful for debugging purposes.
		/// </summary>
		/// <param name="layer"></param>
		/// <returns>The name of the enums variable</returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static string ToString(this Layer layer)
		{
			switch (layer)
			{
				case Layer.Default: return "Layer.Default";
				case Layer.TransparentFX: return "Layer.TransparentFX";
				case Layer.IgnoreRaycast: return "Layer.IgnoreRaycast";
				case Layer.L3: return "Layer.L3";
				case Layer.Water: return "Layer.Water";
				case Layer.UI: return "Layer.UI";
				case Layer.L6: return "Layer.L6";
				case Layer.L7: return "Layer.L7";
				case Layer.Clickables: return "Layer.IgnoreTopDown";
				case Layer.L9: return "Layer.Selectable";
				case Layer.L10: return "Layer.IgnoreVR";
				case Layer.L11: return "Layer.Floor";
				case Layer.L12: return "Layer.Player";
				case Layer.L13: return "Layer.Wall";
				case Layer.L14: return "Layer.EscapeRoute";
				case Layer.L15: return "Layer.TestOBJ";
				case Layer.L16: return "Layer.IgnorePhysics";
				case Layer.L17: return "Layer.VROnly";
				case Layer.L18: return "Layer.MiniModelPreview";
				case Layer.L19: return "Layer.SelectableIcon";
				case Layer.L20: return "Layer.EndlessPlane";
				case Layer.L21: return "Layer.ExteriorObjectBlocking";
				case Layer.L22: return "Layer.L22";
				case Layer.L23: return "Layer.L23";
				case Layer.L24: return "Layer.L24";
				case Layer.L25: return "Layer.L25";
				case Layer.L26: return "Layer.L26";
				case Layer.L27: return "Layer.L27";
				case Layer.L28: return "Layer.L28";
				case Layer.L29: return "Layer.L29";
				case Layer.L30: return "Layer.L30";
				case Layer.L31: return "Layer.L31";
				default:
					throw new ArgumentOutOfRangeException(nameof(layer), layer, "Please provide a string version of Util.Layer");
			}
		}
	}
}
// ReSharper restore CheckNamespace