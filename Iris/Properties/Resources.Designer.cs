﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Iris.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Iris.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to [
        ///    {
        ///        &quot;AgentString&quot;: &quot;BaiDuSpider&quot;,
        ///        &quot;AgentType&quot;: &quot;Crawler&quot;,
        ///        &quot;AgentName&quot;: &quot;Baiduspider&quot;,
        ///        &quot;OSType&quot;: &quot;unknown&quot;,
        ///        &quot;OSName&quot;: &quot;unknown&quot;,
        ///        &quot;DeviceType&quot;: &quot;Crawler&quot;
        ///    },
        ///    {
        ///        &quot;AgentString&quot;: &quot;Baiduspider+(+http:\/\/www.baidu.com\/search\/spider.htm)&quot;,
        ///        &quot;AgentType&quot;: &quot;Crawler&quot;,
        ///        &quot;AgentName&quot;: &quot;Baiduspider&quot;,
        ///        &quot;OSType&quot;: &quot;unknown&quot;,
        ///        &quot;OSName&quot;: &quot;unknown&quot;,
        ///        &quot;DeviceType&quot;: &quot;Crawler&quot;
        ///    },
        ///    {
        ///        &quot;AgentString&quot;: [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string agent_list {
            get {
                return ResourceManager.GetString("agent_list", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Redo_16x {
            get {
                object obj = ResourceManager.GetObject("Redo_16x", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Redo_grey_16x {
            get {
                object obj = ResourceManager.GetObject("Redo_grey_16x", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Undo_16x {
            get {
                object obj = ResourceManager.GetObject("Undo_16x", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Undo_grey_16x {
            get {
                object obj = ResourceManager.GetObject("Undo_grey_16x", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}