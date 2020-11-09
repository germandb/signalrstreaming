namespace SignalRStreaming
{
    /// <summary> Values that represent the Cocktail application errors. </summary>
    /// <remarks>
    ///     Here we should add only error codes that are common to all the applications in Cocktail.
    ///     Normally, those error code will correspond to exceptions defined in Cocktail.Core solution.
    /// </remarks>
    public enum ErrorCode
    {
        /// <summary> An unknown error in the application. </summary>
        UnknownError = 0,

        /// <summary> An update after delete concurrency error in the application. </summary>
        UpdateAfterDeleteConcurrencyError = 1,

        /// <summary> The entity that is trying to be inserted already exists in the context. </summary>
        CreateExistingEntityError = 2,

        /// <summary>
        ///     The expression that is trying to be created from the condition is not valid.
        /// </summary>
        InvalidConditionError = 3,

        /// <summary> The DTO used to create the entity does not allow entity creation. </summary>
        CreateEntityNotAllowedError = 4,

        /// <summary>
        ///     The filter used in a GetData operation for a field with Units of Measurement does
        ///     not provide a valid measurement.
        /// </summary>
        FilterNotUsingValidMeasurement = 5,

        /// <summary> A constraint of unique value has been violated. </summary>
        UniqueValueConstraintError = 6,

        /// <summary> The type of the Domain entity is not valid. </summary>
        NotValidDomainEntityType = 7,

        /// <summary> A constraint of a foreign key has been violated. </summary>
        ForeignKeyConstraintError = 8,
    }
}