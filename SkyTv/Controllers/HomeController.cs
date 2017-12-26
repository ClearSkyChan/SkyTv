using System;
using System.IO;
using System.Web.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using SkyTv.Models;

namespace SkyTv.Controllers
{
    public class HomeController : Controller
    {
        public static readonly string Token = "ClearSky";
        public static readonly string EncodingAesKey = "6bD0egpnTEQ8ypSlA6Hi2YtahfO6N8AXbyXx5AK9ZRV";
        public static readonly string AppId = "wxf8a95c336c15e901";

        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content("参数错误！");
            }

            #region 打包 PostModel 信息

            postModel.Token = Token;//根据自己后台的设置保持一致
            postModel.EncodingAESKey = EncodingAesKey;//根据自己后台的设置保持一致
            postModel.AppId = AppId;//根据自己后台的设置保持一致

            #endregion

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            var maxRecordCount = 10;

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new WxMessageHandler(Request.InputStream, postModel, maxRecordCount)
            {
                OmitRepeatedMessage = true
            };

            try
            {
                //执行微信处理过程
                messageHandler.Execute();
                return Content(messageHandler.ResponseDocument.ToString());//v0.7-
            }
            catch (Exception ex)
            {
                return Content("错了错了");
            }
        }

    }
}
