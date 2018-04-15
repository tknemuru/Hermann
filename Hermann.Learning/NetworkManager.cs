using Accord.IO;
using Accord.Neuro.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Learning
{
    /// <summary>
    /// 学習ネットワークの管理機能を提供します。
    /// </summary>
    public class NetworkManager
    {
        /// <summary>
        /// DeepBeliefNetworkのファイル名
        /// </summary>
        private const string DeepBeliefNetworkFileName = "deep_belief_network";

        /// <summary>
        /// DeepBeliefNetwork
        /// </summary>
        private DeepBeliefNetwork DeepBeliefNetwork { get; set; }

        /// <summary>
        /// DeepBeliefNetworkを取得します。
        /// </summary>
        /// <returns>DeepBeliefNetwork</returns>
        public DeepBeliefNetwork GetDeepBeliefNetwork()
        {
            if(this.DeepBeliefNetwork == null)
            {
                this.DeepBeliefNetwork = Serializer.Load<DeepBeliefNetwork>(string.Format(LearningConfig.NetworkSavePath + @"/{0}.bin", "deep_belief_network"));
            }
            return this.DeepBeliefNetwork;
        }

        /// <summary>
        /// DeepBeliefNetworkを保存します。
        /// </summary>
        /// <param name="network">DeepBeliefNetwork</param>
        public void SaveDeepBeliefNetwork(DeepBeliefNetwork network)
        {
            var filePath = string.Format(LearningConfig.NetworkSavePath + @"/{0}_{1}.bin", DeepBeliefNetworkFileName, DateTime.Now.ToString("yyyyMMddhhmmss"));
            network.Save(filePath);
        }
    }
}
