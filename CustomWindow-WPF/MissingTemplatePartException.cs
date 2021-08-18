namespace CustomWindow_WPF
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="MissingTemplatePartException" />.
    /// </summary>
    [Serializable]
    internal class MissingTemplatePartException : Exception
    {
        /// <summary>
        /// Gets the PartName.
        /// </summary>
        public string PartName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the PartType.
        /// </summary>
        public Type? PartType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingTemplatePartException"/> class.
        /// </summary>
        public MissingTemplatePartException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingTemplatePartException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string?"/>.</param>
        public MissingTemplatePartException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingTemplatePartException"/> class.
        /// </summary>
        /// <param name="partName">The partName<see cref="string"/>.</param>
        /// <param name="partType">The partType<see cref="Type"/>.</param>
        public MissingTemplatePartException(string partName, Type partType)
        {
            PartName = partName;
            PartType = partType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingTemplatePartException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string?"/>.</param>
        /// <param name="innerException">The innerException<see cref="Exception?"/>.</param>
        public MissingTemplatePartException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingTemplatePartException"/> class.
        /// </summary>
        /// <param name="info">The info<see cref="SerializationInfo"/>.</param>
        /// <param name="context">The context<see cref="StreamingContext"/>.</param>
        protected MissingTemplatePartException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
