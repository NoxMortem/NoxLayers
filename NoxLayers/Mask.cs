using System.Collections;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedMember.Global, This is a utility class and it is expected that some methods are unused

namespace Infrastructure.NoxLayers
{
	/// <summary>
	/// The Mask is an Immutable to ease the use of the Unity Layer and LayerMask.
	/// It overrides Equals, ==,|,%,+,- and provides IEnumerable&lt;Layer&gt;
	///
	/// IMPORTANT! A mask treats all provided int values as a Layer (0-31). This solves the ambiguity between layers (int)
	/// and bitmasks. It internally uses a bitmask.
	/// all operations as a Mask created with that int. This means if you provide a bitmask for bitwise operations
	/// like Mask & (int|int) it will treat it as Mask & Mask((Layer) (int|int)) is unlikely what you want.
	///
	/// This is required to allow the syntax Mask operator GameObject.Layer and GameObject.Layer operator Mask, eg.
	/// gameObject.Layer & new Mask(Layer,Layer).
	///
	/// It also removes the ambiguity between UnityEngine.Layer (int) and UnityEngine.LayerMask (int) as our Mask
	/// completly replaces the latter and all ints are treated as Layer
	///
	/// You can use Mask.FromBits(int|int&amp;int) or Mask.FromLayer(gameObject.layer) to clarify the used
	/// contract.
	/// </summary>
	/// <seealso cref="Layer"/>
	/// <seealso cref="FromBits"/>
	/// <seealso cref="FromLayer"/>
	public class Mask : IEnumerable<Layer>
	{
		/// <summary>
		/// The Bits of this LayerMask
		/// </summary>
		private readonly int bits;

		/// <summary>
		/// Holds all Layers. This is a cache to avoid recalculation of all Layers if the layers
		/// of a mask are iterated through multiple times. A Mask is immutable and we want to test
		/// which layers it consists of only once.
		/// Layers has to be a Set, this means, that it contains each layer it is made of exactly once, iff
		/// the bit at (int) Layer is set.
		/// </summary>
		private readonly HashSet<Layer> layers = new HashSet<Layer>();

		/// <summary>
		/// Returns the amount of layers this mask contains
		/// </summary>
		public int LayerCount => layers.Count;

		private static readonly int[] AllLayersInts = {
			                                              0,
			                                              1,
			                                              2,
			                                              3,
			                                              4,
			                                              5,
			                                              6,
			                                              7,
			                                              8,
			                                              9,
			                                              10,
			                                              11,
			                                              12,
			                                              13,
			                                              14,
			                                              15,
			                                              16,
			                                              17,
			                                              18,
			                                              19,
			                                              20,
			                                              21,
			                                              22,
			                                              23,
			                                              24,
			                                              25,
			                                              26,
			                                              27,
			                                              28,
			                                              29,
			                                              30,
			                                              31
		                                              };

		/// <summary>
		/// AllLayers is a static readonly containing the often required bitmask for all Layers
		/// </summary>
		public static readonly Mask AllLayers = new Mask(AllLayersInts);

		/// <summary>
		/// Generate a new Mask from the given BitMask, where each bit (0-31) represents one Layer
		/// </summary>
		/// <param name="bits">BitMask for this LayerMask</param>
		private Mask(int bits)
		{
			this.bits = bits;

			for (var i = 0; i <= 31; i++)
			{
				if ((bits & (1 << i)) != 0)
					layers.Add((Layer) i);
			}
		}

		/// <summary>
		/// The static FromBits factory method allows to distinguish between the ambigous
		/// bitmask (int) and layer (int). UnityEngine does not provide a clear differentiation.
		/// </summary>
		/// <param name="bitMask">Each bit set in the bitmask represents one layer (0-31)</param>
		/// <returns>A new Mask containing each layer where the corresponding bit is set (0-31)</returns>
		/// <seealso cref="FromLayer"/>
		public static Mask FromBits(int bitMask) => new Mask(bitMask);

