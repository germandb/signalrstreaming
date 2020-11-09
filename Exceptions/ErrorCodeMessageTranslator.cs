namespace SignalRStreaming.Exceptions
{
    using System;
    using System.Globalization;

    /// <summary>
    ///     Error code message translator. Translates an <see cref="ErrorCode"/> to the
    ///     corresponding error message.
    /// </summary>
    internal static class ErrorCodeMessageTranslator
    {
        /// <summary>
        ///     Gets the message from a corresponding error code. Reads the value from the
        ///     <see cref="ErrorCodeMessages"/> resources, doing a match with the
        ///     <paramref name="errorCodeName"/>. If a match is not found, it tries to match
        ///     <see cref="ErrorCode.UnknownError"/>, and if a match is not present, returns a
        ///     generic string.
        /// </summary>
        /// <param name="errorCodeName"> The error code name in the enumeration. </param>
        /// <returns> The error message from the error code enumeration. </returns>
        internal static string GetErrorMessage(string errorCodeName)
        {
            return GetErrorMessageFromEnum(errorCodeName)
                ?? GetErrorMessageFromEnum(ErrorCode.UnknownError.ToString())
                ?? "There was an unknown error in the application.";
        }

        /// <summary>
        ///     Gets the message from a corresponding error code. Reads the value from the
        ///     <see cref="ErrorCodeMessages"/> resources, doing a match with the
        ///     <paramref name="errorCodeName"/>. If a match is not found, it tries to match
        ///     <see cref="ErrorCode.UnknownError"/>, and if a match is not present, returns a
        ///     generic string. The message will also contain the
        ///     <paramref name="correlationToken"/> as the tracking identifier.
        /// </summary>
        /// <param name="errorCodeName"> The error code name in the enumeration. </param>
        /// <param name="correlationToken">
        ///     The correlation token that will be shown as the tracking identifier.
        /// </param>
        /// <returns> The error message from the error code enumeration. </returns>
        internal static string GetErrorMessage(string errorCodeName, Guid correlationToken)
        {
            return $"{GetErrorMessage(errorCodeName)} | Tracking identifier: {correlationToken}";
        }

        /// <summary> Gets an error message from the name of an Error Code enumeration. </summary>
        /// <param name="errorCodeName"> The error code name of an Error Code enumeration. </param>
        /// <returns> The error message from the error code name enumeration. </returns>
        private static string GetErrorMessageFromEnum(string errorCodeName)
        {
            return ErrorCodeMessages.ResourceManager.GetString(errorCodeName, CultureInfo.CurrentUICulture);
        }
    }
}