<<<<<<< HEAD
﻿using Models.NorthwindIB.NH;
using NHibernate;
using NHibernate.Cfg;

namespace Sample_WebApi2.Controllers
{
    public static class NorthwindNHConfig
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;

        static NorthwindNHConfig()
        {
            var modelAssembly = typeof(Customer).Assembly;

            // Configure NHibernate
            _configuration = new Configuration();
            _configuration.Configure();  //configure from the app.config
            _configuration.AddAssembly(modelAssembly);  // mapping is in this assembly

            _sessionFactory = _configuration.BuildSessionFactory();
        }

        public static Configuration Configuration
        {
            get { return _configuration; }
        }

        public static ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        public static ISession OpenSession()
        {
            ISession session = _sessionFactory.OpenSession();
            return session;
        }


    }
=======
﻿using Models.NorthwindIB.NH;
using NHibernate;
using NHibernate.Cfg;

namespace Sample_WebApi2.Controllers
{
    public static class NorthwindNHConfig
    {
        //private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;

        static NorthwindNHConfig()
        {
            var modelAssembly = typeof(Customer).Assembly;

            // Configure NHibernate
            var configuration = new Configuration();
            configuration.Configure();  //configure from the app.config
            configuration.AddAssembly(modelAssembly);  // mapping is in this assembly

            _sessionFactory = configuration.BuildSessionFactory();
        }

        //public static Configuration Configuration
        //{
        //    get { return _configuration; }
        //}

        public static ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        public static ISession OpenSession()
        {
            ISession session = _sessionFactory.OpenSession();
            return session;
        }


    }
>>>>>>> 9fe7264a8ea74100b2ba57719bc9e8f615dc7393
}