		/// <summary>
		/// The static FromLayer factory method allows to distinguish between the ambigous
		/// bitmask (int) and a layer (int). UnityEngine does not provide a clear differentiation, but we do.
		/// </summary>
		/// <param name="layer">The layer bit (0-31) to set in the new Mask.</param>
		/// <returns>A new Mask containing exactly a single set bit - the bit of the layer (0-31)</returns>
		/// <seealso cref="FromBits"/>
		public static Mask FromLayer(int layer) => new Mask((Layer) layer);

		/// <summary>
		/// Generates a new Mask from a given Layer. This allows for new Mask((Layer) UnityEngine.Layer) syntax.
		/// </summary>
		/// <param name="layer">The UnityEngine.Layer (0-31) which represents the bit to set</param>
		public Mask(Layer layer) : this(1 << (int) layer) {}

		/// <summary>
		/// Generate a new Mask from a set of int. Each int (0-31) represents one unity layer
		/// This allows the syntax new Mask(gameObject1.layer, gameObject2.layer)
		/// It generates the BitMask from | over all layers.
		/// </summary>
		/// <param name="layers">The layers from the unity engine. Each int (0-31) represents a bit in a bitmask</param>
		public Mask(params int[] layers) : this(layers.Aggregate(0, (current, layer) => current | (1 << layer))) {}

		/// <summary>
		/// Generate a new Mask from a set of Layers.
		/// It generates the BitMask from | over all layers.
		/// </summary>
		/// <param name="layers">The layers to generate the BitMask from</param>
		public Mask(params Layer[] layers) : this(layers.Aggregate(0, (working, next) => working | (1 << (int) next))) {}

		/// <summary>
		/// Generates a new Mask from a set of Masks.
		/// It generates the BitMask from | over all masks.
		/// </summary>
		/// <param name="masks"></param>
		public Mask(params Mask[] masks) : this(masks.Aggregate(0, (working, next) => working | next.bits)) {}

		/// <summary>
		/// Returns a MaskContainsOperation which wraps this masks. This generates overhead, but allows a fluent syntax.
		/// e.g. Mask.Contains().Any(...),Mask.Contains().All(...),Mask.Contains().None(...) or Mask.Contains().Only(...).
		/// </summary>
		/// <returns>A MaskContainsOperation which is a wrapper for fluent syntax</returns>
		public MaskContainsOperation Contains() => new MaskContainsOperation(this);

		/// <summary>
		/// Returns a MaskIsOperation which wraps this masks. This generates overhead, but allows a fluent syntax.
		/// e.g. Mask.Is().Any(...),Mask.Is().Exactly(...)
		/// </summary>
		/// <returns></returns>
		public MaskIsOperation Is() => new MaskIsOperation(this);

		/// <summary>
		/// Tests if this mask is exactly the other Mask.
		/// </summary>
		/// <returns>true if this mask is exactly the other Mask</returns>
		/// <seealso cref="MaskIsOperation.Exactly(Mask[])"/>
		public bool Is(params Mask[] others) => new MaskIsOperation(this).Exactly(others);

		/// <summary>
		/// Tests if this mask is exactly the other Layer.
		/// </summary>
		/// <returns>true if this mask is exactly the other Layer</returns>
		/// <seealso cref="MaskIsOperation.Exactly(Layer[])"/>
		public bool Is(params Layer[] others) => new MaskIsOperation(this).Exactly(others);

		/// <summary>
		/// Tests if this mask is exactly the other int.
		/// </summary>
		/// <returns>true if this mask is exactly the other int</returns>
		/// <seealso cref="MaskIsOperation.Exactly(int[])"/>
		public bool Is(int[] others) => new MaskIsOperation(this).Exactly(others);

		/// <summary>
		/// A Mask contains another Mask if it contains all layers. Every Mask contains the empty Mask.
		/// </summary>
		/// <param name="other">The other mask to test</param>
		/// <returns>true if this Mask contains all layers from other Mask</returns>
		public bool Contains(Mask other) => other.LayerCount == 0 || !other.layers.Except(layers).Any();

		/// <summary>
		/// A Mask contains another Mask if it contains all layers. Every Mask contains the empty Mask, but
		/// ContainsAndNotEmpty also requires the other mask to contain at least one Layer.
		/// </summary>
		/// <param name="other">The other mask to test</param>
		/// <returns>true if this Mask contains all layers from other Mask and other contains at least 1 layer</returns>
		public bool ContainsAndNotEmpty(Mask other) => other.LayerCount > 0 && !other.layers.Except(layers).Any();

