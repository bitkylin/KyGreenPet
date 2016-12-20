package cc.bitky.kygreenpet.bean;

import cn.bmob.v3.BmobObject;

public class KyCommand extends BmobObject {

  public Boolean isOpenedLed;

  public Boolean getOpenedLed() {
    return isOpenedLed;
  }

  public void setOpenedLed(Boolean openedLed) {
    isOpenedLed = openedLed;
  }
}
