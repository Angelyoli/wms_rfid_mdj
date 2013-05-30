﻿using System;
using System.Linq;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Service
{
    public class ServerService : ServiceBase<Server>, IServerService
    {
        [Dependency]
        public IServerRepository ServerRepository { get; set; }
        [Dependency]
        public ICityRepository CityRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string serverName, string description, string url, string isActive)
        {
            IQueryable<THOK.Authority.DbModel.Server> query = ServerRepository.GetQueryable();
            var servers = query.Where(i => i.ServerName.Contains(serverName)
                && i.Description.Contains(description)
                && i.Url.Contains(url))
                .OrderBy(i => i.ServerID)
                .Select(i => new { i.ServerID, i.ServerName, i.City.CityID, i.City.CityName, i.Description, i.Url, IsActive = i.IsActive ? "启用" : "禁用" });

            int total = servers.Count();
            servers = servers.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = servers.ToArray() };
        }

        public bool Add(string serverName, string description, string url, bool isActive, string cityID)
        {
            Guid gCityID = new Guid(cityID);
            var city = CityRepository.GetQueryable().Single(c => c.CityID == gCityID);
            var server = new THOK.Authority.DbModel.Server()
            {
                ServerID = Guid.NewGuid(),
                ServerName = serverName,
                Description = description,
                Url = url,
                IsActive = isActive,
                City = city
            };
            ServerRepository.Add(server);
            ServerRepository.SaveChanges();
            return true;
        }

        public bool Delete(string serverID)
        {
            Guid gServerID = new Guid(serverID);
            var server = ServerRepository.GetQueryable()
                .FirstOrDefault(i => i.ServerID == gServerID);
            if (server != null)
            {
                ServerRepository.Delete(server);
                ServerRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(string serverID, string serverName, string description, string url, bool isActive, string cityID)
        {
            Guid gServerID = new Guid(serverID);
            Guid gCityID = new Guid(cityID);
            var city = CityRepository.GetQueryable().Single(c => c.CityID == gCityID);
            var server = ServerRepository.GetQueryable()
                .FirstOrDefault(i => i.ServerID == gServerID);
            server.ServerName = serverName;
            server.Description = description;
            server.Url = url;
            server.IsActive = isActive;
            server.City = city;
            ServerRepository.SaveChanges();
            return true;
        }

        public object GetServerById(string serverID)
        {
            Guid sid = new Guid(serverID);
            var server = ServerRepository.GetQueryable().FirstOrDefault(s => s.ServerID == sid);
            return server.ServerName;
        }

        public object GetDetails(string cityID,string serverID)
        {
            Guid cityid=new Guid(cityID);
            Guid serverid=new Guid(serverID);
            var server = ServerRepository.GetQueryable().Where(s => s.City.CityID == cityid && s.ServerID == serverid).Select(s => s.ServerID);
            var servers = ServerRepository.GetQueryable().Where(s => !server.Any(sv => sv == s.ServerID) && s.City.CityID == cityid).Select(s => new { s.ServerID, s.ServerName, s.Description, Status = s.IsActive ? "启用" : "禁用" });
            return servers.ToArray();
        }

        public System.Data.DataTable GetServer(int page, int rows, Server server, bool isactiveIsNull)
        {
            IQueryable<Server> serverQuery = ServerRepository.GetQueryable();

            var serverDetail = serverQuery.Where(s =>
                s.ServerName.Contains(server.ServerName)
                && s.Description.Contains(server.Description)
                && s.Url.Contains(server.Url));
            if (isactiveIsNull == false)
            {
                serverDetail = serverDetail.Where(s => s.IsActive == server.IsActive);
            }
            serverDetail = serverDetail.OrderBy(s => s.ServerName);
            var server_Detail = serverDetail.ToArray().Select(s => new
            {
                s.ServerName,
                s.City.CityName,
                s.Url,
                s.Description,
                IsActive = s.IsActive == true ? "启用" : "禁用"
            });

            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Columns.Add("服务器名称", typeof(string));
            dt.Columns.Add("地市名称", typeof(string));
            dt.Columns.Add("URL", typeof(string));
            dt.Columns.Add("描述", typeof(string));
            dt.Columns.Add("状态", typeof(string));
            foreach (var s in server_Detail)
            {
                dt.Rows.Add
                    (
                        s.ServerName,
                        s.CityName,
                        s.Url,
                        s.Description,
                        s.IsActive
                    );
            }
            return dt;
        }
    }
}
