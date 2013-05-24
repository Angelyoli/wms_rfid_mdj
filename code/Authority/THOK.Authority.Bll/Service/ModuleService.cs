﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Bll.Models;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Service
{
    public class ModuleService : ServiceBase<Module>, IModuleService
    {
        [Dependency]
        public IUserRepository UserRepository { get; set; }
        [Dependency]
        public ICityRepository CityRepository { get; set; }
        [Dependency]
        public ISystemRepository SystemRepository { get; set; }
        [Dependency]
        public IModuleRepository ModuleRepository { get; set; }
        [Dependency]
        public IUserSystemRepository UserSystemRepository { get; set; }
        [Dependency]
        public IUserModuleRepository UserModuleRepository { get; set; }
        [Dependency]
        public IUserFunctionRepository UserFunctionRepository { get; set; }
        [Dependency]
        public IRoleSystemRepository RoleSystemRepository { get; set; }
        [Dependency]
        public IRoleModuleRepository RoleModuleRepository { get; set; }
        [Dependency]
        public IRoleFunctionRepository RoleFunctionRepository { get; set; }
        [Dependency]
        public IFunctionRepository FunctionRepository { get; set; }
        [Dependency]
        public IRoleRepository RoleRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region 模块信息维护        
        
        public object GetDetails(string systemID)
        {
            IQueryable<THOK.Authority.DbModel.System> querySystem = SystemRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.Module> queryModule = ModuleRepository.GetQueryable();
            var systems = querySystem.AsEnumerable();
            if (systemID != null && systemID != string.Empty)
            {
                Guid gsystemid = new Guid(systemID);
                systems = querySystem.Where(i => i.SystemID == gsystemid)
                                     .Select(i => i);
            }

            HashSet<Menu> systemMenuSet = new HashSet<Menu>();
            foreach (var system in systems)
            {
                Menu systemMenu = new Menu();
                systemMenu.ModuleID = system.SystemID.ToString();
                systemMenu.ModuleName = system.SystemName;
                systemMenu.SystemID = system.SystemID.ToString();
                systemMenu.SystemName = system.SystemName;

                var modules = queryModule.Where(m => m.System.SystemID == system.SystemID && m.ModuleID == m.ParentModule.ModuleID)
                                         .OrderBy(m => m.ShowOrder)
                                         .Select(m => m);
                HashSet<Menu> moduleMenuSet = new HashSet<Menu>();
                foreach (var item in modules)
                {
                    Menu moduleMenu = new Menu();
                    moduleMenu.ModuleID = item.ModuleID.ToString();
                    moduleMenu.ModuleName = item.ModuleName;
                    moduleMenu.SystemID = item.System.SystemID.ToString();
                    moduleMenu.SystemName = item.System.SystemName;
                    moduleMenu.ParentModuleID = item.ParentModule.ModuleID.ToString();
                    moduleMenu.ParentModuleName = item.ParentModule.ModuleName;

                    moduleMenu.ModuleURL = item.ModuleURL;
                    moduleMenu.iconCls = item.IndicateImage;
                    moduleMenu.ShowOrder = item.ShowOrder;
                    moduleMenuSet.Add(moduleMenu);
                    GetChildMenu(moduleMenu, item);
                    moduleMenuSet.Add(moduleMenu);
                }
                systemMenu.children = moduleMenuSet.ToArray();
                systemMenuSet.Add(systemMenu);
            }

            return systemMenuSet.ToArray();
        }

        public bool Add(string moduleName, int showOrder, string moduleUrl, string indicateImage, string desktopImage, string systemID, string moduleID)
        {
            IQueryable<THOK.Authority.DbModel.System> querySystem = SystemRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.Module> queryModule = ModuleRepository.GetQueryable();
            moduleID = !String.IsNullOrEmpty(moduleID) ? moduleID : "40DD7298-F410-43F2-840A-7C04F09B5CE2";
            var system = querySystem.FirstOrDefault(i => i.SystemID == new Guid(systemID));
            var parentModule = queryModule.FirstOrDefault(i => i.ModuleID == new Guid(moduleID));
            var module = new Module();
            module.ModuleID = Guid.NewGuid();
            module.ModuleName = moduleName;
            module.ShowOrder = showOrder;
            module.ModuleURL = moduleUrl;
            module.IndicateImage = indicateImage;
            module.DeskTopImage = desktopImage;
            module.System = system;
            module.ParentModule = parentModule ?? module;
            ModuleRepository.Add(module);
            ModuleRepository.SaveChanges();
            return true;
        }

        public bool Delete(string moduleID)
        {
            IQueryable<THOK.Authority.DbModel.Module> queryModule = ModuleRepository.GetQueryable();

            Guid gmoduleId = new Guid(moduleID);
            var module = queryModule.FirstOrDefault(i => i.ModuleID == gmoduleId);
            if (module != null)
            {
                Del(FunctionRepository, module.Functions);
                Del(ModuleRepository, module.Modules);
                Del(RoleModuleRepository, module.RoleModules);
                Del(UserModuleRepository, module.UserModules);

                ModuleRepository.Delete(module);
                ModuleRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(string moduleID, string moduleName, int showOrder, string moduleUrl, string indicateImage, string deskTopImage)
        {
            IQueryable<THOK.Authority.DbModel.Module> queryModule = ModuleRepository.GetQueryable();
            Guid sid = new Guid(moduleID);
            var module = queryModule.FirstOrDefault(i => i.ModuleID == sid);
            module.ModuleName = moduleName;
            module.ShowOrder = showOrder;
            module.ModuleURL = moduleUrl;
            module.IndicateImage = indicateImage;
            module.DeskTopImage = deskTopImage;
            ModuleRepository.SaveChanges();
            return true;
        }

        private void GetChildMenu(Menu menu, Module module)
        {
            HashSet<Menu> childMenuSet = new HashSet<Menu>();
            var modules = from m in module.Modules
                          orderby m.ShowOrder
                          select m;
            foreach (var item in modules)
            {
                if (item != module)
                {
                    Menu childMenu = new Menu();
                    childMenu.ModuleID = item.ModuleID.ToString();
                    childMenu.ModuleName = item.ModuleName;
                    childMenu.SystemID = item.System.SystemID.ToString();
                    childMenu.SystemName = item.System.SystemName;
                    childMenu.ParentModuleID = item.ParentModule.ModuleID.ToString();
                    childMenu.ParentModuleName = item.ParentModule.ModuleName;
                    childMenu.ModuleURL = item.ModuleURL;
                    childMenu.iconCls = item.IndicateImage;
                    childMenu.ShowOrder = item.ShowOrder;
                    childMenuSet.Add(childMenu);
                    if (item.Modules.Count > 0)
                    {
                        GetChildMenu(childMenu, item);
                    }
                }
            }
            menu.children = childMenuSet.ToArray();
        }

        #endregion

        #region 页面权限控制        
        
        public object GetUserMenus(string userName, string cityID, string systemID)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("值不能为NULL或为空。", "userName");
            if (String.IsNullOrEmpty(cityID)) throw new ArgumentException("值不能为NULL或为空。", "cityID");
            if (String.IsNullOrEmpty(systemID)) throw new ArgumentException("值不能为NULL或为空。", "systemId");

            IQueryable<THOK.Authority.DbModel.User> queryUser = UserRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.City> queryCity = CityRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.System> querySystem = SystemRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.RoleModule> queryRoleModule = RoleModuleRepository.GetQueryable();

            Guid gSystemID = new Guid(systemID);
            Guid gCityID = new Guid(cityID);
            var user = queryUser.Single(u => u.UserName == userName);
            var city = queryCity.Single(c => c.CityID == gCityID);
            var system = querySystem.Single(s => s.SystemID == gSystemID);
            InitUserSystem(user, city, system);

            var userSystem = (from us in user.UserSystems
                              where us.User.UserID == user.UserID
                                && us.City.CityID == city.CityID
                                && us.System.SystemID == system.SystemID
                              select us).Single();

            HashSet<Menu> systemMenuSet = new HashSet<Menu>();
            Menu systemMenu = new Menu();
            systemMenu.ModuleID = userSystem.System.SystemID.ToString();
            systemMenu.ModuleName = userSystem.System.SystemName;
            systemMenu.SystemID = userSystem.System.SystemID.ToString();
            systemMenu.SystemName = userSystem.System.SystemName;

            var userModules = from um in userSystem.UserModules
                              where um.Module.ModuleID == um.Module.ParentModule.ModuleID
                              orderby um.Module.ShowOrder
                              select um;

            HashSet<Menu> moduleMenuSet = new HashSet<Menu>();
            foreach (var userModule in userModules)
            {
                var roles = user.UserRoles.Select(ur => ur.Role);
                foreach (var role in roles)
                {
                    InitRoleSystem(role, city, system);
                }

                if (userModule.IsActive ||
                    userModule.Module.RoleModules.Any(rm => roles.Any(r => r.RoleID == rm.RoleSystem.Role.RoleID
                        && rm.IsActive)) ||
                    userModule.Module.Modules
                        .Any(m => m.UserModules.Any(um => um.UserSystem.User.UserID == userModule.UserSystem.User.UserID 
                            && (um.IsActive || um.UserFunctions.Any(uf=>uf.IsActive)))) ||
                    userModule.Module.Modules
                        .Any(m => m.RoleModules.Any(rm => roles.Any(r=>r.RoleID == rm.RoleSystem.Role.RoleID 
                            && (rm.IsActive || rm.RoleFunctions.Any(rf=>rf.IsActive) )))) ||
                    user.UserName == "Admin"
                    )
                {
                    var module = userModule.Module;
                    Menu moduleMenu = new Menu();
                    moduleMenu.ModuleID = module.ModuleID.ToString();
                    moduleMenu.ModuleName = module.ModuleName;
                    moduleMenu.SystemID = module.System.SystemID.ToString();
                    moduleMenu.SystemName = module.System.SystemName;
                    moduleMenu.ParentModuleID = module.ParentModule.ModuleID.ToString();
                    moduleMenu.ParentModuleName = module.ParentModule.ModuleName;
                    moduleMenu.ModuleURL = module.ModuleURL;
                    moduleMenu.iconCls = module.IndicateImage;
                    moduleMenu.ShowOrder = module.ShowOrder;
                    GetChildMenu(moduleMenu, userSystem, module);
                    moduleMenuSet.Add(moduleMenu);
                }
            }
            systemMenu.children = moduleMenuSet.ToArray();
            systemMenuSet.Add(systemMenu);
            return systemMenuSet.ToArray();
        }

        public object GetModuleFuns(string userName, string cityID, string moduleID)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("值不能为NULL或为空。", "userName");
            if (String.IsNullOrEmpty(cityID)) throw new ArgumentException("值不能为NULL或为空。", "cityID");
            if (String.IsNullOrEmpty(moduleID)) throw new ArgumentException("值不能为NULL或为空。", "moduleID");

            IQueryable<THOK.Authority.DbModel.User> queryUser = UserRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.City> queryCity = CityRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.Module> queryModule = ModuleRepository.GetQueryable();

            Guid gCityID = new Guid(cityID);
            Guid gModuleID = new Guid(moduleID);
            var user = queryUser.Single(u => u.UserName == userName);
            var city = queryCity.Single(c => c.CityID == gCityID);
            var module = queryModule.Single(m => m.ModuleID == gModuleID);

            var userModule = (from um in module.UserModules
                              where um.UserSystem.User.UserID == user.UserID
                                && um.UserSystem.City.CityID == city.CityID
                              select um).Single();

            var userFunctions = userModule.UserFunctions;
            Fun fun = new Fun();
            HashSet<Fun> moduleFunctionSet = new HashSet<Fun>();
            foreach (var userFunction in userFunctions)
            {
                var roles = user.UserRoles.Select(ur=>ur.Role);
                bool bResult = userFunction.Function.RoleFunctions.Any(
                        rf=> roles.Any(
                            r=>r.RoleID == rf.RoleModule.RoleSystem.Role.RoleID 
                               && rf.IsActive
                         )
                    );
                moduleFunctionSet.Add(new Fun()
                    {
                        funid = userFunction.Function.FunctionID.ToString(),
                        funname = userFunction.Function.ControlName,
                        iconCls = userFunction.Function.IndicateImage,
                        isActive = userFunction.IsActive || bResult || user.UserName=="Admin"
                    });

            }
            fun.funs = moduleFunctionSet.ToArray();
            return fun;
        }

        private void GetChildMenu(Menu moduleMenu, UserSystem userSystem, Module module)
        {
            IQueryable<THOK.Authority.DbModel.RoleModule> queryRoleModule = RoleModuleRepository.GetQueryable();
            HashSet<Menu> childMenuSet = new HashSet<Menu>();
            var userModules = from um in userSystem.UserModules
                              where um.Module.ParentModule == module
                              orderby um.Module.ShowOrder
                              select um;
            foreach (var userModule in userModules)
            {
                var childModule = userModule.Module;
                if (childModule != module)
                {
                    var roles = userSystem.User.UserRoles.Select(ur=> ur.Role);
                    if (userModule.IsActive || userModule.UserFunctions.Any(uf=>uf.IsActive) ||
                        userModule.Module.RoleModules.Any(rm => roles.Any(r => r.RoleID == rm.RoleSystem.Role.RoleID
                            && (rm.IsActive || rm.RoleFunctions.Any(rf=>rf.IsActive)))) ||
                        userModule.Module.Modules
                            .Any(m => m.UserModules.Any(um => um.UserSystem.User.UserID == userModule.UserSystem.User.UserID
                                && (um.IsActive || um.UserFunctions.Any(uf => uf.IsActive)))) ||
                        userModule.Module.Modules
                            .Any(m => m.RoleModules.Any(rm => roles.Any(r => r.RoleID == rm.RoleSystem.Role.RoleID
                                && (rm.IsActive || rm.RoleFunctions.Any(rf => rf.IsActive))))) ||
                        userModule.UserSystem.User.UserName == "Admin"
                        )
                    {
                        Menu childMenu = new Menu();
                        childMenu.ModuleID = childModule.ModuleID.ToString();
                        childMenu.ModuleName = childModule.ModuleName;
                        childMenu.SystemID = childModule.System.SystemID.ToString();
                        childMenu.SystemName = childModule.System.SystemName;
                        childMenu.ParentModuleID = childModule.ParentModule.ModuleID.ToString();
                        childMenu.ParentModuleName = childModule.ParentModule.ModuleName;
                        childMenu.ModuleURL = childModule.ModuleURL;
                        childMenu.iconCls = childModule.IndicateImage;
                        childMenu.ShowOrder = childModule.ShowOrder;
                        childMenuSet.Add(childMenu);
                        if (childModule.Modules.Count > 0)
                        {
                            GetChildMenu(childMenu, userSystem, childModule);
                        }
                    }
                }
            }
            moduleMenu.children = childMenuSet.ToArray();
        }

        #endregion

        #region 初始化角色权限

        private void InitRoleSystem(Role role, City city, THOK.Authority.DbModel.System system)
        {
            var roleSystems = role.RoleSystems.Where(rs => rs.City.CityID == city.CityID
                && rs.System.SystemID == system.SystemID);
            if (roleSystems.Count() == 0)
            {
                RoleSystem rs = new RoleSystem()
                {
                    RoleSystemID = Guid.NewGuid(),
                    Role = role,
                    City = city,
                    System = system,
                    IsActive = false
                };
                RoleSystemRepository.Add(rs);
                RoleSystemRepository.SaveChanges();
            }

            var roleSystem = role.RoleSystems.Single(rs => rs.City.CityID == city.CityID
                && rs.System.SystemID == system.SystemID);
            InitRoleModule(roleSystem);
        }

        private void InitRoleModule(RoleSystem roleSystem)
        {
            foreach (var module in roleSystem.System.Modules)
            {
                var roleModules = roleSystem.RoleModules.Where(rm => rm.Module.ModuleID == module.ModuleID
                    && rm.RoleSystem.System.SystemID == roleSystem.System.SystemID);
                if (roleModules.Count() == 0)
                {
                    RoleModule rm = new RoleModule()
                    {
                        RoleModuleID = Guid.NewGuid(),
                        RoleSystem = roleSystem,
                        Module = module,
                        IsActive = false
                    };
                    roleSystem.IsActive = false;
                    SetParentRoleModuleIsActiveFalse(rm);
                    RoleModuleRepository.Add(rm);
                    RoleModuleRepository.SaveChanges();
                }
                var roleModule = roleSystem.RoleModules.Single(rm => rm.Module.ModuleID == module.ModuleID
                    && rm.RoleSystem.System.SystemID == roleSystem.System.SystemID);
                InitRoleFunctions(roleModule);
            }
        }

        private void SetParentRoleModuleIsActiveFalse(RoleModule roleModule)
        {
            var parentRoleModule = roleModule.Module.ParentModule.RoleModules.FirstOrDefault(prm => prm.RoleSystem.Role.RoleID == roleModule.RoleSystem.Role.RoleID);
            if (parentRoleModule != null)
            {
                parentRoleModule.IsActive = false;
                if (parentRoleModule.Module.ModuleID != parentRoleModule.Module.ParentModule.ModuleID)
                {
                    SetParentRoleModuleIsActiveFalse(parentRoleModule);
                }
            }
        }

        private void InitRoleFunctions(RoleModule roleModule)
        {
            foreach (var function in roleModule.Module.Functions)
            {
                var roleFunctions = roleModule.RoleFunctions.Where(rf => rf.Function.FunctionID == function.FunctionID);
                if (roleFunctions.Count() == 0)
                {
                    RoleFunction rf = new RoleFunction()
                    {
                        RoleFunctionID = Guid.NewGuid(),
                        RoleModule = roleModule,
                        Function = function,
                        IsActive = false
                    };                    
                    roleModule.RoleSystem.IsActive = false;
                    SetParentRoleModuleIsActiveFalse(roleModule);
                    roleModule.IsActive = false;                    
                    RoleFunctionRepository.Add(rf);
                    RoleFunctionRepository.SaveChanges();
                }
            }
        }

        #endregion

        #region 初始化用户权限

        private void InitUserSystem(User user, City city,THOK.Authority.DbModel.System system)
        {
            var userSystems = user.UserSystems.Where(us => us.City.CityID == city.CityID
                && us.System.SystemID == system.SystemID);
            if (userSystems.Count() == 0)
            {
                UserSystem us = new UserSystem()
                {
                    UserSystemID = Guid.NewGuid(),
                    User = user,
                    City = city,
                    System = system,
                    IsActive = user.UserName == "Admin"
                };
                UserSystemRepository.Add(us);
                UserSystemRepository.SaveChanges();
            }
            var userSystem = user.UserSystems.Single(us => us.City.CityID == city.CityID
                && us.System.SystemID == system.SystemID);
            InitUserModule(userSystem);
        }

        private void InitUserModule(UserSystem userSystem)
        {
            foreach (var module in userSystem.System.Modules)
            {
                var userModules = userSystem.UserModules.Where(um => um.Module.ModuleID == module.ModuleID
                    && um.UserSystem.UserSystemID == userSystem.UserSystemID);
                if (userModules.Count() == 0)
                {
                    UserModule um = new UserModule()
                    {
                        UserModuleID = Guid.NewGuid(),
                        UserSystem = userSystem,
                        Module = module,
                        IsActive = userSystem.User.UserName == "Admin"
                    };
                    userSystem.IsActive = userSystem.User.UserName == "Admin";
                    SetParentUserModuleIsActiveFalse(um);
                    UserModuleRepository.Add(um);
                    UserModuleRepository.SaveChanges();
                }
                var userModule = userSystem.UserModules.Single(um => um.Module.ModuleID == module.ModuleID
                    && um.UserSystem.UserSystemID == userSystem.UserSystemID);
                InitUserFunctions(userModule);
            }
        }

        private void SetParentUserModuleIsActiveFalse(UserModule userModule)
        {
            var parentUserModule = userModule.Module.ParentModule.UserModules.FirstOrDefault(pum => pum.UserSystem.User.UserID == userModule.UserSystem.User.UserID);
            if (parentUserModule != null)
            {
                parentUserModule.IsActive = false;
                if (parentUserModule.Module.ModuleID != parentUserModule.Module.ParentModule.ModuleID)
                {
                    SetParentUserModuleIsActiveFalse(parentUserModule);
                }
            }
        }

        private void InitUserFunctions(UserModule userModule)
        {
            foreach (var function in userModule.Module.Functions)
            {
                var userFunctions = userModule.UserFunctions.Where(uf => uf.Function.FunctionID == function.FunctionID);
                if (userFunctions.Count() == 0)
                {
                    UserFunction uf = new UserFunction()
                    {
                        UserFunctionID = Guid.NewGuid(),
                        UserModule = userModule,
                        Function = function,
                        IsActive = userModule.UserSystem.User.UserName == "Admin"
                    };
                    userModule.UserSystem.IsActive = userModule.UserSystem.User.UserName == "Admin";
                    SetParentUserModuleIsActiveFalse(userModule);
                    userModule.IsActive = userModule.UserSystem.User.UserName == "Admin";     
                    UserFunctionRepository.Add(uf);
                    UserFunctionRepository.SaveChanges();
                }
            }
        }

        #endregion

        #region

        private void SetMenu(Menu menu, Module module)
        {
            IQueryable<THOK.Authority.DbModel.RoleModule> queryRoleModule = RoleModuleRepository.GetQueryable();
            HashSet<Menu> childMenuSet = new HashSet<Menu>();
            var modules = from m in module.Modules
                          orderby m.ShowOrder
                          select m;
            foreach (var item in modules)
            {
                if (item != module)
                {
                    Menu childMenu = new Menu();
                    childMenu.ModuleID = item.ModuleID.ToString();
                    childMenu.ModuleName = item.ModuleName;
                    childMenu.SystemID = item.System.SystemID.ToString();
                    childMenu.SystemName = item.System.SystemName;
                    childMenu.ParentModuleID = item.ParentModule.ModuleID.ToString();
                    childMenu.ParentModuleName = item.ParentModule.ModuleName;
                    childMenu.ModuleURL = item.ModuleURL;
                    childMenu.iconCls = item.IndicateImage;
                    childMenu.ShowOrder = item.ShowOrder;
                    childMenuSet.Add(childMenu);
                    if (item.Modules.Count > 0)
                    {
                        SetMenu(childMenu, item);
                    }
                }
            }
            menu.children = childMenuSet.ToArray();
        }

        private void SetFunTree(Tree childTree, Module item,RoleModule roleModules)
        {
            var function = FunctionRepository.GetQueryable().Where(f => f.Module.ModuleID == item.ModuleID);
            IQueryable<THOK.Authority.DbModel.RoleFunction> queryRoleFunction = RoleFunctionRepository.GetQueryable();
            HashSet<Tree> functionTreeSet = new HashSet<Tree>();
            foreach (var func in function)
            {
                Tree funcTree = new Tree();
                var roleFunction = queryRoleFunction.FirstOrDefault(rf => rf.Function.FunctionID == func.FunctionID&&rf.RoleModule.RoleModuleID==roleModules.RoleModuleID);
                funcTree.id = roleFunction.RoleFunctionID.ToString();
                funcTree.text = "功能：" + func.FunctionName;
                funcTree.@checked = roleFunction == null ? false : roleFunction.IsActive;
                funcTree.attributes = "function";
                functionTreeSet.Add(funcTree);
            }
            childTree.children = functionTreeSet.ToArray();
        }

        public void InitRoleSys(string roleID, string cityID, string systemID)
        {
            IQueryable<THOK.Authority.DbModel.Role> queryRole = RoleRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.City> queryCity = CityRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.System> querySystem = SystemRepository.GetQueryable();
            var role = queryRole.Single(i => i.RoleID == new Guid(roleID));
            var city = queryCity.Single(i => i.CityID == new Guid(cityID));
            var system = querySystem.Single(i => i.SystemID == new Guid(systemID));
            InitRoleSystem(role, city, system);
        }

        public object GetRoleSystemDetails(string roleID,string cityID, string systemID)
        {
            IQueryable<THOK.Authority.DbModel.System> querySystem = SystemRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.Module> queryModule = ModuleRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.RoleSystem> queryRoleSystem = RoleSystemRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.RoleModule> queryRoleModule = RoleModuleRepository.GetQueryable();
            var systems = querySystem.Single(i => i.SystemID == new Guid(systemID));
            var roleSystems = queryRoleSystem.FirstOrDefault(i => i.System.SystemID == new Guid(systemID)&&i.Role.RoleID==new Guid(roleID)&&i.City.CityID==new Guid(cityID));
            HashSet<Tree> RolesystemTreeSet = new HashSet<Tree>();
            Tree roleSystemTree = new Tree();
            roleSystemTree.id = roleSystems.RoleSystemID.ToString();
            roleSystemTree.text = "系统：" + systems.SystemName;
            roleSystemTree.@checked = roleSystems.IsActive;
            roleSystemTree.attributes = "system";

            var modules = queryModule.Where(m => m.System.SystemID == systems.SystemID && m.ModuleID == m.ParentModule.ModuleID)
                                     .OrderBy(m => m.ShowOrder)
                                     .Select(m => m);
            HashSet<Tree> moduleTreeSet = new HashSet<Tree>();
            foreach (var item in modules)
            {
                Tree moduleTree = new Tree();
                string moduleID = item.ModuleID.ToString();
                var roleModules = queryRoleModule.FirstOrDefault(i => i.Module.ModuleID == new Guid(moduleID)&&i.RoleSystem.RoleSystemID==roleSystems.RoleSystemID);
                moduleTree.id = roleModules.RoleModuleID.ToString();
                moduleTree.text = "模块：" + item.ModuleName;
                moduleTree.@checked = roleModules.IsActive;
                moduleTree.attributes = "module";

                moduleTreeSet.Add(moduleTree);
                SetTree(moduleTree, item,roleSystems);
                moduleTreeSet.Add(moduleTree);
            }
            roleSystemTree.children = moduleTreeSet.ToArray();
            RolesystemTreeSet.Add(roleSystemTree);
            return RolesystemTreeSet.ToArray();
        }

        private void SetTree(Tree tree,Module module,RoleSystem roleSystems)
        {
            IQueryable<THOK.Authority.DbModel.RoleModule> queryRoleModule = RoleModuleRepository.GetQueryable();
            HashSet<Tree> childTreeSet = new HashSet<Tree>();
            var modules = from m in module.Modules
                          orderby m.ShowOrder
                          select m;
            foreach (var item in modules)
            {
                if (item != module)
                {
                    Tree childTree = new Tree();
                    string moduleID = item.ModuleID.ToString();
                    var roleModules = queryRoleModule.FirstOrDefault(i => i.Module.ModuleID == new Guid(moduleID)&&i.RoleSystem.RoleSystemID==roleSystems.RoleSystemID);
                    childTree.id = roleModules.RoleModuleID.ToString();
                    childTree.text = "模块：" + item.ModuleName;
                    childTree.@checked = roleModules == null ? false : roleModules.IsActive;
                    childTree.attributes = "module";
                    childTreeSet.Add(childTree);
                    if (item.Modules.Count > 0)
                    {
                        SetTree(childTree, item,roleSystems);
                    }
                    if (item.Functions.Count > 0)
                    {
                        SetFunTree(childTree, item,roleModules);
                    }
                }
            }
            tree.children = childTreeSet.ToArray();
        }

        public bool ProcessRolePermissionStr(string rolePermissionStr)
        {
            string[] rolePermissionList = rolePermissionStr.Split(',');
            string type;
            string id;
            bool isActive;
            bool result = false;
            for (int i = 0; i < rolePermissionList.Length - 1; i++)
            {
                string[] rolePermission = rolePermissionList[i].Split('^');
                type = rolePermission[0];
                id = rolePermission[1];
                isActive = Convert.ToBoolean(rolePermission[2]);
                UpdateRolePermission(type, id, isActive);
                result = true;
            }
            return result;
        }

        public bool UpdateRolePermission(string type, string id, bool isActive)
        {
            bool result = false;
            if (type=="system")
            {
                IQueryable<THOK.Authority.DbModel.RoleSystem> queryRoleSystem = RoleSystemRepository.GetQueryable();
                Guid sid = new Guid(id);
                var system = queryRoleSystem.FirstOrDefault(i => i.RoleSystemID== sid);
                system.IsActive = isActive;
                RoleSystemRepository.SaveChanges();
                result = true;
            }
            else if (type=="module")
            {
                IQueryable<THOK.Authority.DbModel.RoleModule> queryRoleModule = RoleModuleRepository.GetQueryable();
                Guid mid = new Guid(id);
                var module = queryRoleModule.FirstOrDefault(i => i.RoleModuleID == mid);
                module.IsActive = isActive;
                RoleModuleRepository.SaveChanges();
                result = true;
            }
            else if (type=="function")
            {
                IQueryable<THOK.Authority.DbModel.RoleFunction> queryRoleFunction = RoleFunctionRepository.GetQueryable();
                Guid fid = new Guid(id);
                var system = queryRoleFunction.FirstOrDefault(i => i.RoleFunctionID== fid);
                system.IsActive = isActive;
                RoleSystemRepository.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool InitUserSystemInfo(string userID, string cityID, string systemID)
        {
            var user = UserRepository.GetQueryable().Single(u => u.UserID == new Guid(userID));
            var city = CityRepository.GetQueryable().Single(c => c.CityID == new Guid(cityID));
            var system = SystemRepository.GetQueryable().Single(s => s.SystemID == new Guid(systemID));
            InitUserSystem(user, city, system);
            return true;
        }
        
        public object GetUserSystemDetails(string userID,string cityID,string systemID)
        {
            IQueryable<THOK.Authority.DbModel.System> querySystem = SystemRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.Module> queryModule = ModuleRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.UserSystem> queryUserSystem = UserSystemRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.UserModule> queryUserModule = UserModuleRepository.GetQueryable();
            var systems = querySystem.Single(i => i.SystemID == new Guid(systemID));
            var userSystems = queryUserSystem.FirstOrDefault(i => i.System.SystemID == new Guid(systemID) && i.User.UserID == new Guid(userID) && i.City.CityID == new Guid(cityID));
            HashSet<Tree> userSystemTreeSet = new HashSet<Tree>();
            Tree userSystemTree = new Tree();
            userSystemTree.id = userSystems.UserSystemID.ToString();
            userSystemTree.text = "系统：" + systems.SystemName;            
            userSystemTree.@checked = userSystems.IsActive;
            userSystemTree.attributes = "system";

            var modules = queryModule.Where(m => m.System.SystemID == systems.SystemID && m.ModuleID == m.ParentModule.ModuleID)
                                     .OrderBy(m => m.ShowOrder)
                                     .Select(m => m);
            HashSet<Tree> moduleTreeSet = new HashSet<Tree>();
            foreach (var item in modules)
            {
                Tree moduleTree = new Tree();
                string moduleID = item.ModuleID.ToString();
                var userModules = queryUserModule.FirstOrDefault(i => i.Module.ModuleID == new Guid(moduleID) && i.UserSystem.UserSystemID == userSystems.UserSystemID);
                moduleTree.id = userModules.UserModuleID.ToString();
                moduleTree.text = "模块：" + item.ModuleName;                
                moduleTree.@checked = userModules.IsActive;
                moduleTree.attributes = "module";

                moduleTreeSet.Add(moduleTree);
                SetModuleTree(moduleTree, item, userSystems);
                moduleTreeSet.Add(moduleTree);
            }
            userSystemTree.children = moduleTreeSet.ToArray();
            userSystemTreeSet.Add(userSystemTree);
            return userSystemTreeSet.ToArray();
        }

        private void SetModuleTree(Tree tree, Module module,UserSystem userSystems)
        {
            HashSet<Tree> childTreeSet = new HashSet<Tree>();
            var modules = from m in module.Modules
                          orderby m.ShowOrder
                          select m;
            foreach (var item in modules)
            {
                if (item != module)
                {
                    Tree childTree = new Tree();
                    string moduleID = item.ModuleID.ToString();
                    var userModules = UserModuleRepository.GetQueryable().FirstOrDefault(i => i.Module.ModuleID == new Guid(moduleID) && i.UserSystem.UserSystemID == userSystems.UserSystemID);
                    childTree.id = userModules.UserModuleID.ToString();
                    childTree.text = "模块：" + item.ModuleName;                   
                    childTree.@checked = userModules == null ? false : userModules.IsActive;
                    childTree.attributes = "module";
                    childTreeSet.Add(childTree);
                    if (item.Modules.Count > 0)
                    {
                        SetModuleTree(childTree, item, userSystems);
                    }
                    if (item.Functions.Count > 0)
                    {
                        SetModuleFunTree(childTree, item, userModules);
                    }
                }
            }
            tree.children = childTreeSet.ToArray();
        }

        private void SetModuleFunTree(Tree childTree, Module item,UserModule userModules)
        {
            var function = FunctionRepository.GetQueryable().Where(f => f.Module.ModuleID == item.ModuleID);            
            HashSet<Tree> functionTreeSet = new HashSet<Tree>();
            foreach (var func in function)
            {
                Tree funcTree = new Tree();
                var userFunction = UserFunctionRepository.GetQueryable().FirstOrDefault(rf => rf.Function.FunctionID == func.FunctionID && rf.UserModule.UserModuleID == userModules.UserModuleID);
                funcTree.id = userFunction.UserFunctionID.ToString();
                funcTree.text = "功能：" + func.FunctionName;                
                funcTree.@checked = userFunction == null ? false : userFunction.IsActive;
                funcTree.attributes = "function";
                functionTreeSet.Add(funcTree);
            }
            childTree.children = functionTreeSet.ToArray();
        }

        public bool ProcessUserPermissionStr(string userPermissionStr)
        {
            string[] rolePermissionList = userPermissionStr.Split(',');
            string type;
            string id;
            bool isActive;
            bool result = false;
            for (int i = 0; i < rolePermissionList.Length - 1; i++)
            {
                string[] rolePermission = rolePermissionList[i].Split('^');
                type = rolePermission[0];
                id = rolePermission[1];
                isActive = Convert.ToBoolean(rolePermission[2]);
                UpdateUserPermission(type, id, isActive);
                result = true;
            }
            return result;
        }

        private bool UpdateUserPermission(string type, string id, bool isActive)
        {
            bool result = false;
            if (type == "system")
            {
                IQueryable<THOK.Authority.DbModel.UserSystem> queryUserSystem = UserSystemRepository.GetQueryable();
                Guid sid = new Guid(id);
                var system = queryUserSystem.FirstOrDefault(i => i.UserSystemID == sid);
                system.IsActive = isActive;
                RoleSystemRepository.SaveChanges();
                result = true;
            }
            else if (type == "module")
            {
                IQueryable<THOK.Authority.DbModel.UserModule> queryUserModule = UserModuleRepository.GetQueryable();
                Guid mid = new Guid(id);
                var module = queryUserModule.FirstOrDefault(i => i.UserModuleID == mid);
                module.IsActive = isActive;
                RoleModuleRepository.SaveChanges();
                result = true;
            }
            else if (type == "function")
            {
                IQueryable<THOK.Authority.DbModel.UserFunction> queryUserFunction = UserFunctionRepository.GetQueryable();
                Guid fid = new Guid(id);
                var system = queryUserFunction.FirstOrDefault(i => i.UserFunctionID == fid);
                system.IsActive = isActive;
                RoleSystemRepository.SaveChanges();
                result = true;
            }
            return result;
        }

        #endregion


        #region IModuleService 成员


        public object GetDetails2(int page, int rows, string QueryString, string Value)
        {
            string SystemName = "";
            string ModuleName = "";
            if (QueryString == "SystemName")
            {
                SystemName = Value;
            }
            else
            {
                ModuleName = Value;
            }
            IQueryable<Module> ModuleQuery = ModuleRepository.GetQueryable();
            var Module = ModuleQuery.Where(c => c.System.SystemName.Contains(SystemName) && c.ModuleName.Contains(ModuleName))
                .OrderBy(c => c.ModuleName)
                .Select(c => new
                {
                   c.ModuleID,
                   c.ModuleName,
                   c.ModuleURL,
                   ParentModule = c.ParentModule.ModuleName,
                   c.System.SystemName
                });
            int total = Module.Count();
            Module = Module.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = Module.ToArray() };
        }

        #endregion

        public System.Data.DataTable GetModules(int page, int rows, Module module, bool systemIdIsNull)
        {
            IQueryable<Module> moduleQuery = ModuleRepository.GetQueryable();

            var moduleDetail = moduleQuery;
            if (systemIdIsNull == false)
            {
                moduleDetail = moduleDetail.Where(m => m.System_SystemID == module.System_SystemID);
            }

            var system = moduleDetail.ToArray().OrderBy(s=>s.ShowOrder).Select(s => new {
                ModuleName=s.System.SystemName,
                IndicateImage="",
                ModuleURL="",
                ShowOrder=""
            }).Distinct();

            //var ParentModule = moduleDetail.Where(s => s.ParentModule_ModuleID == s.ModuleID).ToArray()
            //    .OrderBy(s => s.ShowOrder)
            //    .Select(p => new
            //    {
            //        ModuleName = "    |---" + p.ModuleName,
            //        p.IndicateImage,
            //        p.ModuleURL,
            //        p.ShowOrder,
            //        p.ModuleID
            //    });

            //var module_Detail = moduleDetail.Where(s => s.ParentModule_ModuleID != s.ModuleID).ToArray()
            //    .OrderBy(s => s.ShowOrder)
            //    .Select(m => new
            //    {
            //        ModuleName ="        |---"+m.ModuleName,
            //        IndicateImage="        |---"+m.IndicateImage,
            //        ModuleURL="        |---"+m.ModuleURL,
            //        ShowOrder = "        |---" + m.ShowOrder
            //    });
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("模块名称", typeof(string));
            dt.Columns.Add("图标", typeof(string));
            dt.Columns.Add("路径", typeof(string));
            dt.Columns.Add("显标顺序", typeof(string));
            foreach (var sys in system)
            {
                dt.Rows.Add
                    (
                        sys.ModuleName,
                        sys.IndicateImage,
                        sys.ModuleURL,
                        sys.ShowOrder
                    );
                var ParentModule = moduleDetail.Where(s => s.ParentModule_ModuleID == s.ModuleID 
                    && s.System.SystemName==sys.ModuleName).ToArray()
                    .OrderBy(s => s.ShowOrder)
                    .Select(p => new
                    {
                        ModuleName = "    |---" + p.ModuleName,
                        p.IndicateImage,
                        p.ModuleURL,
                        p.ShowOrder,
                        p.ModuleID
                    });
                foreach (var pm in ParentModule)
                {
                    dt.Rows.Add
                    (
                        pm.ModuleName,
                        pm.IndicateImage,
                        pm.ModuleURL,
                        pm.ShowOrder
                    );
                    var module_Detail = moduleDetail.Where(s => s.ParentModule_ModuleID != s.ModuleID
                        && s.ParentModule_ModuleID == pm.ModuleID).ToArray()
                        .OrderBy(s => s.ShowOrder)
                        .Select(m => new
                        {
                            ModuleName = "    |---------" + m.ModuleName,
                            m.IndicateImage,
                            m.ModuleURL,
                            m.ShowOrder
                        });
                    foreach (var m in module_Detail)
                    {
                        dt.Rows.Add
                        (
                            m.ModuleName,
                            m.IndicateImage,
                            m.ModuleURL,
                            m.ShowOrder
                        );
                    }
                }
            }
            return dt;
        }
    }
}
