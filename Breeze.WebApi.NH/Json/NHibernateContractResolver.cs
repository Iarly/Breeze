<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate;
using NHibernate.Proxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Breeze.WebApi.NH
{
    /// <summary>
    /// Newtonsoft.Json ContractResolver for NHibernate objects.
    /// Allows JSON serializer to skip properties that are not already resolved, thus preventing the 
    /// serializer from trying to serialize the entire object graph.
    /// Code copied from https://github.com/PeteGoo/NHibernate.QueryService
    /// </summary>
    public class NHibernateContractResolver : DefaultContractResolver
    {
        //protected override JsonContract CreateContract(Type objectType) {
        //    if (typeof(NHibernate.Proxy.INHibernateProxy).IsAssignableFrom(objectType))

        //        return base.CreateContract(objectType.BaseType);
        //    else
        //        return base.CreateContract(objectType);
        //}

        private static readonly MemberInfo[] NHibernateProxyInterfaceMembers = typeof(INHibernateProxy).GetMembers();

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var members = base.GetSerializableMembers(objectType);

            members.RemoveAll(memberInfo =>
                              (IsMemberPartOfNHibernateProxyInterface(memberInfo)) ||
                              (IsMemberDynamicProxyMixin(memberInfo)) ||
                              (IsMemberMarkedWithIgnoreAttribute(memberInfo, objectType)) ||
                              (IsMemberInheritedFromProxySuperclass(memberInfo, objectType)));

            return members;

            //var actualMemberInfos = new List<MemberInfo>();

            //foreach (var memberInfo in members) {
            //    var infos = memberInfo.DeclaringType.BaseType.GetMember(memberInfo.Name);
            //    actualMemberInfos.Add(infos.Length == 0 ? memberInfo : infos[0]);
            //}

            //return actualMemberInfos;
        }

        private static bool IsMemberDynamicProxyMixin(MemberInfo memberInfo)
        {
            return memberInfo.Name == "__interceptors";
        }

        private static bool IsMemberInheritedFromProxySuperclass(MemberInfo memberInfo, Type objectType)
        {
            return memberInfo.DeclaringType.Assembly == typeof(INHibernateProxy).Assembly;
        }

        private static bool IsMemberMarkedWithIgnoreAttribute(MemberInfo memberInfo, Type objectType)
        {
            var infos = typeof(INHibernateProxy).IsAssignableFrom(objectType)
                          ? objectType.BaseType.GetMember(memberInfo.Name)
                          : objectType.GetMember(memberInfo.Name);

            return infos[0].GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length > 0;
        }

        private static bool IsMemberPartOfNHibernateProxyInterface(MemberInfo memberInfo)
        {
            return Array.Exists(NHibernateProxyInterfaceMembers, mi => memberInfo.Name == mi.Name);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            property.ShouldSerialize =
            instance =>
            {
                var inited = NHibernateUtil.IsInitialized(instance);
                return inited;
            };

            return property;
        }

    }
=======
using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate;
using NHibernate.Proxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;

namespace Breeze.WebApi.NH
{
    /// <summary>
    /// Newtonsoft.Json ContractResolver for NHibernate objects.
    /// Allows JSON serializer to skip properties that are not already resolved, thus preventing the
    /// serializer from trying to serialize the entire object graph.
    /// Code copied from https://github.com/PeteGoo/NHibernate.QueryService
    /// </summary>
    public class NHibernateContractResolver : DefaultContractResolver
    {
        public new static readonly NHibernateContractResolver Instance = new NHibernateContractResolver();

        /*
        private static readonly MemberInfo[] NHibernateProxyInterfaceMembers = typeof(INHibernateProxy).GetMembers();

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var members = base.GetSerializableMembers(objectType);

            members.RemoveAll(memberInfo =>
                              (IsMemberPartOfNHibernateProxyInterface(memberInfo)) ||
                              (IsMemberDynamicProxyMixin(memberInfo)) ||
                              (IsMemberMarkedWithIgnoreAttribute(memberInfo, objectType)) ||
                              (IsMemberInheritedFromProxySuperclass(memberInfo, objectType)));

            return members;

            //var actualMemberInfos = new List<MemberInfo>();

            //foreach (var memberInfo in members) {
            //    var infos = memberInfo.DeclaringType.BaseType.GetMember(memberInfo.Name);
            //    actualMemberInfos.Add(infos.Length == 0 ? memberInfo : infos[0]);
            //}

            //return actualMemberInfos;
        }

        private static bool IsMemberDynamicProxyMixin(MemberInfo memberInfo)
        {
            return memberInfo.Name == "__interceptors";
        }

        private static bool IsMemberInheritedFromProxySuperclass(MemberInfo memberInfo, Type objectType)
        {
            return memberInfo.DeclaringType.Assembly == typeof(INHibernateProxy).Assembly;
        }

        private static bool IsMemberMarkedWithIgnoreAttribute(MemberInfo memberInfo, Type objectType)
        {
            var infos = typeof(INHibernateProxy).IsAssignableFrom(objectType)
                          ? objectType.BaseType.GetMember(memberInfo.Name)
                          : objectType.GetMember(memberInfo.Name);

            return infos[0].GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length > 0;
        }

        private static bool IsMemberPartOfNHibernateProxyInterface(MemberInfo memberInfo)
        {
            return Array.Exists(NHibernateProxyInterfaceMembers, mi => memberInfo.Name == mi.Name);
        }
        */
        /// <summary>
        /// Control serialization NHibernate collections by using JsonProperty.ShouldSerialize.
        /// Serialization should only be attempted on collections that are initialized.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            PropertyInfo pinfo = member as PropertyInfo;

            if ((typeof(string) != property.PropertyType) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && pinfo != null)
            {
                property.ShouldSerialize =
                instance =>
                {
                    var value = pinfo.GetValue(instance);
                    var inited = NHibernateUtil.IsInitialized(value);
                    return inited;
                };
            }

            return property;
        }


    }
>>>>>>> 9fe7264a8ea74100b2ba57719bc9e8f615dc7393
}