		/// <summary>
		/// A Mask contains a Layer if the bit (0-31) representing the layer is set
		/// </summary>
		/// <param name="layer"></param>
		/// <returns>true if this Mask contains the given layer</returns>
		public bool Contains(Layer layer) => (bits & (1 << (int) layer)) != 0;

		/// <summary>
		/// A Mask contains a unity layer if the bit it represents (0-31) is set.
		/// </summary>
		/// <param name="layer">The UnityEngine.Layer (0-31) which represents the bit which needs to be set</param>
		/// <returns>true if the bit at index layer (0-31) is set</returns>
		public bool Contains(int layer) => (bits & (1 << layer)) != 0;

		/// <summary>
		/// Provide IEnumerable&lt;Layer&gt; for Mask. This allows the usage of
		/// foreach(Layer l in Mask){...}
		/// </summary>
		/// <returns>An IEnumerable of all mask</returns>
		public IEnumerable<Layer> GetLayers() => layers;

		/// <summary>
		/// Bitwise & of both Bitmasks
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with both bitmasks & connected</returns>
		public static Mask operator &(Mask left, Mask right) => new Mask(left.bits & right.bits);

		/// <summary>
		/// Bitwise & of Mask & int
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with both bitmasks & connected</returns>
		public static Mask operator &(Mask left, int right) => left & new Mask((Layer) right);

		/// <summary>
		/// Bitwise & of int & Mask
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with both bitmasks & connected</returns>
		public static Mask operator &(int left, Mask right) => new Mask((Layer) left) & right;

		/// <summary>
		/// Bitwise & of Mask & Layer
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with both bitmasks & connected</returns>
		public static Mask operator &(Mask left, Layer right) => left & new Mask(right);

		/// <summary>
		/// Bitwise & of Layer & Mask
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with both bitmasks & connected</returns>
		public static Mask operator &(Layer left, Mask right) => new Mask(left) & right;

		/// <summary>
		/// Unsets all bits in the left Mask.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>The left mask minus all bits set in both masks</returns>
		public static Mask operator -(Mask left, Mask right) => new Mask(left.bits - (left.bits & right.bits));

		/// <summary>
		/// Unsets all bits in the left Mask.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>The left mask without the bits from the right mask</returns>
		public static Mask operator -(Mask left, int right) => left - new Mask((Layer) right);

		/// <summary>
		/// Unsets all bits in the left Mask.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>The left mask without the bits from the right mask</returns>
		public static Mask operator -(int left, Mask right) => new Mask((Layer) left) - right;

		/// <summary>
		/// Unsets the layer bit in the left Mask.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>The left mask without the bits from the right mask</returns>
		public static Mask operator -(Mask left, Layer right) => left - new Mask(right);

		/// <summary>
		/// Unsets the layer bit if it is set in the right mask
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>The left mask without the bits from the right mask</returns>
		public static Mask operator -(Layer left, Mask right) => new Mask(left) - right;

		/// <summary>
		/// operator+ behaves exactly like | for Masks.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with | of both bitmasks</returns>
		public static Mask operator +(Mask left, Mask right) => left | right;

		/// <summary>
		/// operator+ behaves exactly like | for Masks.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with | of both bitmasks</returns>
		public static Mask operator +(Mask left, int right) => left | (Layer) right;

		/// <summary>
		/// operator+ behaves exactly like | for Masks.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with | of both bitmasks</returns>
		public static Mask operator +(int left, Mask right) => new Mask((Layer) left) | right;

		/// <summary>
		/// operator+ behaves exactly like | for Masks.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with | of both bitmasks</returns>
		public static Mask operator +(Mask left, Layer right) => left | new Mask(right);

		/// <summary>
		/// operator+ behaves exactly like | for Masks.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with | of both bitmasks</returns>
		public static Mask operator +(Layer left, Mask right) => new Mask(left) | right;

		/// <summary>
		/// Returns a new mask where all bits set, which are set in one of the masks.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with | of both bitmasks</returns>
		public static Mask operator |(Mask left, Mask right) => new Mask(left.bits | right.bits);

