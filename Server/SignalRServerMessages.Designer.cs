﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SignalRStreaming.Server {
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
    internal class SignalRServerMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SignalRServerMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SignalRStreaming.Server.SignalRServerMessages", typeof(SignalRServerMessages).Assembly);
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
        ///   Looks up a localized string similar to Client &apos;{0}&apos; connected..
        /// </summary>
        internal static string ClientConnected {
            get {
                return ResourceManager.GetString("ClientConnected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client &apos;{0}&apos; explicitly closed the connection..
        /// </summary>
        internal static string ClientExplicitlyClosedTheConnection {
            get {
                return ResourceManager.GetString("ClientExplicitlyClosedTheConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client &apos;{0}&apos; joined..
        /// </summary>
        internal static string ClientJoined {
            get {
                return ResourceManager.GetString("ClientJoined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client &apos;{0}&apos; from group &apos;{1}&apos; left..
        /// </summary>
        internal static string ClientLeftGroup {
            get {
                return ResourceManager.GetString("ClientLeftGroup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client &apos;{0}&apos; timed out..
        /// </summary>
        internal static string ClientTimeOut {
            get {
                return ResourceManager.GetString("ClientTimeOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an exception processing the stream..
        /// </summary>
        internal static string ReturnProcessedStreamException {
            get {
                return ResourceManager.GetString("ReturnProcessedStreamException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The parameter &apos;{0}&apos; is null..
        /// </summary>
        internal static string TheParameterIsNull {
            get {
                return ResourceManager.GetString("TheParameterIsNull", resourceCulture);
            }
        }
    }
}
