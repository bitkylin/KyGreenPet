using System;
using cn.bmob.io;

namespace KyGreenPet.bean
{
    public class KyCommand : BmobTable
    {
        public BmobBoolean isOpenedLed { set; get; } = true;

        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);
            isOpenedLed = input.getBoolean("isOpenedLed");
        }

        //写字段信息
        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);
            output.Put("isOpenedLed", isOpenedLed);
        }
    }
}