		/// <summary>
		/// Returns a new mask where all bits set, which are set in one of the masks.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with | of both bitmasks</returns>
		public static Mask operator |(Mask left, int right) => left | new Mask((Layer) right);

		/// <summary>
		/// Returns a new mask where all bits set, which are set in one of the masks.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with | of both bitmasks</returns>
		public static Mask operator |(int left, Mask right) => new Mask((Layer) left) | right;

		/// <summary>
		/// Returns a new mask where all bits set, which are set in the mask or the layer
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with | of both bitmasks</returns>
		public static Mask operator |(Layer left, Mask right) => new Mask(left) | right;

		/// <summary>
		/// Returns a new mask where all bits set, which are set in the mask or the layer
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>A new Mask with | of both bitmasks</returns>
		public static Mask operator |(Mask left, Layer right) => left | new Mask(right);

		/// <summary>
		/// Inverts the bitmask bitwise
		/// </summary>>
		/// <returns>A new Mask with a bitwise negation of its bitmask</returns>
		public static Mask operator !(Mask mask) => new Mask(~mask.bits);

		/// <summary>
		/// Inverts the bitmask bitwise
		/// </summary>>
		/// <returns>A new Mask with a bitwise negation of its bitmask</returns>
		public static Mask operator ~(Mask mask) => new Mask(~mask.bits);

		/// <summary>
		/// Provides IEnumerable&lt;Layer&gt; to allow usage of foreach(Layer l in Mask)
		/// </summary>
		/// <returns></returns>
		public IEnumerator<Layer> GetEnumerator() => GetLayers().GetEnumerator();

		/// <summary>
		/// Provides IEnumerable&lt;Layer&gt; to allow usage of foreach(Layer l in Mask)
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <summary>
		/// Implicit operator int allows to compare mask to int by casting Mask to int via its bitmask
		/// </summary>
		/// <param name="mask"></param>
		/// <returns>The BitMask</returns>
		public static implicit operator int(Mask mask) => mask.bits;

		/// <summary>
		/// Implicit operator bool allows to test if any bit is set in the bitmask
		/// </summary>
		/// <param name="mask"></param>
		/// <returns>true if any bit is set, false otherwise</returns>
		public static implicit operator bool(Mask mask) => mask.bits > 0;

		/// <summary>
		/// Two masks are equal if and only if they have the exact same bitmask
		/// </summary>
		/// <param name="other">The bitmask to test for equality</param>
		/// <returns>true if both Masks have the exact same bitmask</returns>
		protected bool Equals(Mask other) => bits == other.bits;

		/// <summary>
		/// Equals return true if this and obj are ReferenceEqual. If obj is a Mask it return true
		/// if both bitmasks of this and obj are equal. If obj is a Layer it returns true if
		/// this contains the Layer
		/// </summary>
		/// <param name="obj">The other object to test for equality</param>
		/// <returns>true if equal or this contains obj if obj is a layer</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;

			if (obj.GetType() == Layer.Default.GetType())
				return this == (Layer) obj;
			return obj.GetType() == GetType() && this == (Mask) obj;
		}

		/// <summary>
		/// A Mask is uniquely represented by it's bitmask
		/// </summary>
		/// <returns>the bitmask as a hash</returns>
		public override int GetHashCode() => bits;

		/// <summary>
		/// Two masks are equal if both are null or if they have the exact same bitmask
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>true if both masks are null or their bitmasks are equal</returns>
		public static bool operator ==(Mask left, Mask right)
		{
			if (ReferenceEquals(left, right))
				return true;
			if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
				return false;
			return left.bits == right.bits;
		}

		/// <summary>
		/// A mask is not equal another mask one of them is null and the other isn't or
		/// if their bitmasks are not equal
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns>true if the mask does not contain the layer and only the layer</returns>
		public static bool operator !=(Mask left, Mask right) => !(left == right);

		/// <summary>
		/// A Layer is equal to a mask if the Mask contains the layer and only the layer
		/// </summary>
		/// <param name="left">The Mask to test if it contains the layer</param>
		/// <param name="right">The layer the masks needs to contain</param>
		/// <returns>true if the Mask contains the layert and only the layer</returns>
		public static bool operator ==(Mask left, int right) => left == new Mask((Layer) right);

