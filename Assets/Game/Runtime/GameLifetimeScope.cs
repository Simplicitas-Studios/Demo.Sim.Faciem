using System;
using Sim.Faciem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Runtime
{
    public class GameLifetimeScope : LifetimeScope, IDIRegistrationBridge
    {
        private IContainerBuilder _builder;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            _builder = builder;
            
            builder.Register<IDIContainerBridge, FaciemVContainerBridge>(Lifetime.Singleton);
            builder.RegisterEntryPoint<FaciemSetupStartup>();
            
            FaciemBootstrapper.RegisterServices(this);
        }

        public void RegisterSingleton<TInterface, TImpl>() where TImpl : class, TInterface
        {
            _builder.Register<TInterface, TImpl>(Lifetime.Singleton);
        }

        public void RegisterTransient<TInterface, TImpl>() where TImpl : class, TInterface
        {
            _builder.Register<TInterface, TImpl>(Lifetime.Transient);
        }

        public void RegisterSingleton<TInterface, TImpl>(params Type[] aliases) where TImpl : class, TInterface
        {
            _builder.Register<TImpl>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }

        public void RegisterTransient<TInterface, TImpl>(params Type[] aliases) where TImpl : class, TInterface
        {
            _builder.Register<TImpl>(Lifetime.Transient)
                .AsImplementedInterfaces();
        }

        public void RegisterTransient(Type tInterface, Type tImpl)
        {
            _builder.Register(
                tInterface, 
                tImpl,
                Lifetime.Transient);
        }
    }
}