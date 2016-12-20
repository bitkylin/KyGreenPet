package cc.bitky.kygreenpet;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.CardView;
import android.view.View;
import android.widget.ImageButton;
import android.widget.TextView;
import cc.bitky.kygreenpet.bean.KyCommand;
import cc.bitky.kygreenpet.bean.KyOperation;
import cc.bitky.kygreenpet.util.KyToolBar;
import cc.bitky.kygreenpet.util.ToastUtil;
import cn.bmob.v3.BmobQuery;
import cn.bmob.v3.exception.BmobException;
import cn.bmob.v3.listener.QueryListener;
import cn.bmob.v3.listener.UpdateListener;
import cn.sharesdk.framework.ShareSDK;
import cn.sharesdk.onekeyshare.OnekeyShare;
import com.socks.library.KLog;

public class MainActivity extends AppCompatActivity {

  private ToastUtil toastUtil;
  private TextView textViewTemperature;
  private TextView textViewHumid;
  private CardView cardviewHumid;
  private CardView cardviewTemp;
  private TextView textViewCardviewTempShow;
  private ImageButton btnCardviewTemper;

  @Override protected void onCreate(Bundle savedInstanceState) {
    super.onCreate(savedInstanceState);
    setContentView(R.layout.activity_main);
    ShareSDK.initSDK(this, "199200678cdda");
    KyToolBar kyToolBar = (KyToolBar) findViewById(R.id.kyGreenPet_toolbar);
    kyToolBar.setRightButtonOnClickListener(new View.OnClickListener() {
      @Override public void onClick(View v) {
        showShare();
      }
    });
    toastUtil = new ToastUtil(this);
    textViewTemperature = (TextView) findViewById(R.id.kyGreenPet_temperature);
    textViewHumid = (TextView) findViewById(R.id.kyGreenPet_humid);
    cardviewHumid = (CardView) findViewById(R.id.kyGreenPet_cardViewHumid_exception);
    cardviewTemp = (CardView) findViewById(R.id.kyGreenPet_cardViewtemper_exception);
    textViewCardviewTempShow =
        (TextView) findViewById(R.id.kyGreenPet_cardViewtemper_exception_show);
    btnCardviewTemper = (ImageButton) findViewById(R.id.kyGreenPet_cardViewtemper_exception_button);
    ImageButton btnCardviewHumid = (ImageButton) findViewById(R.id.kyGreenPet_cardViewHumid_button);

    btnCardviewHumid.setOnClickListener(new View.OnClickListener() {
      @Override public void onClick(View v) {
        KyCommand kyCommand = new KyCommand();
        kyCommand.setOpenedLed(true);
        kyCommand.update("6402c5a401", new UpdateListener() {
          @Override public void done(BmobException e) {
            if (e != null) {
              toastUtil.show(e.getMessage());
            }
          }
        });
      }
    });
    new Thread(new Runnable() {
      @Override public void run() {
        while (true) {
          try {
            Thread.sleep(6000);
          } catch (InterruptedException e) {
            toastUtil.show(e.getMessage());
          }
          queryDataFromBmob();
        }
      }
    }).start();
  }

  private void queryDataFromBmob() {
    BmobQuery<KyOperation> kyOperationBmobQuery = new BmobQuery<>();
    kyOperationBmobQuery.getObject("429af4fea3", new QueryListener<KyOperation>() {
      @Override public void done(KyOperation kyOperation, BmobException e) {
        if (e != null) {
          KLog.d(e.getMessage());
          return;
        }
        KLog.d(kyOperation.getObjectId());
        if (kyOperation != null && kyOperation.getOpened()) {
          textViewHumid.setVisibility(View.VISIBLE);
          setViewStatus(kyOperation);
        } else {
          runOnUiThread(new Runnable() {
            @Override public void run() {
              textViewTemperature.setText("未开启哦！");
              textViewHumid.setText("");
              cardviewHumid.setVisibility(View.GONE);
              cardviewTemp.setVisibility(View.GONE);
            }
          });
        }
      }
    });
  }

  private void setViewStatus(final KyOperation kyOperation) {
    runOnUiThread(new Runnable() {
      @Override public void run() {
        double tempValue = kyOperation.getTempValue();
        textViewTemperature.setText(tempValue + "°");
        if (kyOperation.getHumidity()) {
          textViewHumid.setText("湿润");
          cardviewHumid.setVisibility(View.GONE);
        } else {
          textViewHumid.setText("干燥");
          cardviewHumid.setVisibility(View.VISIBLE);
        }
        if (tempValue >= 15 && tempValue <= 25) {
          cardviewTemp.setVisibility(View.GONE);
        } else {
          cardviewTemp.setVisibility(View.VISIBLE);
          if (tempValue < 15) {
            textViewCardviewTempShow.setText("温度太低");
            btnCardviewTemper.setImageResource(R.mipmap.icon_button_sun);
          }
          if (tempValue > 25) {
            textViewCardviewTempShow.setText("温度太高");
            btnCardviewTemper.setImageResource(R.mipmap.icon_button_snow);
          }
        }
      }
    });
  }

  private void showShare() {
    ShareSDK.initSDK(this);
    OnekeyShare oks = new OnekeyShare();
    //关闭sso授权
    oks.disableSSOWhenAuthorize();

    // title标题，印象笔记、邮箱、信息、微信、人人网和QQ空间等使用
    oks.setTitle("绿色萌宠");
    // titleUrl是标题的网络链接，QQ和QQ空间等使用
    oks.setTitleUrl("http://sharesdk.cn");
    // text是分享文本，所有平台都需要这个字段
    oks.setText("绿色萌宠，你值得拥有");
    // imagePath是图片的本地路径，Linked-In以外的平台都支持此参数
    //oks.setImagePath("/sdcard/test.jpg");//确保SDcard下面存在此张图片
    // url仅在微信（包括好友和朋友圈）中使用
    oks.setUrl("http://sharesdk.cn");
    // comment是我对这条分享的评论，仅在人人网和QQ空间使用
    oks.setComment("绿色萌宠，你值得拥有");
    // site是分享此内容的网站名称，仅在QQ空间使用
    oks.setSite(getString(R.string.app_name));
    // siteUrl是分享此内容的网站地址，仅在QQ空间使用
    oks.setSiteUrl("http://sharesdk.cn");

    // 启动分享GUI
    oks.show(this);
  }
}
