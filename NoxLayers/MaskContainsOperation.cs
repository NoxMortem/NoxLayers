using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.NoxLayers
{
    /// <summary>
    /// MaskContainsOperation is a helper class to allow the following syntax
    /// Mask.Contains().All(params T) => T.All(Mask.Contains)
    /// Mask.Contains().Any(params T) => T.Any(Mask.Contains)
    /// Mask.Contains().None(params T) => !T.Any(Mask.Contains)
    /// </summary>
    public class MaskContainsOperation
    {
        /// <summary>
        /// The Mask this operation operates on.
        /// </summary>
        private readonly Mask mask;

        /// <summary>
        /// Creates anew MaskContainsOperation
        /// </summary>
        /// <param name="mask">The mask to perform Contains operation on</param>
        public MaskContainsOperation(Mask mask)
        {
            this.mask = mask;
        }

        /// <summary>
        /// Tests if this mask contains all of the other masks. A Mask Contains a Mask if it contains all of its layers
        /// </summary>
        /// <param name="others">The masks to test</param>
        /// <returns>true if this mask contains all given masks</returns>
        /// <seealso cref="Mask.Contains(Mask)"/>
        public bool All(params Mask[] others) => others.All(mask.Contains);

        /// <summary>
        /// Tests if this mask contains any of the other masks. A Mask Contains a Mask if it contains all of its layers
        /// </summary>
        /// <param name="others">The masks to test</param>
        /// <returns>true if this mask contains any of the given masks</returns>
        /// <seealso cref="Mask.Contains(Mask)"/>
        public bool Any(params Mask[] others) => others.Any(mask.ContainsAndNotEmpty);

        /// <summary>
        /// Tests if this mask does not contain any of the other masks. A Mask Contains a Mask if it contains all of its layers
        /// </summary>
        /// <param name="others">The masks to test</param>
        /// <returns>true if this mask does not contain any of the given masks</returns>
        /// <seealso cref="Mask.Contains(Mask)"/>
        public bool None(params Mask[] others) => !Any(others);

        /// <summary>
        /// Tests if this mask does contain all of the layers. A Mask contains a Layer if the corresponding bit (0-31) is set.s
        /// </summary>
        /// <param name="others">The layers to test</param>
        /// <returns>true if this mask does contain all of the given layers</returns>
        /// <seealso cref="Mask.Contains(Layer)"/>
        public bool All(params Layer[] others) => others.All(mask.Contains);

        /// <summary>
        /// Tests if this mask does contain all of the given layers and nothing else. A Mask contains a Layer if the corresponding bit (0-31) is set.
        /// </summary>
        /// <param name="others">The layers to test</param>
        /// <returns>true if this mask does contain all given layers and nothing else</returns>
        /// <seealso cref="Mask.Contains(int)"/>
        public bool Only(params Mask[] others) => others.All(mask.Contains)
                                                  && others.Aggregate(new HashSet<Layer>(),
                                                                      (agg, next) =>
                                                                          new HashSet<Layer
                                                                          >(agg.Union(next.GetLayers()))).Count
                                                  == mask.LayerCount;

        /// <summary>
        /// Tests if this mask does contain any of the layers. A Mask contains a Layer if the corresponding bit (0-31) is set.
        /// </summary>
        /// <param name="others">The layers to test</param>
        /// <returns>true if this mask does contain any of the given layers</returns>
        /// <seealso cref="Mask.Contains(Layer)"/>
        public bool Any(params Layer[] others) => others.Any(mask.Contains);

        /// <summary>
        /// Tests if this mask does contain exactly the given layers and nothing else. A Mask contains a Layer if the corresponding bit (0-31) is set.
        /// </summary>
        /// <param name="others">The layers to test</param>
        /// <returns>true if this mask does contain exactly the given layers and nothing else</returns>
        /// <seealso cref="Mask.Contains(Layer)"/>
        public bool Only(params Layer[] others) => others.All(mask.Contains) && others.Length == mask.LayerCount;

        /// <summary>
        /// Tests if this mask does not contain any of the given layers. A Mask contains a Layer if the corresponding bit (0-31) is set.
        /// </summary>
        /// <param name="others">The layers to test</param>
        /// <returns>true if this mask does not contain any given layer</returns>
        /// <seealso cref="Mask.Contains(Layer)"/>
        public bool None(params Layer[] others) => !Any(others);

        /// <summary>
        /// Tests if this mask does contain all of the given layers. A Mask contains a Layer if the corresponding bit (0-31) is set.
        /// </summary>
        /// <param name="others">The layers to test</param>
        /// <returns>true if this mask does contain all given layer</returns>
        /// <seealso cref="Mask.Contains(int)"/>
        public bool All(params int[] others) => others.All(mask.Contains);

        /// <summary>
        /// Tests if this mask does contain any of the given layers. A Mask contains a Layer if the corresponding bit (0-31) is set.
        /// </summary>
        /// <param name="others">The layers to test</param>
        /// <returns>true if this mask does contain any given layer</returns>
        /// <seealso cref="Mask.Contains(int)"/>
        public bool Any(params int[] others) => others.Any(mask.Contains);

        /// <summary>
        /// Tests if this mask does not contain any of the given layers. A Mask contains a Layer if the corresponding bit (0-31) is set.
        /// </summary>
        /// <param name="others">The layers to test</param>
        /// <returns>true if this mask does not contain any given layer</returns>
        /// <seealso cref="Mask.Contains(int)"/>        
        public bool None(params int[] others) => !Any(others);

        /// <summary>
        /// Tests if this mask does contain all of the given layers and nothing else. A Mask contains a Layer if the corresponding bit (0-31) is set.
        /// </summary>
        /// <param name="others">The layers to test</param>
        /// <returns>true if this mask does contain all given layers and nothing else</returns>
        /// <seealso cref="Mask.Contains(int)"/>
        public bool Only(params int[] others) => others.All(mask.Contains) && others.Length == mask.LayerCount;
    }
}