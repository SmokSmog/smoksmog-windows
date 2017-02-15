namespace SmokSmog.Model
{
    public class Norm
    {
        /// <summary>
        /// Norm Aggregation Type
        /// </summary>
        public AggregationType Aggregation { get; internal set; }

        /// <summary>
        /// Full name of particulate
        /// example: Carbon Monoxide
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The value of the norm expressed in units defined by unit field in Parameter class
        /// </summary>
        public double Value { get; internal set; }
    }
}