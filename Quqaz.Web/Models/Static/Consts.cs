using System.Collections.Generic;
using Quqaz.Web.Dtos.Common;

namespace Quqaz.Web.Models.Static
{
    public static class Consts
    {
        public readonly static NameAndIdDto[] OrderPlaceds = new NameAndIdDto[] { OrderPlace.Way, OrderPlace.Delivered, OrderPlace.CompletelyReturned };
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
            },
            //خارجي اربيل
            {
                1018,4
            },
            //خبات
            {
                1029,4
            },
            //سامراء
            {
                1039,18
            },
            //عمارة
            {
                1041,14
            },
            //كوت
            {
                1044,9
            },
            //سيروان
            {
                1047,4
            },
            //مخمور
            {
                1048,4
            },
            //مصيف
            {
                1049,4
            },
            //سوران
            {
                1050,4
            },
            //كوية
            {
                1051,4
            },
            //شقلاوة
            {
                1052,4
            },
            //ملا عمر
            {
                1053,4
            },
            //بارازان
            {
                1054,4
            },
            //سلافا ستي
            {
                1055,4
            },
            //كوير
            {
                1056,4
            },
            //برده رش
            {
                1057,4
            },
            //ديانا
            {
                1058,4
            },
            //خليفان
            {
                1059,4
            },
            //راوندوز
            {
                1060,4
            },
            //رانيا
            {
                1061,4
            },
            //طق طق
            {
                1062,4
            },
            //قلادزي
            {
                1063,4
            },
            //سماوة
            {
                1070,4
            },
            //مام زاوه
            {
                1072,4
            },
            //كلار
            {
                1073,5
            },


        };
    }
}
