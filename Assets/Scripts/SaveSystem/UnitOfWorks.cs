using Customs.Data;
using Cysharp.Threading.Tasks;
using SaveSystem.Repositories;

namespace SaveSystem
{
    public class UnitOfWorks
    {
        public Repository<MethodName> RepositoryNameMethod { get; }
        public Repository<MethodLabelName> RepositoryLabelName { get; }
        
        public readonly DataContext DataContext;
        
        public UnitOfWorks(Repository<MethodName> repositoryNameMethod, Repository<MethodLabelName> repositoryLabelName, DataContext dataContext)
        {
            RepositoryNameMethod = repositoryNameMethod;
            RepositoryLabelName = repositoryLabelName;
            DataContext = dataContext;
        }

        public async UniTask Save()
        { 
            await DataContext.Save();
        }
    }
}