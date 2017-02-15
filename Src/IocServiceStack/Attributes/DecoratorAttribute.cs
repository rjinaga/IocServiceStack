#region License
// Copyright (c) 2016-2017 Rajeswara Rao Jinaga
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

namespace IocServiceStack
{
    using System;

    /*WHY IT TARGETS INTERFACE ONLY: We don't want to support the decorator for classes because there's a possibility to 
     set default values in default constructor and allow the derived classes to
     override. But the global decorator has a flexibility to override the constructed object.
     
    Not recommended decorators for classes, because it has alternate, 
    but there's no alternate set some defaults for an interface.    */

    /// <summary>
    /// The interface of DecoratorAttribute will be executed when service is being accessed, this has 
    /// two operations that work before and after invocation of the service call.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public abstract class DecoratorAttribute : Attribute
    {
        /// <summary>
        /// Executes before invocation of the service.
        /// </summary>
        /// <param name="context">The context of the service</param>
        public virtual void OnBeforeInvoke(ServiceCallContext context) { }

        /// <summary>
        /// Executes after invocation of the service, in this event, service instance 
        /// is accessible so that you can inject the data or dependencies that can be set through properties/fields.
        /// </summary>
        /// <param name="context">The context of the service</param>
        public virtual void OnAfterInvoke(ServiceCallContext context) { }
    }

}
