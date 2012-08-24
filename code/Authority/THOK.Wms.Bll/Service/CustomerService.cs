using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Download.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class CustomerService : ServiceBase<Customer>, ICustomerService
    {
        [Dependency]
        public ICustomerRepository CustomerRepository { get; set; }

        [Dependency]
        public ICustomerDownService CustomerDownService { get; set; }
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool DownDeliverLine(out string errorInfo)
        {
            errorInfo = string.Empty;
            bool result = false;
            try
            {
                var customerCodes = CustomerRepository.GetQueryable().Where(c => c.CustomerCode == c.CustomerCode).Select(s => new { s.CustomerCode }).ToArray();
                string customerStrs = "";
                for (int i = 0; i < customerCodes.Length; i++)
                {
                    customerStrs += customerCodes[i].CustomerCode + ",";
                }
                Customer[] customerLines = CustomerDownService.GetCustomer(customerStrs);
                foreach (var item in customerLines)
                {
                    var customer = new Customer();
                    customer.CustomerCode = item.CustomerCode;
                    customer.CustomCode = item.CustomCode;
                    customer.CustomerName = item.CustomerName;
                    customer.CompanyCode = item.CompanyCode;
                    customer.SaleRegionCode = item.SaleRegionCode;
                    customer.UniformCode = item.UniformCode;
                    customer.CustomerType = item.CustomerType;
                    customer.SaleScope = item.SaleScope;
                    customer.IndustryType = item.IndustryType;
                    customer.CityOrCountryside = item.CityOrCountryside;
                    customer.DeliverLineCode = item.DeliverLineCode;
                    customer.DeliverOrder = item.DeliverOrder;
                    customer.Address = item.Address;
                    customer.Phone = item.Phone;
                    customer.LicenseCode = item.LicenseCode;
                    customer.LicenseType = item.LicenseType;
                    customer.PrincipalAddress = item.PrincipalAddress;
                    customer.PrincipalName = item.PrincipalName;
                    customer.PrincipalPhone = item.PrincipalPhone;
                    customer.ManagementName = item.ManagementName;
                    customer.ManagementPhone = item.ManagementPhone;
                    customer.Bank = item.Bank;
                    customer.BankAccounts = item.BankAccounts;
                    customer.Description = item.Description;
                    customer.IsActive = item.IsActive;
                    customer.UpdateTime = item.UpdateTime;
                    CustomerRepository.Add(customer);
                }
                CustomerRepository.SaveChanges();
                result = true;
            }
            catch (Exception e)
            {
                errorInfo = "出错，原因:" + e.Message;
            }
            return result;
        }
    }
}
