using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Senparc.Weixin;
using Senparc.Weixin.Entities.Request;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;

namespace SkyTv.Models
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public class WxMessageHandler : MessageHandler<WxMessageContext>
    {
        private string _appId = "wxf8a95c336c15e901";
        public WxMessageHandler(RequestMessageBase requestMessage)
            : base(requestMessage)
        {
        }

        public WxMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {

        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            return responseMessage;
        }
        //文字处理
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {

            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            var msg = requestMessage.Content;
            if (msg.StartsWith("http"))
            {
                var url = "http://clearskychan.hk05.iis800.com/Tv/Media?url=" + msg;
                var target = "<a href='"+url+"'>点此此处查看</a>,祝您观影愉快！";
                responseMessage.Content = target;
            }
            return responseMessage;
        }
        //订阅
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
            responseMessage.Articles.Add(new Article
            {
                Title="使用指南",
                PicUrl = "https://mmbiz.qpic.cn/mmbiz_jpg/RRSo81kwBSEBmAcWIVjiacrF9e2ibND7Sicu8weEeX2RUiaW4K2JG4W0icTAW1UBmmSc543CzFK6JvrJ62icshKTsxWg/0?wx_fmt=jpeg",
                Url = "https://mp.weixin.qq.com/s/kuhE_1v5XR3vi6XZ4d58hQ"
            });
            responseMessage.ArticleCount = 1;
            return responseMessage;
        }
    }
}
     