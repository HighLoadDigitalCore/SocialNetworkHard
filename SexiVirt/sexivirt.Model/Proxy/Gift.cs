using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sexivirt.Model
{ 
    public partial class Gift
    {
        public enum TypeGift
        {
            Popular = 0x01, 
            Sexual = 0x02, 
            Romantic = 0x03
        }

        public string TypeStr
        {
            get
            {
                switch ((TypeGift)Type)
                {
                    case TypeGift.Popular:
                        return "����������";
                    case TypeGift.Sexual :
                        return "�����������";
                    case TypeGift.Romantic:
                        return "�����������";
                }

                return string.Empty;
            }
        }
	}
}