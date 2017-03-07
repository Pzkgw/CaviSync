using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace MainLib
{
    public static class BindClient
    {
        public static ServiceEndpoint Get(string s)
        {
            NetTcpBinding binding = null;
            binding = new NetTcpBinding(SecurityMode.None);

            binding.Name = "customTcpBinding";
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.ReceiveTimeout = TimeSpan.FromHours(3);
            binding.CloseTimeout = TimeSpan.FromHours(3);
            binding.OpenTimeout = TimeSpan.FromHours(3);
            binding.SendTimeout = TimeSpan.FromHours(3);
            binding.TransferMode = TransferMode.Streamed;

            ContractDescription contract = ContractDescription.GetContract(typeof(MainLib.IFileRepositoryService));

            ServiceEndpoint retVal = null;
            retVal = new ServiceEndpoint(contract, binding, new EndpointAddress(s));
            retVal.Name = "FileRepositoryService";

            return retVal;
        }

    }
}


//binding.Security = new NetTcpSecurity()
//{
//    Mode = SecurityMode.None
//};
