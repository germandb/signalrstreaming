﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SignalRStreaming.Exceptions {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ErrorCodeMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorCodeMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SignalRStreaming.Exceptions.ErrorCodeMessages", typeof(ErrorCodeMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The entity creation was not allowed because the used DTO does not support it..
        /// </summary>
        internal static string CreateEntityNotAllowedError {
            get {
                return ResourceManager.GetString("CreateEntityNotAllowedError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The data that is trying to be inserted already exists in the database..
        /// </summary>
        internal static string CreateExistingEntityError {
            get {
                return ResourceManager.GetString("CreateExistingEntityError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The filter used in a GetData operation does not provide a valid measurement..
        /// </summary>
        internal static string FilterNotUsingValidMeasurement {
            get {
                return ResourceManager.GetString("FilterNotUsingValidMeasurement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The entity can&apos;t be deleted because other instances reference it. Delete first the entities that reference it..
        /// </summary>
        internal static string ForeignKeyConstraintError {
            get {
                return ResourceManager.GetString("ForeignKeyConstraintError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expression that is trying to be created from the condition is not valid..
        /// </summary>
        internal static string InvalidConditionError {
            get {
                return ResourceManager.GetString("InvalidConditionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The domain entity type of the entity is not valid..
        /// </summary>
        internal static string NotValidDomainEntityType {
            get {
                return ResourceManager.GetString("NotValidDomainEntityType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Some instances have been created but not all the given fields were set because the application user is not authorized..
        /// </summary>
        internal static string PartialCreate {
            get {
                return ResourceManager.GetString("PartialCreate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Some instances have been deleted but others not because the application user is not authorized to delete them..
        /// </summary>
        internal static string PartialDelete {
            get {
                return ResourceManager.GetString("PartialDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Some instances have been updated but others not and/or some of the fields were not updated because the application user is not authorized..
        /// </summary>
        internal static string PartialUpdate {
            get {
                return ResourceManager.GetString("PartialUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unauthorized action has been attempted..
        /// </summary>
        internal static string UnauthorizedAction {
            get {
                return ResourceManager.GetString("UnauthorizedAction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A field that is not authorized for view has been used in a filtering or sorting operation..
        /// </summary>
        internal static string UnauthorizedFilterOrSort {
            get {
                return ResourceManager.GetString("UnauthorizedFilterOrSort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A unique value constraint has been violated and the changes can&apos;t be saved..
        /// </summary>
        internal static string UniqueValueConstraintError {
            get {
                return ResourceManager.GetString("UniqueValueConstraintError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an unknown error in the application..
        /// </summary>
        internal static string UnknownError {
            get {
                return ResourceManager.GetString("UnknownError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The entity can´t be updated because other user has deleted it..
        /// </summary>
        internal static string UpdateAfterDeleteError {
            get {
                return ResourceManager.GetString("UpdateAfterDeleteError", resourceCulture);
            }
        }
    }
}