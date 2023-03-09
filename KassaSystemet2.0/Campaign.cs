using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystemet2._0
{
    public class Campaign
    {
        public Campaign(int freeCampaignId, int productId, decimal campaignPrice, DateOnly startDate, DateOnly endDate)
        {
            _endDate = endDate;
            _productId = productId;
            _Campaignprice= campaignPrice;
            _startDate = startDate;
            _freeCampaignId= freeCampaignId;
        }
        
        private int _productId {get; set;}
        private decimal _Campaignprice { get; set;}
        private DateOnly _startDate { get; set;}
        private DateOnly _endDate { get; set;}
        private int _freeCampaignId { get; set;}

        public int GetCampaignProductId()
        {
            return _productId;
        }

        public decimal GetCampaignPrice() 
        { 
            return _Campaignprice; 
        }

        public DateOnly GetStartDate() 
        { 
            return _startDate;
        }

        public DateOnly GetEndDate()
        {
            return _endDate;
        }

        public int CampaignId()
        {
            return _freeCampaignId;
        }
    }
}
