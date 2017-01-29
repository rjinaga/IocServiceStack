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
    public static class ContainerServiceExtensions
    {
        public static IContainerExtension Add<TC>(this IContainerService service, Func<TC> expression) where TC : class
        {
            return InternalAdd<TC>(service, expression, null);
        }

        public static IContainerExtension Add<TC>(this IContainerService service, Func<TC> expression, string serviceName) where TC : class
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }

            return InternalAdd<TC>(service, expression, serviceName);
        }

        public static IContainerExtension Replace<TC>(this IContainerService service, Func<TC> expression) where TC : class
        {
            return InternalReplace<TC>(service, expression, null);
        }

        public static IContainerExtension Replace<TC>(this IContainerService service, Func<TC> expression, string serviceName) where TC : class
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }

            return InternalReplace<TC>(service, expression, serviceName);
        }

        private static IContainerExtension InternalReplace<TC>(IContainerService service, Func<TC> expression, string serviceName) where TC : class
        {
            var containerExtension = service as IContainerExtension;
            if (containerExtension != null)
            {
                if (string.IsNullOrWhiteSpace(serviceName))
                {
                    containerExtension.Replace<TC>(expression);
                }
                else
                {
                    containerExtension.Replace<TC>(expression, serviceName);
                }
            }
            else
                throw new Exception("Service factory is not implemented IContainerExtension");

            return containerExtension;
        }

        private static IContainerExtension InternalAdd<TC>(this IContainerService service, Func<TC> expression, string serviceName) where TC : class
        {
            var containerExtension = service as IContainerExtension;
            if (containerExtension != null)
            {
                if (string.IsNullOrWhiteSpace(serviceName))
                {
                    containerExtension.Add<TC>(expression);
                }
                else
                {
                    containerExtension.Add<TC>(expression, serviceName);
                }
            }
            else
                throw new Exception("Service factory is not implemented IContainerExtension");

            return containerExtension;
        }
    }
}
