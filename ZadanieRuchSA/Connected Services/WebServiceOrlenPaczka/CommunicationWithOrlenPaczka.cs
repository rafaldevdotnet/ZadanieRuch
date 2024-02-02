using WebServiceOrlenPaczka;

namespace ZadanieRuchSA.Connected_Services.WebServiceOrlenPaczka
{
    public class CommunicationWithOrlenPaczka
    {
        private const string _partnerId = "TEST004196";
        private const string _partnerKey = "37049A000D";

        private static List<PointPwR> _pointPwRs;
        public static List<PointPwR> pointPwRs
        {
            get 
            {
                if (_pointPwRs == null || _pointPwRs.Count == 0) GetAllPoints();
                return _pointPwRs; 
            }
            set { _pointPwRs = value; }
        }
        private static void GetAllPoints()
        {
            var ws = new WebServicePwRSoapClient(WebServicePwRSoapClient.EndpointConfiguration.WebServicePwRSoap);
            var points = ws.GiveMeAllRUCHWithFilledD1Async(_partnerId, _partnerKey).Result;
            if (points != null && points.Data != null)
            {
                pointPwRs = points.Data.ToList();
            }
        }
    }
}
