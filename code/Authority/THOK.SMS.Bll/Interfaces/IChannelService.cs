using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.SMS.DbModel;

namespace THOK.SMS.Bll.Interfaces
{
    public interface IChannelService : IService<Channel>
    {
        object GetDetails(int page, int rows, Channel Channel);
    }
}
