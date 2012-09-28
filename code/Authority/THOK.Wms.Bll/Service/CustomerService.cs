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

        #region ICustomerService 成员


       
        #endregion

        #region ICustomerService 成员


        public object GetDetails(int page, int rows, string CustomerCode,string CustomerName, string CompanyCode, string SaleRegionCode, string CustomerType, string CityOrCountryside, string LicenseCode, string IsActive)
        {
            IQueryable<Customer> customerQuery = CustomerRepository.GetQueryable();
            var customer = customerQuery.Where(c => c.CustomerCode.Contains(CustomerCode) &&
                                                          c.CustomerType.Contains(CustomerType) &&
                                                          c.SaleRegionCode.Contains(SaleRegionCode) &&
                                                          c.CityOrCountryside.Contains(CityOrCountryside) &&
                                                          c.CustomerName.Contains(CustomerName)&&
                                                          c.IsActive.Contains(IsActive));
            if (!CompanyCode.Equals(string.Empty))
            {
                customer = customer.Where(c => c.CompanyCode == CompanyCode);
            }
            if (!LicenseCode.Equals(string.Empty))
            {
                customer = customer.Where(c => c.LicenseCode == LicenseCode);
            }
            customer = customer.OrderBy(h => h.CustomerCode);
            int total = customer.Count();
            customer = customer.Skip((page - 1) * rows).Take(rows);

            var temp = customer.ToArray().Select(c => new
            {
                CustomerCode = c.CustomerCode,
                CustomCode = c.CustomCode,
                CustomerName = c.CustomerName,
                CompanyCode = c.CompanyCode,
                SaleRegionCode = c.SaleRegionCode,
                UniformCode = c.UniformCode,
                CustomerType = c.CustomerType,
                SaleScope = c.SaleScope,
                IndustryType = c.IndustryType,
                CityOrCountryside = c.CityOrCountryside,
                DeliverOrder = c.DeliverOrder,
                DeliverLineCode = c.DeliverLineCode,
                Address = c.Address,
                Phone=c.Phone,
                LicenseType=c.LicenseType,
                LicenseCode=c.LicenseCode,
                PrincipalName=c.PrincipalName,
                PrincipalPhone=c.PrincipalPhone,
                PrincipalAddress=c.PrincipalAddress,
                ManagementName=c.ManagementName,
                ManagementPhone=c.ManagementPhone,
                Bank=c.Bank,
                BankAccounts = c.BankAccounts,
                Description=c.Description,
                IsActive = c.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = c.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });
            return new { total, rows = temp.ToArray() };
        }

        #endregion

        #region ICustomerService 成员


        public bool Add(Customer customer, out string strResult)
        {
            strResult = string.Empty;
            bool result = false;
            var cust = new Customer();
            if (customer != null)
            {
                try
                {
                    cust.CustomerCode = customer.CustomerCode;
                    cust.CustomCode = customer.CustomCode;
                    cust.CustomerName = customer.CustomerName;
                    cust.CompanyCode = customer.CompanyCode;
                    cust.SaleRegionCode = customer.SaleRegionCode;
                    cust.UniformCode = customer.UniformCode;
                    cust.CustomerType = customer.CustomerType;
                    cust.SaleScope = customer.SaleScope;
                    cust.IndustryType = customer.IndustryType;
                    cust.CityOrCountryside = customer.CityOrCountryside;
                    cust.DeliverLineCode = customer.DeliverLineCode;
                    cust.DeliverOrder = customer.DeliverOrder;
                    cust.Address = customer.Address;
                    cust.Phone = customer.Phone;
                    cust.LicenseType = customer.LicenseType;
                    cust.LicenseCode = customer.LicenseCode;
                    cust.PrincipalName = customer.PrincipalName;
                    cust.PrincipalPhone = customer.PrincipalPhone;
                    cust.PrincipalAddress = customer.PrincipalAddress;
                    cust.ManagementName = customer.ManagementName;
                    cust.ManagementPhone = customer.ManagementPhone;
                    cust.Bank = customer.Bank;
                    cust.BankAccounts = customer.BankAccounts;
                    cust.Description = customer.Description;
                    cust.IsActive = customer.IsActive;
                    cust.UpdateTime = DateTime.Now;

                    CustomerRepository.Add(cust);
                    CustomerRepository.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    strResult = "原因：" + ex.Message;
                }
            }
            return result;
        }

        #endregion

        #region ICustomerService 成员


        public object C_Details(int page, int rows, string QueryString, string Value)
        {
            string CustomerCode = "";
            string CustomerName = "";
            if (QueryString == "CustomerCode")
            {
                CustomerCode = Value;
            }
            else
            {
                CustomerName = Value;
            }
            IQueryable<Customer> customerQuery = CustomerRepository.GetQueryable();
            var customer = customerQuery.Where(c => c.CustomerCode.Contains(CustomerCode) && c.CustomerName.Contains(CustomerName))
                .OrderBy(c => c.CompanyCode)
                .Select(c => c);
            if (!CustomerCode.Equals(string.Empty))
            {
                customer = customer.Where(p => p.CustomerCode == CustomerCode);
            }
            int total = customer.Count();
            customer = customer.Skip((page - 1) * rows).Take(rows);

            var temp = customer.ToArray().Select(c => new
            {

                CustomerCode = c.CustomerCode,
                CustomCode = c.CustomCode,
                CustomerName = c.CustomerName,
                CompanyCode = c.CompanyCode,
                IsActive = c.IsActive == "1" ? "可用" : "不可用"
            });
            return new { total, rows = temp.ToArray() };
        }

        #endregion

        #region ICustomerService 成员

        public bool Delete(string CustomerCode)
        {
            var customer = CustomerRepository.GetQueryable()
               .FirstOrDefault(i => i.CustomerCode == CustomerCode);
            if (CustomerCode != null)
            {
                CustomerRepository.Delete(customer);
                CustomerRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        #endregion

        #region ICustomerService 成员


        public bool Save(Customer customer, out string strResult)
        {
            strResult = string.Empty;
            try
            {
                var cust= CustomerRepository.GetQueryable()
                    .FirstOrDefault(i => i.CustomerCode == customer.CustomerCode);
                cust.CustomCode = customer.CustomCode;
                cust.CustomerName = customer.CustomerName;
                cust.CompanyCode = customer.CompanyCode;
                cust.SaleRegionCode = customer.SaleRegionCode;
                cust.UniformCode = customer.UniformCode;
                cust.CustomerType = customer.CustomerType;
                cust.SaleScope = customer.SaleScope;
                cust.IndustryType = customer.IndustryType;
                cust.CityOrCountryside = customer.CityOrCountryside;
                cust.DeliverOrder = customer.DeliverOrder;
                cust.DeliverLineCode = customer.DeliverLineCode;
                cust.Address = customer.Address;
                cust.Phone = customer.Phone;
                cust.LicenseType = customer.LicenseType;
                cust.LicenseCode = customer.LicenseCode;
                cust.PrincipalName = customer.PrincipalName;
                cust.PrincipalPhone = customer.PrincipalPhone;
                cust.PrincipalAddress = customer.PrincipalAddress;
                cust.ManagementName = customer.ManagementName;
                cust.ManagementPhone = customer.ManagementPhone;
                cust.Bank = customer.Bank;
                cust.BankAccounts = customer.BankAccounts;
                cust.Description = customer.Description;
                cust.IsActive = customer.IsActive;
                cust.UpdateTime = DateTime.Now;
                CustomerRepository.SaveChanges();

            }
            catch (Exception ex)
            {
                strResult = "原因：" + ex.InnerException;
            }
            return true;
        }

        #endregion
    }
}
