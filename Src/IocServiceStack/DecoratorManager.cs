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
    using System.Collections.Generic;
    /// <summary>
    /// <see cref="DecoratorManager"/> executes the registered decorators. It contains
    /// global decorators and while executing decorators for a service, it executes the global decorators first and
    /// then executes specific decorators to the contract.
    /// </summary>
    public class DecoratorManager : IDecoratorManager
    {
        /// <summary>
        /// Gets or sets list of global decorators.
        /// </summary>
        public List<DecoratorAttribute> GlobalDecorators { set; get; }

        /// <summary>
        /// Executes decorator
        /// </summary>
        /// <param name="context">Specify the service call context object.</param>
        /// <param name="case"></param>
        /// <param name="localDecorators">Specify decorators of the contract.</param>
        public void Execute(ServiceCallContext context, InvocationCase @case, IEnumerable<DecoratorAttribute> localDecorators)
        {
            switch (@case)
            {
                case InvocationCase.Before:
                    /*Invoke first global decorators and then local decorators*/
                    if (GlobalDecorators != null && GlobalDecorators.Count > 0)
                    {
                        BeforeInvoce(context, GlobalDecorators);
                    }
                    if (localDecorators != null)
                    {
                        BeforeInvoce(context, localDecorators);
                    }
                    break;
                case InvocationCase.After:
                    /*Invoke first global decorators and then local decorators*/
                    if (GlobalDecorators != null && GlobalDecorators.Count > 0)
                    {
                        AfterInvoke(context, GlobalDecorators);
                    }
                    if (localDecorators != null)
                    {
                        AfterInvoke(context, localDecorators);
                    }
                    break;
            }
        }

        private void BeforeInvoce(ServiceCallContext context, IEnumerable<DecoratorAttribute> decorators)
        {
            foreach (var decorator in decorators)
            {
                decorator.OnBeforeInvoke(context);
            }
        }

        private void AfterInvoke(ServiceCallContext context, IEnumerable<DecoratorAttribute> decorators)
        {
            foreach (var decorator in decorators)
            {
                decorator.OnAfterInvoke(context);
            }
        }
    }
}
