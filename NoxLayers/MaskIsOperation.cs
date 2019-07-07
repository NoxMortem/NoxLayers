using System.Linq;

namespace Infrastructure.NoxLayers
{
    /// <summary>
    /// A MaskIsOperation is a wrapper to allow fluent syntax for Mask.Is() operations. It generates a small
    /// overhead but allows for easier syntax.
    /// </summary>
    public class MaskIsOperation
    {
        /// <summary>
        /// The Mask to operate on.
        /// </summary>
        private readonly Mask mask;

        /// <summary>
        /// Generates a new MaskIsOperation to perform Is operations on.
        /// </summary>
        /// <param name="mask"></param>
        public MaskIsOperation(Mask mask)
        {
            this.mask = mask;
        }

        /// <summary>
        /// Tests if the provided mask is == to any of the given layers. A Mask is == to a Layer
        /// if the layers bit is set and no other.
        /// </summary>
        /// <param name="others"></param>
        /// <returns>true if the Mask is equal to any layer</returns>
        public bool Any(params int[] others) => others.Any(other => mask == other);

        /// <summary>
        /// Tests if the provided mask is == to any of the given layers. A Mask is == to a Layer
        /// if the layers bit is set and no other.
        /// </summary>
        /// <param name="others"></param>
        /// <returns>true if the Mask is equal to any layer</returns>
        public bool Any(params Layer[] others) => others.Any(other => mask == other);

        /// <summary>
        /// Tests if the provided mask is == to any of the given masks. A Mask is == to another Mask
        /// if their bitsets are equal
        /// </summary>
        /// <param name="others"></param>
        /// <returns>true if the Mask is equal to any layer</returns>
        public bool Any(params Mask[] others) => others.Any(other => mask == other);

        /// <summary>
        /// A Mask is exactly int[] if it is equal
        /// </summary>
        /// <param name="others"></param>
        /// <returns>true if mask and int[] are equal</returns>
        public bool Exactly(params int[] others) => mask == others;

        /// <summary>
        /// A Mask is exactly Layer[] if it is equal
        /// </summary>
        /// <param name="others"></param>
        /// <returns>true if mask and Layer[] are equal</returns>
        public bool Exactly(params Layer[] others) => mask == others;

        /// <summary>
        /// A Mask is exactly Mask[] if it is equal
        /// </summary>
        /// <param name="others"></param>
        /// <returns>true if mask and Mask[] are equal</returns>
        public bool Exactly(params Mask[] others) => mask == others;

        /// <summary>
        /// A Mask is not int[] if it is not equal
        /// </summary>
        /// <param name="others"></param>
        /// <returns>true if mask and int[] are not equal</returns>
        public bool Not(params int[] others) => mask != others;

        /// <summary>
        /// A Mask is not Layer[] if it is not equal
        /// </summary>
        /// <param name="others"></param>
        /// <returns>true if mask and Layer[] are not equal</returns>
        public bool Not(params Layer[] others) => mask != others;

        /// <summary>
        /// A Mask is not Mask[] if it is not equal
        /// </summary>
        /// <param name="others"></param>
        /// <returns>true if mask and Mask[] are not equal</returns>
        public bool Not(params Mask[] others) => mask != others;

        /// <summary>
        /// A Mask is none int[] if it is not equal to any int
        /// </summary>
        /// <param name="others"></param>
        /// <returns>true if the mask is not equal to any int</returns>
        public bool None(params int[] others) => others.All(other => mask != other);

        /// <summary>
        /// A Mask is none Layer[] if it is not equal to any Layer
        /// </summary>
        /// <param name="others"></param>
        /// <returns>true if the mask is not equal to any Layer</returns>
        public bool None(params Layer[] others) => others.All(other => mask != other);

        /// <summary>
        /// A Mask is none Mask[] if it is not equal to any Mask
        /// </summary>
        /// <param name="others"></param>
        /// <returns>true if the mask is not equal to any Mask</returns>
        public bool None(params Mask[] others) => others.All(other => mask != other);
    }
}