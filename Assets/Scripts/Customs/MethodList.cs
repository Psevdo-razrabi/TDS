using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Customs.Data;
using Input;
using Input.Interface;
using SaveSystem;
using Zenject;

namespace Customs
{
    public class MethodList
    {
        private readonly Type _typeFireMode = typeof(ChangeModeFire);
        private IReadOnlyCollection<MethodName> _methodNames;
        
        [Inject]
        private void Construct(ISetFireModes changeModeFire, UnitOfWorks unitOfWorks)
        {
            unitOfWorks.DataContext.Load();
            _methodNames = unitOfWorks.RepositoryNameMethod.GetCollectionsItems();
            changeModeFire.SetFireModes(SetMethod());
        }
        
        private List<MethodInfo> SetMethod()
        {
            return _methodNames.Select(x => _typeFireMode
                .GetMethod(x.methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)).ToList();
        }
    }
}