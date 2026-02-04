using System.Threading.Tasks;
using System.Net.Security;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Factories
{
    public interface IManager { }

    public enum ManagerType
    {
        GetManager,
        PostManager,
        AjaxManager,
    }

    public class ManagerFactory
    {
        public Dictionary<ManagerType, Dictionary<string, Func<IManager>>> _managers;
        public const string GENERIC = "Generic";

        public ManagerFactory()
        {
            _managers.Add(ManagerType.GetManager, new Dictionary<string, Func<IManager>>());
            _managers.Add(ManagerType.PostManager, new Dictionary<string, Func<IManager>>());
            _managers.Add(ManagerType.AjaxManager, new Dictionary<string, Func<IManager>>());
        }

        public IManager GetManager(ManagerType managerType, string programName)
        {
            if(!_managers.ContainsKey(managerType))
                throw new Exception($"No manager type of {managerType.ToString()} exists!");
            
            var innerFactory = _managers[managerType]();

            if(innerFactory.ContainsKey(programName))
                return innerFactory[programName]();
                return innerFactory[GENERIC]();
        }
        public string[] RegisteredManagerTypes() => _managers.Keys.ToArray();
        public string[] RegisteredManagers(ManagerType managerType) => _managers[managerType]().Keys.ToArray();
        public void RegisterManager(ManagerType managerType, string programName, Func<IManager> manager)
        {
            if(string.IsNullOrEmpty(programName))
                throw new Exception($"No program {programName} given!");

            if(!_managers.ContainsKey(managerType))
                _managers.Add(managerType, new Dictionary<string, Func<IManager>>());
            
            var innerFactory = _managers[managerType]();

            if(innerFactory.ContainsKey(programName))
                return;

            innerFactory.Add(programName, manager);
        }
    }

    public class ManagerBase : IManager
    {
        internal ManagerType ManagerType {get; private set; }
        internal string ManagerTypeName => ManagerType.ToString();
        internal string ProgramName { get; private set; }

        public ManagerBase() { }

        internal void Initialize(/* if you use autofac you may be able to move this to the startup itself*/) { }
    }

    public abstract class GetManager : ManagerBase
    {
        #region Common Methods
        public async Task Method01() { /* code goes here */ }
        public async Task Method02() { /* code goes here */ }
        public async Task Method03() { /* code goes here */ }
        #endregion

        #region Specific Program Based Methods - Extend In The Concrete Classes
        public abstract Task Rule01();
        public abstract Task Rule02();
        public abstract Task Rule03();
        public abstract Task Rule04();
        #endregion
    }
    public class GetManagerProgram01 : GetManager
    {
        public async Task Rule01() { /* code goes here */ }
        public async Task Rule02() { /* code goes here */ }
        public async Task Rule03() { /* code goes here */ }
        public async Task Rule04() { /* code goes here */ }
    }
    public class GetManagerProgram02 : GetManager
    {
        public async Task Rule01() { /* code goes here */ }
        public async Task Rule02() { /* code goes here */ }
        public async Task Rule03() { /* code goes here */ }
        public async Task Rule04() { /* code goes here */ }
    }

    public static class StartupManagerFactoryExtension
    {
        public static void RegisterManagerFactories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(_ =>
                var factory = new ManagerFactory();

                factory.RegisterManager(ManagerType.GetManager, GENERIC, () => new GetManager());
                factory.RegisterManager(ManagerType.GetManager, "Program01", () => new GetManagerProgram01());
                factory.RegisterManager(ManagerType.GetManager, "Program02", () => new GetManagerProgram02());

                return factory;
            );
        }
    }
}