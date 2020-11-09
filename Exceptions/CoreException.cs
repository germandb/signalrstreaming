namespace SignalRStreaming.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    // TODO: (Localization) Translate the information to the appropriate language.

    /// <summary>
    ///     Exception for signalling core library errors.
    ///     - This exception will be launched by the microservices proxies when the RequestResultDTO
    ///     result is read and the RequestResultDTO contains an error code.
    ///     - The ExceptionHandlingMiddleware creates a RequestResultDTO with an error code when it
    ///     catches a CocktailException with error code and message.
    ///     - Only Cocktail.Core or DSL Tool generated code should explicitly throw exceptions from
    ///     this type.
    /// </summary>
    [Serializable]
    public class CoreException : ResultsException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="errorCode"> The error code. </param>
        public CoreException(ErrorCode errorCode)
            : base(ErrorCodeMessageTranslator.GetErrorMessage(errorCode.ToString()))
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="errorCode"> The error code. </param>
        /// <param name="errorMessage"> An accurate error message. </param>
        public CoreException(ErrorCode errorCode, string errorMessage)
            : base(errorMessage)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CoreException"/> class with the
        ///     <paramref name="correlationToken"/> as the tracking identifier.
        /// </summary>
        /// <param name="errorCode"> The error code. </param>
        /// <param name="correlationToken">
        ///     The correlation token that will be shown as the tracking identifier.
        /// </param>
        public CoreException(ErrorCode errorCode, Guid correlationToken)
            : base(ErrorCodeMessageTranslator.GetErrorMessage(errorCode.ToString(), correlationToken))
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="errorCode"> The error code. </param>
        /// <param name="innerException"> The inner exception. </param>
        public CoreException(ErrorCode errorCode, Exception innerException)
            : base(ErrorCodeMessageTranslator.GetErrorMessage(errorCode.ToString()), innerException)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="exception"> The exception. </param>
        public CoreException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <remarks>
        ///     This constructor is made for classes that inherit from
        ///     <see cref="CoreException"/>, that's the reason why it is protected.
        /// </remarks>
        protected CoreException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="serializationInfo"> Information describing the serialization. </param>
        /// <param name="streamingContext"> Context for the streaming. </param>
        protected CoreException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary> Gets the error code. </summary>
        /// <value> The error code. </value>
        public ErrorCode ErrorCode { get; }

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

            serializedObjectInformation.AddValue("ErrorCode", this.ErrorCode);

            base.GetObjectData(serializedObjectInformation, context);
        }
    }
}