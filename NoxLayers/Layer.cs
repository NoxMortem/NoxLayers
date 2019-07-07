namespace Infrastructure.NoxLayers
{
	/// <summary>
	/// Layer provides a enum wrapper for the 32 Unity Layers.
	/// 
	/// It is necessary to refactor the name of the enum variable when the name of
	/// the layers in Unity is changed to avoid confusion, but this is still better than
	/// working with unnamed variables. The enums allows the auto generation of switch labels
	/// it provieds additional information about which layers is requested and what is likely expected
	/// 
	/// A Layer is a value from 0-31 therefore you can not rely on bitwise operations of Layers e.g.
	/// Layer | Layer or Layer & Layer. However, you can use the Mask class to use bitwise operations
	/// new Mask(Layer) | new Mask(Layer) (which is equivalent to new Mask(Layer,Layer)) or
	/// new Mask(Layer) & new Mask(Layer).
	/// 
	/// A Layer uses the values 0-31 such that you can easily cast unity Layers to layers like:
	/// (Layer) gameObject.layer and also assign them via a simple cast gameObject.layer = (int) Layer.Default; 
	/// </summary>
	/// <seealso cref="Mask"/>
	public enum Layer
	{
		/// <summary>
		/// UnityEngine Layer 0
		/// </summary>
		Default = 0,

		/// <summary>
		/// UnityEngine Layer 1
		/// </summary>
		TransparentFX = 1,

		/// <summary>
		/// UnityEngine Layer 2
		/// </summary>
		IgnoreRaycast = 2,

		/// <summary>
		/// UnityEngine Layer 3
		/// </summary>
		L3 = 3,

		/// <summary>
		/// UnityEngine Layer 4
		/// </summary>
		Water = 4,

		/// <summary>
		/// UnityEngine Layer 5
		/// </summary>
		UI = 5,

		/// <summary>
		/// UnityEngine Layer 6
		/// </summary>
		L6 = 6,

		/// <summary>
		/// UnityEngine Layer 7
		/// </summary>
		L7 = 7,

		/// <summary>
		/// UnityEngine Layer 8
		/// </summary>
		Clickables = 8,

		/// <summary>
		/// UnityEngine Layer 9
		/// </summary>
		L9 = 9,

		/// <summary>
		/// UnityEngine Layer 10
		/// </summary>
		L10 = 10,

		/// <summary>
		/// UnityEngine Layer 11
		/// </summary>
		L11 = 11,

		/// <summary>
		/// UnityEngine Layer 12
		/// </summary>
		L12 = 12,

		/// <summary>
		/// UnityEngine Layer 13
		/// </summary>
		L13 = 13,

		/// <summary>
		/// UnityEngine Layer 14
		/// </summary>
		L14 = 14,

		/// <summary>
		/// UnityEngine Layer 15
		/// </summary>
		L15 = 15,

		/// <summary>
		/// UnityEngine Layer 16
		/// </summary>
		L16 = 16,

		/// <summary>
		/// UnityEngine Layer 17
		/// </summary>
		L17 = 17,

		/// <summary>
		/// UnityEngine Layer 18
		/// </summary>
		L18 = 18,

		/// <summary>
		/// UnityEngine Layer 19
		/// </summary>
		L19 = 19,

		/// <summary>
		/// UnityEngine Layer 20
		/// </summary>
		L20 = 20,

		/// <summary>
		/// UnityEngine Layer 21
		/// </summary>
		L21 = 21,

		/// <summary>
		/// UnityEngine Layer 22
		/// </summary>
		L22 = 22,

		/// <summary>
		/// UnityEngine Layer 23
		/// </summary>
		L23 = 23,

		/// <summary>
		/// UnityEngine Layer 24
		/// </summary>
		L24 = 24,

		/// <summary>
		/// UnityEngine Layer 25
		/// </summary>
		L25 = 25,

		/// <summary>
		/// UnityEngine Layer 26
		/// </summary>
		L26 = 26,

		/// <summary>
		/// UnityEngine Layer 27
		/// </summary>
		L27 = 27,

		/// <summary>
		/// UnityEngine Layer 28
		/// </summary>
		L28 = 28,

		/// <summary>
		/// UnityEngine Layer 29
		/// </summary>
		L29 = 29,

		/// <summary>
		/// UnityEngine Layer 30
		/// </summary>
		L30 = 30,

		/// <summary>
		/// UnityEngine Layer 31
		/// </summary>
		L31 = 31
	}
}