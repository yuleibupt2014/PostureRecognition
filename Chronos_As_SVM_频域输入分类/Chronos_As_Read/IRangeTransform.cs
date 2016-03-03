
namespace SVM
{
    /// <summary>
    /// Interface implemented by range transforms.
    /// </summary>
    public interface IRangeTransform
    {
        /// <summary>
        /// Transform the input value using the transform stored for the provided index.
        /// </summary>
        /// <param name="input">Input value</param>
        /// <param name="index">Index of the transform to use</param>
        /// <returns>The transformed value</returns>
        double Transform(double input, int index);
        /// <summary>
        /// Transforms the input array.
        /// </summary>
        /// <param name="input">The array to transform</param>
        /// <returns>The transformed array</returns>
        Node[] Transform(Node[] input);
    }
}
