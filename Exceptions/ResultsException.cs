namespace SignalRStreaming.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary> Exception for signalling errors with validation messages. </summary>
    [Serializable]
    public abstract class ResultsException : Exception
    {
        /// <summary> Initializes a new instance of the <see cref="ResultsException"/> class. </summary>
        protected ResultsException()
            : base()
        {
        }

        /// <summary> Initializes a new instance of the <see cref="ResultsException"/> class. </summary>
        /// <param name="message"> The message. </param>
        /// <remarks>
        ///     This constructor is made for classes that inherit from
        ///     <see cref="CocktailCoreException"/>, that's the reason why it is protected.
        /// </remarks>
        protected ResultsException(string message)
            : base(message)
        {
        }

        /// <summary> Initializes a new instance of the <see cref="ResultsException"/> class. </summary>
        /// <param name="message"> The exception message. </param>
        /// <param name="exception"> The exception. </param>
        protected ResultsException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <summary> Initializes a new instance of the <see cref="ResultsException"/> class. </summary>
        /// <param name="serializationInfo"> Information describing the serialization. </param>
        /// <param name="streamingContext"> Context for the streaming. </param>
        protected ResultsException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary> Gets or sets the correlation token. </summary>
        /// <value> The correlation token. </value>
        public Guid CorrelationToken { get; set; }

        /// <summary> Gets an object data. </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="serializedObjectInformation">
        ///     The data needed to serialize/deserialize the object.
        /// </param>
        /// <param name="context"> The context. </param>
        public override void GetObjectData(SerializationInfo serializedObjectInformation, StreamingContext context)
        {
            if (serializedObjectInformation == null)
            {
                throw new ArgumentNullException(nameof(serializedObjectInformation));
            }

            base.GetObjectData(serializedObjectInformation, context);
        }
    }
}