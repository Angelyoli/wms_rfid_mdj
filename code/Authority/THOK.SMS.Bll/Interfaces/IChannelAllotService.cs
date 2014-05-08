using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.SMS.DbModel;

namespace THOK.SMS.Bll.Interfaces
{
    public interface IChannelAllotService : IService<ChannelAllot>
    {
        object GetDetails(int page, int rows, ChannelAllot ChannelAllot);
    }
}