		/// <summary>
		/// A Layer is not equal to a mask if the Mask does not contain exactly the layer
		/// </summary>
		/// <param name="left">The Mask to test if it contains the layer</param>
		/// <param name="right">The layer the masks needs to contain</param>
		/// <returns>true if the Mask does not contain the layer and only the layer</returns>
		public static bool operator !=(Mask left, int right) => !(left == right);

		/// <summary>
		/// A Layer is equal to a a layer if the mask contains the layer and only the layer
		/// </summary>
		/// <param name="left">The Mask to test if it contains the layer</param>
		/// <param name="right">The layer the masks needs to contain</param>
		/// <returns>true if the Mask contains the layer and only the layer</returns>
		public static bool operator ==(int left, Mask right) => new Mask((Layer) left) == right;

		/// <summary>
		/// A Layer is not equal to a layer if the Mask does not contain the layer and only the layer
		/// </summary>
		/// <param name="left">The Mask to test if it contains the layer</param>
		/// <param name="right">The layer the masks needs to contain</param>
		/// <returns>true if the Mask does not contain the layer and only the layer</returns>
		public static bool operator !=(int left, Mask right) => !(left == right);

		/// <summary>
		/// A Layer is equal to a mask if the Mask contains the layer and only the layer
		/// </summary>
		/// <param name="left">The Mask to test if it contains the layer</param>
		/// <param name="right">The layer the masks needs to contain</param>
		/// <returns>true if the Mask the layer and only the layer</returns>
		public static bool operator ==(Mask left, Layer right) => left == new Mask(right);

		/// <summary>
		/// A Layer is not equal to a mask if the Mask does not contain exactly the layer
		/// </summary>
		/// <param name="left">The Mask to test if it contains the layer</param>
		/// <param name="right">The layer the masks needs to contain</param>
		/// <returns>true if the Mask does not contain the layer and only the layer</returns>
		public static bool operator !=(Mask left, Layer right) => !(left == right);

		/// <summary>
		/// A Layer is equal to a mask if the Mask contains the layer  and only the layer
		/// </summary>
		/// <param name="left">The layer the masks needs to contain</param>
		/// <param name="right">The Mask to test if it contains the layer</param>
		/// <returns>true if the Mask contains only the layer</returns>
		public static bool operator ==(Layer left, Mask right) => right == left;

		/// <summary>
		/// A Layer is not equal to a mask if the Mask does not contain the layer and only the layer
		/// </summary>
		/// <param name="left">The layer the masks needs to contain</param>
		/// <param name="right">The Mask to test if it contains the layer</param>
		/// <returns>true if the Mask does not contain the layer and only the layer</returns>
		public static bool operator !=(Layer left, Mask right) => !(right == left);

		/// <summary>
		/// A Mask is equal to an Array of Masks if new Mask(Mask[]) is equal to the Mask.
		/// Mask== is commutitative
		/// </summary>
		/// <param name="left">The Mask to test for equality</param>
		/// <param name="right">The Mask[] to test for equality</param>
		/// <returns>true if Mask == new Mask(Mask[])</returns>
		public static bool operator ==(Mask left, Mask[] right)
		{
			if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
				return true;
			if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
				return false;
			return left == new Mask(right);
		}

		/// <summary>
		/// A Mask is not equal to Mask[] if the Mask is not equal to new Mask(Mask[])
		/// Mask!= is commutative.
		/// </summary>
		/// <param name="left">The Mask to test for equality</param>
		/// <param name="right">The Mask[] to test for equality</param>
		/// <returns>true if the Mask is not equal to new Mask(Mask[])</returns>
		public static bool operator !=(Mask left, Mask[] right) => !(left == right);

		/// <summary>
		/// A Mask is equal to an Array of Masks if new Mask(Mask[]) is equal to the Mask.
		/// Mask== is commutitative
		/// </summary>
		/// <param name="left">The Mask[] to test for equality</param>
		/// <param name="right">The Mask to test for equality</param>
		/// <returns>true if Mask == new Mask(Mask[])</returns>
		public static bool operator ==(Mask[] left, Mask right) => new Mask(left) == right;

