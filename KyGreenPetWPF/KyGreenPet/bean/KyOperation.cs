using System;
using cn.bmob.io;

namespace KyGreenPet.bean
{
    public class KyOperation : BmobTable
    {
        public BmobDouble tempValue { set; get; }
        public BmobBoolean isHumidity { set; get; }
        public BmobBoolean isOpened { set; get; }
        public BmobBoolean isAuto { set; get; }
        public BmobDate dateTime { set; get; }

        public KyOperation()
        {

        }

        public KyOperation(double tempValue, bool isHumidity)
        {
            this.tempValue = tempValue;
            this.isHumidity = isHumidity;
            isOpened = true;
            dateTime = DateTime.Now;
        }


        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);
            tempValue = input.getDouble("tempValue");
            isHumidity = input.getBoolean("isHumidity");
            isOpened = input.getBoolean("isOpened");
            isAuto = input.getBoolean("isAuto");
            dateTime = input.getDate("dateTime");
        }

        //写字段信息
        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);

            output.Put("tempValue", tempValue);
            output.Put("isHumidity", isHumidity);
            output.Put("isOpened", isOpened);
            output.Put("isAuto", isAuto);
            output.Put("dateTime", dateTime);
        }
    }
}