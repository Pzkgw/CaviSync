
using System;
using System.ServiceModel;

namespace MainLib
{
    public class BindServer
    {
        
        public static NetTcpBinding Get()
        {           

            NetTcpBinding binding = null;
            binding = new NetTcpBinding(SecurityMode.None);

            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.ReceiveTimeout = TimeSpan.FromHours(3);
            binding.CloseTimeout = TimeSpan.FromHours(3);
            binding.OpenTimeout = TimeSpan.FromHours(3);
            binding.SendTimeout = TimeSpan.FromHours(3);
            binding.TransferMode = TransferMode.Streamed;

            return binding;
        }
    }
}