		/// <summary>
		/// A Mask is not equal to Mask[] if the Mask is not equal to new Mask(Mask[]).
		/// Mask!= is commutative.
		/// </summary>
		/// <param name="left">The Mask to test for equality</param>
		/// <param name="right">The Mask[] to test for equality</param>
		/// <returns>true if the Mask is not equal to new Mask(Mask[])</returns>
		public static bool operator !=(Mask[] left, Mask right) => !(new Mask(left) == right);

		/// <summary>
		/// A Mask is equal to an Array of Layers if new Mask(Layer[]) is equal to the Mask.
		/// Mask== is commutitative
		/// </summary>
		/// <param name="left">The Mask to test for equality</param>
		/// <param name="right">The Layer[] to test for equality</param>
		/// <returns>true if Mask == new Mask(Layer[])</returns>
		public static bool operator ==(Mask left, Layer[] right) => left == new Mask(right);

		/// <summary>
		/// A Mask is not equal to Layer[] if the Mask is not equal to new Mask(Layer[]).
		/// Mask!= is commutative.
		/// </summary>
		/// <param name="left">The Mask to test for equality</param>
		/// <param name="right">The Layer[] to test for equality</param>
		/// <returns>true if the Mask is not equal to new Mask(Layer[])</returns>
		public static bool operator !=(Mask left, Layer[] right) => !(left == new Mask(right));

		/// <summary>
		/// A Mask is equal to an Array of Layers if new Mask(Layer[]) is equal to the Mask.
		/// Mask== is commutitative
		/// </summary>
		/// <param name="left">The Layer[] to test for equality</param>
		/// <param name="right">The Mask to test for equality</param>
		/// <returns>true if Mask == new Mask(Layer[])</returns>
		public static bool operator ==(Layer[] left, Mask right) => new Mask(left) == right;

		/// <summary>
		/// A Mask is not equal to Layer[] if the Mask is not equal to new Mask(Layer[]).
		/// Mask!= is commutative.
		/// </summary>
		/// <param name="left">The Layer[] to test for equality</param>
		/// <param name="right">The Mask to test for equality</param>
		/// <returns>true if the Mask is not equal to new Mask(Layer[])</returns>
		public static bool operator !=(Layer[] left, Mask right) => !(new Mask(left) == right);

		/// <summary>
		/// A Mask is equal to an Array of ints if new Mask(int[]) is equal to the Mask.
		/// Mask== is commutitative
		/// </summary>
		/// <param name="left">The Mask to test for equality</param>
		/// <param name="right">The int[] to test for equality</param>
		/// <returns>true if Mask == new Mask(int[])</returns>
		public static bool operator ==(Mask left, int[] right) => left == new Mask(right);

		/// <summary>
		/// A Mask is not equal to int[] if the Mask is not equal to new Mask(int[]).
		/// Mask!= is commutative.
		/// </summary>
		/// <param name="left">The Mask to test for equality</param>
		/// <param name="right">The int[] to test for equality</param>
		/// <returns>true if the Mask is not equal to new Mask(int[])</returns>
		public static bool operator !=(Mask left, int[] right) => !(left == new Mask(right));

		/// <summary>
		/// A Mask is equal to an Array of ints if new Mask(int[]) is equal to the Mask.
		/// Mask== is commutitative
		/// </summary>
		/// <param name="left">The int[] to test for equality</param>
		/// <param name="right">The Mask to test for equality</param>
		/// <returns>true if Mask == new Mask(int[])</returns>
		public static bool operator ==(int[] left, Mask right) => new Mask(left) == right;

		/// <summary>
		/// A Mask is not equal to int[] if the Mask is not equal to new Mask(int[]).
		/// Mask!= is commutative.
		/// </summary>
		/// <param name="left">The int[] to test for equality</param>
		/// <param name="right">The Mask to test for equality</param>
		/// <returns>true if the Mask is not equal to new Mask(int[])</returns>
		public static bool operator !=(int[] left, Mask right) => !(new Mask(left) == right);
	}
}