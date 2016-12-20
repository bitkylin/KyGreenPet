package cc.bitky.kygreenpet.bean;

import cn.bmob.v3.BmobObject;
import cn.bmob.v3.datatype.BmobDate;

public class KyOperation extends BmobObject {

  private Double tempValue;
  private Boolean isHumidity;
  private Boolean isOpened;
  private Boolean isAuto;
  private BmobDate dateTime;

  public Double getTempValue() {
    return tempValue;
  }

  public void setTempValue(Double tempValue) {
    this.tempValue = tempValue;
  }

  public Boolean getHumidity() {
    return isHumidity;
  }

  public void setHumidity(Boolean humidity) {
    isHumidity = humidity;
  }

  public Boolean getOpened() {
    return isOpened;
  }

  public void setOpened(Boolean opened) {
    isOpened = opened;
  }

  public Boolean getAuto() {
    return isAuto;
  }

  public void setAuto(Boolean auto) {
    isAuto = auto;
  }

  public BmobDate getDateTime() {
    return dateTime;
  }

  public void setDateTime(BmobDate dateTime) {
    this.dateTime = dateTime;
  }
}
