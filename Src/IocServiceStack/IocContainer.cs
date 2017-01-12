﻿#region License
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
    public class IocContainer
    {
        private IServiceProvider _serviceProvider;
        private readonly ServiceConfig _config;
        
        internal ServiceConfig Config => _config;
        public IServiceProvider ServiceProvider => _serviceProvider;

        public IocContainer(ServiceConfig config)
        {
            _config = config;
            Prepare();
        }

        private void Prepare()
        {
            _serviceProvider = _config.ServiceProvider ?? new DefaultServiceProvider(_config);
            //set decorators to service provider
            _serviceProvider.DecoratorManager = new DecoratorManager() { GlobalDecorators = _config.Decorators };
        }

        #region Static Members
        public static IocContainer GlobalIocContainer { get; internal set; }
        #endregion
    }

    

}
