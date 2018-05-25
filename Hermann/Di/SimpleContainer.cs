using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Hermann.Di
{
    public enum Lifestyle
    {
        Singleton,
        Transient,
    }

    public class SimpleContainer
    {
        private Dictionary<Type, System.Object> Instances { get; set; }

        private Dictionary<Type, Func<System.Object>> Funcs { get; set; }

        private Dictionary<Type, Lifestyle> Lifestyles { get; set; }

        private Dictionary<Type, ConstructorInfo> ConstructorInfos { get; set; }

        public SimpleContainer()
        {
            this.Instances = new Dictionary<Type, object>();
            this.Funcs = new Dictionary<Type, Func<object>>();
            this.Lifestyles = new Dictionary<Type, Lifestyle>();
            this.ConstructorInfos = new Dictionary<Type, ConstructorInfo>();
        }

        public void Register<TService>(Lifestyle lifestyle) where TService : class
        {
            var type = typeof(TService);
            var obj = (TService)type.GetConstructor(Type.EmptyTypes).Invoke(null);
            switch (lifestyle)
            {
                case Lifestyle.Singleton:
                    if (this.Instances.ContainsKey(type))
                    {
                        this.Instances[type] = obj;
                    }
                    else
                    {
                        this.Instances.Add(type, obj);
                    }
                    break;
                case Lifestyle.Transient:
                default:
                    throw new NotSupportedException("invalid lifestyle");
            }
            this.Lifestyles[type] = lifestyle;
        }

        public void Register<TService>(Func<TService> func) where TService : class
        {
            this.Register<TService>(func, Lifestyle.Transient);
        }

        public void Register<TService>(Func<TService> func, Lifestyle lifestyle) where TService : class
        {
            var type = typeof(TService);
            switch (lifestyle)
            {
                case Lifestyle.Singleton:
                    if (this.Instances.ContainsKey(type))
                    {
                        this.Instances[type] = func();
                    }
                    else
                    {
                        this.Instances.Add(type, func());
                    }
                    break;
                case Lifestyle.Transient:
                    if (this.Funcs.ContainsKey(type))
                    {
                        this.Funcs[type] = func;
                    }
                    else
                    {
                        this.Funcs.Add(type, func);
                    }
                    break;
                default:
                    throw new NotSupportedException("invalid lifestyle");
            }
            this.Lifestyles[type] = lifestyle;
        }

        public TService GetInstance<TService>() where TService : class
        {
            var type = typeof(TService);
            var lifestyle = Lifestyle.Transient;
            if (this.Lifestyles.ContainsKey(type))
            {
                lifestyle = this.Lifestyles[type];
            }

            switch (lifestyle)
            {
                case Lifestyle.Singleton:
                    if (!this.Instances.ContainsKey(type))
                    {
                        throw new InvalidOperationException("not registerd type");
                    }
                    return (TService)this.Instances[type];
                case Lifestyle.Transient:
                    if (this.Funcs.ContainsKey(type))
                    {
                        return (TService)this.Funcs[type]();
                    }
                    else
                    {
                        if (!this.ConstructorInfos.ContainsKey(type))
                        {
                            this.ConstructorInfos.Add(type, type.GetConstructor(Type.EmptyTypes));
                        }
                        //var ins = (TService)this.ConstructorInfos[type].Invoke(null);
                        var ins = (TService)Activator.CreateInstance(type, null);
                        return ins;
                    }
                default:
                    throw new NotSupportedException("invalid lifestyle");
            }
        }

        public void Verify()
        {
            
        }
    }
}
