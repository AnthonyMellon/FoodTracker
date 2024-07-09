namespace FoodTracker.Scripts.Utils
{
    public static class ErrorUtils
    {
#region ErrorMessages
        /// <summary>
        /// A collection generic of error messages
        /// </summary>
        public static class Messages
        {
            /// <summary>
            /// When <paramref name="param"/> is null and you dont want it to be
            /// </summary>
            /// <param name="param">The param that should not be null</param>
            /// <returns></returns>
            public static string IsNull(string param)
            {
                return $"{param} cannot be null";
            }

            /// <summary>
            /// When <paramref name="param"/> is empty and you dont want it to be
            /// </summary>
            /// <param name="param">The param that should not be empty</param>
            /// <returns></returns>
            public static string IsEmpty(string param)
            {
                return $"{param} cannot be empty";
            }

            /// <summary>
            /// When <paramref name="param"/> is negative and you dont want it to be
            /// </summary>
            /// <param name="param">The name of the param that should be positive</param>
            /// <returns></returns>
            public static string IsNegative(string param)
            {
                return $"{param} cannot be less than zero";
            }

            /// <summary>
            /// When a <paramref name="param"/> with <paramref name="value"/> already exists and you don't want doubles
            /// </summary>
            /// <param name="param">The name of the param the should be unique</param>
            /// <param name="value">The value of the param that should be unique</param>
            /// <returns></returns>
            public static string AlreadyExists(string param, string value)
            {
                return $"{param} with value {value} already exists";
            }
        }
#endregion
    }
}
