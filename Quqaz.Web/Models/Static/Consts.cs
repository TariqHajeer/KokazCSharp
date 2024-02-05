using System.Collections.Generic;
using Quqaz.Web.Dtos.Common;

namespace Quqaz.Web.Models.Static
{
    public static class Consts
    {
        public readonly static NameAndIdDto[] OrderPlaceds = new NameAndIdDto[] { OrderPlace.Client, OrderPlace.Store, OrderPlace.Way, OrderPlace.Delivered, OrderPlace.CompletelyReturned, OrderPlace.PartialReturned, OrderPlace.Unacceptable, OrderPlace.Delayed };
        public readonly static NameAndIdDto[] MoneyPlaceds = new NameAndIdDto[] { MoneyPalce.OutSideCompany, MoneyPalce.WithAgent, MoneyPalce.InsideCompany, MoneyPalce.Delivered };
        /// <summary>
        /// this is for the mobile app to marke the country
        /// the key is the id in  the database 
        /// </summary>
        public readonly static Dictionary<int, int> CountryMap = new Dictionary<int, int>()
        {

            //الأنبار
            {
                1030,1
            },
            //موصل
            {
                1021,2
            },
            //دهوك
            {
                1019,3
            },
            //اربيل
            {
                1028,4
            }
            ,
            //كركوك
            {
                1024,5
            },
            //السليمانية
            {
                1020,6
            },
            //ديالي
            {
                1032,7
            },
            //بغداد            
            {
                1022,8
            },
            //واسط
            {
                1033,9
            },
            //بابل
            {
                1043,10
            },
            //كربلاء
            {
                1035,11
            }
            ,
            //القادسية or الديوانية
            {
                1036,12
            },
            //النجف
            {
                1034,13
            },
            //ميسان
            {
                1046,14
            },
            //ذي قارة
            {
                1045,15
            },
            //المثنى
            {
                1037,16
            },
            //البصرة
            {
                1040,17
            },
            // تكريت
            {
                1031,18
            },
        //صلاح الدين
            {
                1038,18
            },
            // الناصرية
            {
                1042,15
            }

        };
    }
}
