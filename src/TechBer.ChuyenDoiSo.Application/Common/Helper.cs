using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using NPOI.SS.Formula.Functions;


namespace TechBer.ChuyenDoiSo.Common
{
     public static class Helper
    {
        public static string GetCurrentTimeStamp()
        {
            return ((Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString();
        }
        public static bool isEmpty<T>(this List<T> list)
        {
            return !list.Any();
        }

        public static bool isNull<T>(this T objects)
        {
            return (objects == null);
        }
        public static string toCurrency<T>(this T objects)
        {
            //return String.Format("{0:n0}", objects).Replace(".",",");
            return String.Format("{0:n0}", objects);
        }
        
        private static double rad(double x)
        {
            return x * Math.PI / 180;
        }
        public class KarionCustomLocation
        {
            public double lattitude { get; set; }
            public double longtitude { get; set; }
        }
        public static double getDistance(KarionCustomLocation p1, KarionCustomLocation p2)
        {
            var R = 6378137; // Earth’s mean radius in meter
            var dLat = rad(p2.lattitude - p1.lattitude);
            var dLong = rad(p2.longtitude - p1.longtitude);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(rad(p1.lattitude)) * Math.Cos(rad(p2.lattitude)) *
                    Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return Math.Round(d,2); // returns the distance in meter
        }
        
      
        private static string[] ChuSo = new string[10] { " không", " một", " hai", " ba"," bốn", " năm", " sáu", " bẩy", " tám", " chín" };
        private static string[] Tien = new string[6] { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };
        // Hàm đọc số thành chữ
        public static string DocTienBangChu(long SoTien, string strTail)
        {
            int lan, i;
            long so;
            string KetQua = "", tmp = "";
            int[] ViTri = new int[6];
            if (SoTien < 0) return "Số tiền âm !";
            if (SoTien == 0) return "Không đồng !";
            if (SoTien > 0)
            {
                so = SoTien;
            }
            else
            {
                so = -SoTien;
            }
            //Kiểm tra số quá lớn
             if (SoTien > 8999999999999999)
            {
                SoTien = 0;
             return "";
            }
            ViTri[5] = (int)(so / 1000000000000000);
            so = so - long.Parse(ViTri[5].ToString()) * 1000000000000000;
            ViTri[4] = (int)(so / 1000000000000);
            so = so - long.Parse(ViTri[4].ToString()) * +1000000000000;
            ViTri[3] = (int)(so / 1000000000);
            so = so - long.Parse(ViTri[3].ToString()) * 1000000000;
            ViTri[2] = (int)(so / 1000000);
            ViTri[1] = (int)((so % 1000000) / 1000);
            ViTri[0] = (int)(so % 1000);
            if (ViTri[5] > 0)
            {
                lan = 5;
            }
            else if (ViTri[4] > 0)
            {
                lan = 4;
            }
            else if (ViTri[3] > 0)
            {
                lan = 3;
            }
            else if (ViTri[2] > 0)
            {
                lan = 2;
            }
            else if (ViTri[1] > 0)
            {
                lan = 1;
            }
            else
            {
                lan = 0;
            }
            for (i = lan; i >= 0; i--)
            {
                tmp = DocSo3ChuSo(ViTri[i]);
                KetQua += tmp;
                if (ViTri[i] != 0) KetQua += Tien[i];
                if ((i > 0) && (!string.IsNullOrEmpty(tmp))) KetQua += ",";//&& (!string.IsNullOrEmpty(tmp))
            }
            if (KetQua.Substring(KetQua.Length - 1, 1) == ",") KetQua = KetQua.Substring(0, KetQua.Length - 1);
            KetQua = KetQua.Trim() + strTail;
            return KetQua.Substring(0, 1).ToUpper() + KetQua.Substring(1);
        }
        // Hàm đọc số có 3 chữ số
        private static string DocSo3ChuSo(int baso)
        {
            int tram, chuc, donvi;
            string KetQua = "";
            tram = (int)(baso / 100);
            chuc = (int)((baso % 100) / 10);
            donvi = baso % 10;
            if ((tram == 0) && (chuc == 0) && (donvi == 0)) return "";
            if (tram != 0)
            {
                KetQua += ChuSo[tram] + " trăm";
                if ((chuc == 0) && (donvi != 0)) KetQua += " linh";
            }
            if ((chuc != 0) && (chuc != 1))
            {
                KetQua += ChuSo[chuc] + " mươi";
                if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh";
            }
            if (chuc == 1) KetQua += " mười";
            switch (donvi)
            {
                case 1:
                    if ((chuc != 0) && (chuc != 1))
                    {
                        KetQua += " mốt";
                    }
                    else
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
                case 5:
                    if (chuc == 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    else
                    {
                        KetQua += " lăm";
                    }
                    break;
                default:
                    if (donvi != 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
            }
            return KetQua;
        }
        
        public static String DinhDangSo(String Number)
        {
            if (Number.Length <= 3)
                return Number;
 
            int commasPos = 0;
            String tmpFormated = String.Empty, Formated = String.Empty;  
 
            for (int i = Number.Length - 1; i >= 0; i--)
            {
                commasPos++;
                tmpFormated += Number;
                if ((commasPos == 3) && (i != 0))
                {
                    tmpFormated += ",";
                    commasPos = 0;
                }                
            }
   
            for (int i = tmpFormated.Length - 1; i >= 0; i--)
            {
                Formated += tmpFormated;
            }
 
            return Formated;
        }
       
    }

    public class ConstValue_SoGhiChiSo
    {
        public const int TYPE_INSERT = 1;
        public const int TYPE_UPDATE_NON_OVERWRITE = 2;
        public const int TYPE_UPDATE_OVERWRITE  = 3;
    }
}