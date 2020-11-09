namespace SignalRStreaming.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.SignalR;
    using Newtonsoft.Json;
    using SignalRStreaming.Exceptions;

    /// <summary>
    ///     Request result DTO. This class defines the returning of error codes from Application to
    ///     Proxy, the Proxy must process this result and launch an exception if necessary.
    /// </summary>
    /// <typeparam name="T"> The request result type. </typeparam>
    public class RequestResultDTO<T>
    {
        /// <summary> The request result. </summary>
        private T result;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RequestResultDTO{T}"/> class.
        /// </summary>
        /// <param name="result"> The result. </param>
        public RequestResultDTO(T result)
        {
            this.result = result;

            this.ErrorCodes = new List<int>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RequestResultDTO{T}"/> class.
        /// </summary>
        /// <param name="correlationToken"> The correlation token. </param>
        public RequestResultDTO(Guid correlationToken)
        {
            this.CorrelationToken = correlationToken;

            this.ErrorCodes = new List<int>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RequestResultDTO{T}"/> class.
        /// </summary>
        /// <param name="errorCodes">
        ///     The codes of the errors given in the execution of the action.
        /// </param>
        /// <param name="correlationToken"> The correlation token. </param>
        public RequestResultDTO(IEnumerable<int> errorCodes, Guid correlationToken)
        {
            this.ErrorCodes = errorCodes ?? new List<int>();
            this.CorrelationToken = correlationToken;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RequestResultDTO{T}"/> class.
        ///     Constructor for serialization purposes.
        /// </summary>
        /// <param name="result"> The result data. </param>
        /// <param name="errorCodes"> The error codes. </param>
        /// <param name="correlationToken"> The correlation token. </param>
        [JsonConstructor]
        public RequestResultDTO(T result, IEnumerable<int> errorCodes, Guid correlationToken)
        {
            this.result = result;
            this.ErrorCodes = errorCodes ?? new List<int>();
            this.CorrelationToken = correlationToken;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RequestResultDTO{T}"/> class based on <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        /// <param name="correlationToken"> The correlation token. </param>
        /// <returns> The created <see cref="RequestResultDTO{T}"/>. </returns>
        public RequestResultDTO(Exception exception, Guid correlationToken)
        {
            if (exception is ResultsException resultsException)
            {
                List<int> errorCodes = new List<int>();

                PopulateError(resultsException, errorCodes);

                this.ErrorCodes = errorCodes;
                this.CorrelationToken = correlationToken;
            }
            else
            {
                this.ErrorCodes = new List<int> { (int)ErrorCode.UnknownError };
                this.CorrelationToken = correlationToken;
            }
        }

        /// <summary>
        ///     Gets the result data of the request. It is marked for type name serialization,
        ///     because we often request types that inherit from base classes, but because the
        ///     <see cref="RequestResultDTO{T}"/> type parameter is the same as the
        ///     <see cref="RequestResultDTO{T}.Result"/> type, the Newtonsoft.JSON serializer does
        ///     not add the type, it expects that you know the type parameter.
        ///     <para>
        ///         For example, in the Contract, we specify a parent EntityDTO return type. As the
        ///         Services layer knows what to return, it creates a RequestResultDTO of Type
        ///         UserDTO and not of type EntityDTO, and because of this, the type does not get
        ///         serialized with TypeNameHandling.Auto.
        ///     </para>
        /// </summary>
        /// <value> The result. </value>
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public T Result => this.GetResult();

        /// <summary>
        ///     Gets the list of errors codes and messages of the exceptions thrown executing the
        ///     action. It is used to thrown the same exceptions in the client without having to
        ///     serialize the exceptions.
        /// </summary>
        /// <value> The error codes. </value>
        /// <remarks>
        ///     This property is public to be able to serialize it and use it in the constructor
        ///     when the object is deserialized.
        /// </remarks>
        public IEnumerable<int> ErrorCodes { get; }

        /// <summary> Gets the correlation token. </summary>
        /// <value> The correlation token. </value>
        public Guid CorrelationToken { get; }

        /// <summary>
        ///     Queries the result should be serialized. The result is true when there are error
        ///     codes because then the result is not important because the action of the request
        ///     couldn't be properly processed and an exception will be thrown. This is needed by
        ///     the Newtonsoft.Json serializer because otherwise, when it tries to serialize the
        ///     Result string, the property launches an exception.
        /// </summary>
        /// <returns> True if it should serialize, false if it not. </returns>
        public bool ShouldSerializeResult()
        {
            return !this.ErrorCodes.Any();
        }

        /// <summary>
        ///     Populates the given <paramref name="errorCodes"/> and
        ///     <paramref name="resultsException"/> collections based on the errors of <paramref name="resultsException"/>.
        /// </summary>
        /// <param name="resultsException">
        ///     The <see cref="ResultsException"/> to get the errors/validations from.
        /// </param>
        /// <param name="errorCodes"> The list of ErrorCodes that will be populated. </param>
        private static void PopulateError(ResultsException resultsException, List<int> errorCodes)
        {
            switch (resultsException)
            {
                case CoreException cocktailCoreException:
                    errorCodes.Add((int)cocktailCoreException.ErrorCode);

                    break;
                default:
                    errorCodes.Add((int)ErrorCode.UnknownError);

                    break;
            }
        }

        /// <summary> Gets the result. </summary>
        /// <exception cref="CocktailCoreException">
        ///     Thrown when error code is null or message is not null or white space.
        /// </exception>
        /// <returns> The result. </returns>
        private T GetResult()
        {
            if (!this.ErrorCodes.Any())
            {
                return this.result;
            }

            List<ResultsException> exceptions = new List<ResultsException>();

            foreach (int errorCode in this.ErrorCodes)
            {
                // The error code will determine the exception type.
                if (errorCode > 0 && errorCode < 1000)
                {
                    exceptions.Add(new CoreException((ErrorCode)errorCode)
                    {
                        CorrelationToken = this.CorrelationToken,
                    });

                    continue;
                }

                // When the error code is not known or 0, an UnknownError is added.
                exceptions.Add(new CoreException(ErrorCode.UnknownError, this.CorrelationToken));
            }

            throw new HubException(GetProperMessage(exceptions));
        }

        /// <summary>
        ///     Gets the proper message for the <see cref="CocktailAggregateException"/>. When there
        ///     is only one exception, its message is shown in the Message property of the
        ///     CocktailAggregateException to avoid having to check the inner exception to see its message.
        /// </summary>
        /// <param name="exceptions"> The exceptions. </param>
        /// <returns> The proper message for the exception. </returns>
        private static string GetProperMessage(IEnumerable<ResultsException> exceptions)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(ExceptionMessages.CocktailAggregateExceptionMessage);

            foreach (ResultsException exception in exceptions)
            {
                stringBuilder.AppendLine();
                stringBuilder.Append("- ").Append(exception.Message);
            }

            return stringBuilder.ToString();
        }
    }
}