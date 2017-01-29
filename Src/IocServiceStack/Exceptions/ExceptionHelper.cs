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
    internal class ExceptionHelper
    {
        internal static Exception ThrowServiceNotRegisteredException(string name)
        {
            throw new ServiceNotFoundException($"Requested service of '{name}' contract was not found. Check your configuraion of {nameof(IocServicelet)}. You might have configured different namespaces or assemblies where the requested service was not found.");
        }

        internal static void ThrowDuplicateServiceException(string fullName)
        {
            throw new DuplicateServiceImplementaionException(fullName);
        }

        internal static void ThrowArgumentException(string message)
        {
            throw new ArgumentException(message);
        }

        internal static void ThrowInvalidServiceType(Type contractType,Type  serviceType)
        {
            throw new InvalidServiceTypeException($"'{serviceType.FullName}' cannot be assigned to '{contractType.FullName}'");
        }

        internal static void ThrowContractNotRegisteredException(string fullName)
        {
            throw new ContractNotRegisteredException(fullName);
        }
        internal static void ThrowArgumentNullException(string name)
        {
            throw new ArgumentNullException(name);
        }

        internal static void ThrowOverrideObserverExpection()
        {
            throw new OverrideObserverExpection();
        }
    }
}