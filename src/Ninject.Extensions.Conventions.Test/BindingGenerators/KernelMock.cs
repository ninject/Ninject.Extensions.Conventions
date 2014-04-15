//-------------------------------------------------------------------------------
// <copyright file="KernelMock.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2013 Ninject Project Contributors
//   Authors: Remo Gloor (remo.gloor@gmail.com)
//           
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   you may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

#if !NO_MOQ && !NO_GENERIC_MOQ
namespace Ninject.Extensions.Conventions.BindingGenerators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using Ninject.Syntax;

    public class KernelMock
    {
        public KernelMock()
        {
            this.Mock = new Mock<IKernel>();
            this.Mock.Setup(m => m.Bind(It.IsAny<Type[]>())).Returns<Type[]>(this.CreateBindToMock);
            this.Bindings = new List<Binding>();
            this.ReturnedSyntax = new List<IBindingWhenInNamedWithOrOnSyntax<object>>();
        }

        public Mock<IKernel> Mock { get; private set; }

        public IKernel Object 
        { 
            get
            {
                return this.Mock.Object;
            } 
        }

        public List<Binding> Bindings { get; private set; }

        public List<IBindingWhenInNamedWithOrOnSyntax<object>> ReturnedSyntax { get; private set; }

        public void VerifyBindingCreated<TInterface, TImplementation>()
        {
            this.VerifyBindingCreated(new[] { typeof(TInterface) }, typeof(TImplementation));
        }

        public void VerifyBindingCreated(Type[] interfaceTypes, Type implementationType)
        {
            this.Bindings.Should().Contain(new Binding(interfaceTypes, implementationType));
        }

        public void VerifyAllBindingsCreated(IEnumerable<Type> interfaceTypes, Type implementationType)
        {
            this.Bindings.Count.Should().Be(interfaceTypes.Count());

            foreach (Type interfaceType in interfaceTypes)
            {
                this.VerifyBindingCreated(new[] { interfaceType }, implementationType);
            }
        }
        
        private IBindingToSyntax<object> CreateBindToMock(Type[] interfaceTypes)
        {
            var bindToMock = new Mock<IBindingToSyntax<object>>();
            bindToMock.Setup(m => m.To(It.IsAny<Type>())).Returns<Type>(implementationType => this.CreateConfigSyntax(interfaceTypes, implementationType));

            if (interfaceTypes.Length == 1)
            {
                bindToMock.Setup(m => m.ToSelf()).Returns(() => this.CreateConfigSyntax(interfaceTypes, interfaceTypes.Single()));
            }
            else
            {
                bindToMock.Setup(m => m.ToSelf())
                    .Throws(new Exception(".Bind(type1, type2,..).ToSelf() is not supported. To self is only supported for .Bind(oneType).ToSelf()"));
            }

            return bindToMock.Object;
        }

        private IBindingWhenInNamedWithOrOnSyntax<object> CreateConfigSyntax(Type[] interfaceTypes, Type implementationType)
        {
            var configSyntax = new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>().Object;
            this.ReturnedSyntax.Add(configSyntax);
            this.Bindings.Add(new Binding(interfaceTypes, implementationType));
            return configSyntax;
        }

        public class Binding
        {
            public Binding(Type[] interfaceTypes, Type implementationType)
            {
                this.InterfaceTypes = interfaceTypes;
                this.ImplementationType = implementationType;
            }

            public Type[] InterfaceTypes { get; private set; }

            public Type ImplementationType { get; private set; }

            public override bool Equals(object obj)
            {
                var other = obj as Binding;
                if (other == null)
                {
                    return false;
                }

                return 
                    this.ImplementationType.Equals(other.ImplementationType) && 
                    this.InterfaceTypes.SequenceEqual(other.InterfaceTypes);
            }

            public override int GetHashCode()
            {
                return this.ImplementationType.GetHashCode() + (13 * this.InterfaceTypes.GetHashCode());
            }
        }
    }
}
#endif