#region License
// Copyright (c) 2016 Rajeswara-Rao-Jinaga
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
    
    public class ServiceInfo<TC, TS> : ServiceInfo where TC: class where TS: TC
    {
        private Func<TS> _actionInfo;

        public ServiceInfo(DecoratorAttribute[] decorators, string serviceName) : base(typeof(TS), decorators, serviceName)
        {
        }

        public ServiceInfo(bool isReusable, DecoratorAttribute[] decorators, string serviceName) : base(typeof(TS), decorators, isReusable, serviceName)
        {
        }

        public ServiceInfo(Func<TS> serviceAction, DecoratorAttribute[] decorators, string serviceName) : base(null, decorators, serviceName)
        {
            _actionInfo = serviceAction;
        }

        public ServiceInfo(Func<TS> serviceAction, DecoratorAttribute[] decorators, bool isReusable, string serviceName) : base(null, decorators, isReusable, serviceName)
        {
            _actionInfo = serviceAction;
        }

        public override Func<T1> GetActionInfo<T1>() 
        {
            return _actionInfo as Func<T1>;
        }
    }
}
